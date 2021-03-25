using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    //Input getter
    [SerializeField] private InputHandler inputhandler;
    [SerializeField] private Dictionary<string, int> inputs = new Dictionary<string, int>();
    //Finite States Machine
    private enum State { Idle, Walk, Jump, Go_Up, Go_Down, Normal_Attack } // all states
    private State state = State.Idle; // starting state
    //PLayer Components
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    //Inspector variable
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask hurt_box;
    [SerializeField] private float SPEED = 5f;
    [SerializeField] private float JUMP_VEL = 10f;
    //Game logic constant
    private float PADDING = 0.05f;
    //ground checker
    private RaycastHit2D ground_cast;
    private bool is_grounded;
    // curent action
    private Action action = null;
    private int current_frame = -1;
    private Dictionary<string, Action> action_dict = new Dictionary<string, Action>();

    // following classes are for action logic
    private class Attack
    {
        private int starting_frame;
        private int ending_frame;
        private Vector2 point;
        private Vector2 size;
        private float angle;
        public Attack(int starting_frame, int ending_frame, Vector2 point, Vector2 size, float angle)
        {
            this.starting_frame = starting_frame;
            this.ending_frame = ending_frame;
            this.point = point;
            this.size = size;
            this.angle = angle;
        }
        public Collider2D[] SearchAttack(Vector2 position)
        {
            return Physics2D.OverlapBoxAll(point: point + position, size: size, angle: angle);
        }
        public bool IsActive(int current_frame)
        {
            return starting_frame <= current_frame && current_frame <= ending_frame;
        }
    }
    private class Movement
    {
        private int starting_frame;
        private Vector2 force;
        public Movement(int starting_frame, Vector2 force)
        {
            this.starting_frame = starting_frame;
            this.force = force;
        }
        public bool IsActive(int current_frame)
        {
            return starting_frame == current_frame;
        }
        public void Move(Rigidbody2D rb)
        {
            rb.velocity = force;
        }
    }
    private class Action
    {
        private List<Attack> attack_list;
        private List<Movement> movement_list;
        private int last_frame;
        public Action (List<Attack> attack_list, List<Movement> movement_list,int last_frame)
        {
            this.attack_list = attack_list;
            this.movement_list = movement_list;
            this.last_frame = last_frame;
        }
        public bool NextStep(int current_frame,Rigidbody2D rb)
        {
            foreach(Attack attack in attack_list)
            {
                if (attack.IsActive(current_frame))
                {
                    var hitboxes_detected = attack.SearchAttack(rb.position);
                    foreach ( var hitbox_confirmed in hitboxes_detected)
                    {
                        if (hitbox_confirmed.gameObject.layer == 10)
                        {
                            print(hitbox_confirmed + "  " + hitbox_confirmed.gameObject.layer.ToString());
                        }
                    }
                }
            }
            foreach(Movement movement in movement_list)
            {
                if (movement.IsActive(current_frame))
                {
                    movement.Move(rb);
                }
            }
            return current_frame < last_frame;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        List<Movement> movement_list = new List<Movement>();
        movement_list.Add(new Movement(9, new Vector2(0,JUMP_VEL)));
        Action jump = new Action(new List<Attack>(), movement_list, 9);
        action_dict.Add("Jump", jump );
        List<Attack> attack_list = new List<Attack>();
        attack_list.Add(new Attack(9,9, new Vector2(0, 0), new Vector2(10, 10), 0 ));
        Action attack = new Action(attack_list, new List<Movement>(), 9);
        action_dict.Add("Attack_Netural", attack);
    }
    private void FixedUpdate()
    {
        //print(current_frame);
        if (current_frame >= 0)
        {
            Action_Loading();
            return;
        }
        inputs = inputhandler.GetInputs();
        CheckGround();
        if (is_grounded)
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
        ground_cast = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + PADDING, ground); // check all ground layer collider beneath player
        if (ground_cast.collider != null) { is_grounded = true; } else { is_grounded = false; }
    }
    private void GroundMovementLogic()
    {
        // go left
        if (inputs["Left"] > 0) // check for state 1.2.3 (dtected button press)
        {
            rb.velocity = new Vector2(-1 * SPEED, rb.velocity.y);
        }
        // go right
        else if (inputs["Right"] > 0 )
        {
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // to stop character on release
        }
        // jump
        if (inputs["Jump"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            action = action_dict["Jump"];
            current_frame = 0;
            rb.velocity = new Vector2(0,0);
        }
    }
    private void GroundAttackLogic()
    {
        if (inputs["Attack1"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            action = action_dict["Attack_Netural"];
            current_frame = 0;
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void Action_Loading()
    {
        if (action != null)
        {
            if (action.NextStep(current_frame,rb))
            {
                current_frame++;
            }
            else
            {
                action = null;
                current_frame = -1;
            }
        }
    }
}