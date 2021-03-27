using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    [SerializeField] private float hitPoints = 100;
    //Input getter
    [SerializeField] private InputHandler inputhandler;
    [SerializeField] private Dictionary<string, int> inputsThisFrame = new Dictionary<string, int>();
    //Finite States Machine for animation
    private enum State { Idle, Walk, Jump, Go_Up, Go_Down, Normal_Attack } // all states
    private State state = State.Idle; // starting state
    //PLayer Components
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    //Inspector variable
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask hurtBox;
    [SerializeField] private float SPEED = 5f;
    [SerializeField] private float JUMP_VEL = 10f;
    //Game logic constant
    private float PADDING = 0.05f;
    //ground checker
    private RaycastHit2D groundCast;
    private bool isGrounded;
    // logic action variable
    private Action action = null;
    private int currentActionFrame = -1;
    // logic action constant
    private Dictionary<string, Action> ACTION_DICT = new Dictionary<string, Action>();
    // hit stunt variable
    private int currentHitStuntFrame = -1;

    // following classes are for action logic
    private class Attack
    {
        private int m_startingFrame;
        private int m_endingFrame;
        private Vector2 m_point;
        private Vector2 m_size;
        private float m_angle;
        private float m_damage;
        private int m_hitStunt;
        public Attack(int startingFrame, int endingFrame, Vector2 point, Vector2 size, float angle, float damage, int hitStunt)
        {
            m_startingFrame = startingFrame;
            m_endingFrame = endingFrame;
            m_point = point;
            m_size = size;
            m_angle = angle;
            m_damage = damage;
            m_hitStunt = hitStunt;
        }
        public Collider2D[] SearchAttack(Vector2 position)
        {
            return Physics2D.OverlapBoxAll(point: m_point + position, size: m_size, angle: m_angle);
        }
        public bool IsActive(int currentFrame)
        {
            return m_startingFrame <= currentFrame && currentFrame <= m_endingFrame;
        }
        public float GetDamage()
        {
            return m_damage;
        }
        public int GetHitStunt()
        {
            return m_hitStunt;
        }
    }
    private class Movement
    {
        private int m_startingFrame;
        private Vector2 m_force;
        public Movement(int startingFrame, Vector2 force)
        {
            this.m_startingFrame = startingFrame;
            this.m_force = force;
        }
        public bool IsActive(int currentFrame)
        {
            return m_startingFrame == currentFrame;
        }
        public void Move(Rigidbody2D rb)
        {
            rb.velocity = m_force;
        }
    }
    private class Action
    {
        private List<Attack> m_attacks;
        private List<Movement> m_movements;
        private int m_lastFrame;
        public Action (List<Attack> attacks, List<Movement> movements,int lastFrame)
        {
            m_attacks = attacks;
            m_movements = movements;
            m_lastFrame = lastFrame;
        }
        public bool NextStep(int currentFrame,Rigidbody2D rb, Character_Base thisCharacterBase)
        {
            foreach(Attack attack in m_attacks)
            {
                if (attack.IsActive(currentFrame))
                {
                    Collider2D[] hitboxesDetected = attack.SearchAttack(rb.position);
                    foreach ( Collider2D hitboxConfirmed in hitboxesDetected)
                    {
                        if (hitboxConfirmed.gameObject.layer == 10)
                        {
                            Character_Base detectedCharacter = hitboxConfirmed.GetComponentInParent<Character_Base>();
                            if (detectedCharacter != thisCharacterBase)
                            {
                                detectedCharacter.TakeDamage(attack.GetDamage(), attack.GetHitStunt());
                            }
                        }
                    }
                }
            }
            foreach(Movement movement in m_movements)
            {
                if (movement.IsActive(currentFrame))
                {
                    movement.Move(rb);
                }
            }
            return currentFrame < m_lastFrame;
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        SetUpDictionary();
    }
    private void FixedUpdate()
    {
        if (currentHitStuntFrame >= 0)
        {
            print(currentHitStuntFrame);
            currentHitStuntFrame--;
        }
        // Logic that requires inputs
        inputsThisFrame = inputhandler.GetInputs();
        if (currentActionFrame >= 0)
        {
            ActionLoading();
            return;
        }
        CheckGround();
        if (isGrounded)
        {
            GroundOption();
        }
    }
    private void GroundOption()
    {   
        GroundMovementLogic();
        GroundAttackLogic();
    }
    private void CheckGround()
    {
        groundCast = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + PADDING, ground); // check all ground layer collider beneath player
        if (groundCast.collider != null) { isGrounded = true; } else { isGrounded = false; }
    }
    private void GroundMovementLogic()
    {
        // go left
        if (inputsThisFrame["Left"] > 0) // check for state 1.2.3 (dtected button press)
        {
            rb.velocity = new Vector2(-1 * SPEED, rb.velocity.y);
        }
        // go right
        else if (inputsThisFrame["Right"] > 0 )
        {
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // to stop character on release
        }
        // jump
        if (inputsThisFrame["Jump"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            action = ACTION_DICT["Jump"];
            currentActionFrame = 0;
            rb.velocity = new Vector2(0,0);
        }
    }
    private void GroundAttackLogic()
    {
        if (inputsThisFrame["Attack1"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            action = ACTION_DICT["Attack_Netural"];
            currentActionFrame = 0;
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void ActionLoading()
    {
        if (action != null)
        {
            if (action.NextStep(currentActionFrame,rb,this))
            {
                currentActionFrame++;
            }
            else
            {
                action = null;
                currentActionFrame = -1;
            }
        }
    }
    public void TakeDamage(float damage, int hitStunt)
    {
        action = null;
        currentActionFrame = -1;
        currentHitStuntFrame = hitStunt;
        hitPoints -= damage;
    }
    private void SetUpDictionary()
    {
        List<Movement> movement_list = new List<Movement>();
        movement_list.Add(new Movement(9, new Vector2(0, JUMP_VEL)));
        Action jump = new Action(new List<Attack>(), movement_list, 9);
        ACTION_DICT.Add("Jump", jump);
        List<Attack> attack_list = new List<Attack>();
        attack_list.Add(new Attack(9, 9, new Vector2(0, 0), new Vector2(10, 10), 0, 5, 9));
        Action attack = new Action(attack_list, new List<Movement>(), 9);
        ACTION_DICT.Add("Attack_Netural", attack);
    }
}