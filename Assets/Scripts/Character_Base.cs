using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : ClassScript
{
    [SerializeField] public float hitPoints = 100;

    //Facing Handler
    [SerializeField] private FacingController facingController;
    public bool facingRightLastFrame = false;

    //Input getter
    [SerializeField] private InputHandler inputHandler;
    private Dictionary<string, int> inputsThisFrame = new Dictionary<string, int>();

    //PLayer Components
    private Animator anim;
    public Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

    //Finite States Machine for animation
    private enum State { Idle, Walk, Jump, Go_Up, Go_Down, Damaged, Attack_Light, Air_Attack_Light, Attack_Heavy, Attack_Forward, Air_Attack_Heavy, Knocked, Recovery, Grab, Grabbed, Guard, Crouch, Crouch_Attack_Light, Crouch_Attack_Heavy, Crouch_Guard } // all states
    [SerializeField] private State state = State.Idle; // starting state
    [SerializeField] private bool stateGotChanged = false; // to prevent multiple trigger

    //Inspector variable
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask hurtBox;
    [SerializeField] public float SPEED = 5f;

    //Game logic constant
    private float PADDING = 0.05f;

    //ground checker
    private RaycastHit2D groundCast;
    private bool isGrounded;
    private bool crouch;

    // logic action variable
    private Action action = null;
    private int currentActionFrame = -1;

    // logic action constant
    private ActionLoader actionLoader;
    private Dictionary<string, Action> actionDict = new Dictionary<string, Action>();
    private Dictionary<Action, string> reverseActionDict = new Dictionary<Action, string>();

    // hit stunt variable
    [SerializeField] private int currentHitStuntFrame = -1;
    [SerializeField] private int hitStuntState = -1;
    [SerializeField] public bool connected = false;

    // healthbar
    [SerializeField] private HealthbarController healthbarController;

    // Attacker
    private Character_Base attacker;

    // Invincibility Time
    private int iFrame = -1;

    //Check if light attack hit(0=not hit 1= hit)
    [SerializeField] public int comboTime = -1;

    // Sprite Material Color
    Color color;

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = new Color(1, 0, 0, 0.5f);
    //    Gizmos.DrawCube(new Vector2(0.75f, -1.25f) + rb.position, new Vector2(1.25f, 1.5f));
    //}

    //Stats
    public float attackCount;
    public float hitCount;
    public float comboCount;
    public float lightAttackCount;
    public float heavyAttackCount;
    public float attackForwardCount;
    public float airLightAttackCount;
    public float airHeavyAttackCount;
    public float crouchLightAttackCount;
    public float crouchHeavyAttackCount;
    public float jumpCount;
    public float grabCount;
    public float guardCount;
    public float crouchGuardCount;
    public float moveForwardCount;
    public float moveBackwardCount;
    public float attackedOnAirCount;
    public float grabbedOnGuardCount;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        actionLoader = GetComponent<ActionLoader>();
        sr = GetComponent<SpriteRenderer>();
        color = sr.material.color;
        actionDict = actionLoader.GetDictionary();
        reverseActionDict = actionLoader.GetReverseDictionary();
        facingController = GameObject.Find("[Facing Controller]").GetComponent<FacingController>();
        facingRightLastFrame = facingController.IsFacingRight(gameObject);
        if (!facingRightLastFrame)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX ^= true;
        }
    }
    private void FixedUpdate()
    {
        //Facing
        var facingRight = facingController.IsFacingRight(gameObject);
        if (facingRightLastFrame != facingRight)
        {
            facingRightLastFrame = facingRight;
            gameObject.GetComponent<SpriteRenderer>().flipX ^= true;
        }

        //Get Input
        inputsThisFrame = inputHandler.GetInputs();

        if (comboTime > -1)
        {
            //print("Combotime" + comboTime);
            if (!crouch)
            {
                if (inputsThisFrame["Attack2"] % 2 == 1) // check for state 1 and 3 (newly pressed)
                {

                    comboTime = -1;
                    connected = false;
                    HeavyAttack();
                    comboCount++;
                    attackCount++;

                    SetAnimation();
                }
            }
            else
            {
                if (inputsThisFrame["Attack2"] % 2 == 1) // check for state 1 and 3 (newly pressed)
                {

                    comboTime = -1;
                    connected = false;
                    action = actionDict["Crouch_Attack_Heavy"];
                    currentActionFrame = 0;
                    rb.velocity = new Vector2(0, 0);
                    comboCount++;
                    attackCount++;

                    SetAnimation();
                }
            }
            
        }

        if (currentActionFrame >= 0)
        {
            ActionLoading();
            return;
        }

        //Decrease iFrame overtime
        if (iFrame > -1)
        {
            sr.material.color = new Color(0.2f, 0.3f, 0.8f);
            iFrame--;
        }
        else
        {
            sr.material.color = color;
        }

        //Decrease comboTime overtime
        if (comboTime > -1)
        {
            comboTime--;
        }

        if (currentHitStuntFrame >= 0)
        {
            currentHitStuntFrame--;
            //Damaged
            if (currentHitStuntFrame == 0 && hitStuntState == 0)
            {
                hitStuntState = -1;
            }
            //Recovery
            if (currentHitStuntFrame == 0 && hitStuntState == 1)
            {
                action = actionDict["Recovery"];
                currentActionFrame = 0;
                hitStuntState = -1;
                rb.velocity = new Vector2(0, 0);
            }
            //If be grabbed, then knock
            if (currentHitStuntFrame == 0 && hitStuntState == 2)
            {
                //Hardcode currentHitStuntFrame equal to get kicked
                currentHitStuntFrame = 20;
                hitStuntState = 1;
            }
        }
        else //When isn't revovering, give ground/air option
        {
            CheckGround();
            if (isGrounded)
            {
                GroundOption();
            }
            else
            {
                AirOption();
            }
        }

        SetAnimation();
    }

    //Make sure that player doesn't stuck in the air
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isGrounded)
            {
                rb.velocity = new Vector2(0, 0);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Character_Base enemy = collision.gameObject.GetComponentInParent<Character_Base>();
            if (!isGrounded)
            {
                if (facingRightLastFrame)
                {
                    rb.AddForce(new Vector2(-0.025f, 0));
                }
                else
                {
                    rb.AddForce(new Vector2(0.025f, 0));
                }
            }
            else
            {
                if (!enemy.isGrounded)
                {
                    if (facingRightLastFrame)
                    {
                        rb.AddForce(new Vector2(-0.025f, 0));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(0.025f, 0));
                    }
                }
            }
        }
    }

    //Ground/Air Option
    private void GroundOption()
    {
        if (currentHitStuntFrame == -1)
        {
            GroundMovementLogic();
            GroundAttackLogic();
        }


    }
    private void AirOption()
    {
        AirAttackLogic();

    }

    //Check if player on ground
    private void CheckGround()
    {
        groundCast = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + PADDING, ground); // check all ground layer collider beneath player
        if (groundCast.collider != null) { isGrounded = true; } else { isGrounded = false; }
    }

    //Ground Movement Logic
    private void GroundMovementLogic()
    {
        if(inputsThisFrame["Down"] > 0)
        {
            crouch = true;
            return;
        }
        else
        {
            crouch = false;
        }
        // Movement: Left
        if (inputsThisFrame["Left"] > 0) // check for state 1,2,3 (detected button press)
        {
            rb.velocity = new Vector2(-1 * SPEED, rb.velocity.y);
            if (facingRightLastFrame)
            {
                moveBackwardCount++;
            }
            else
            {
                moveForwardCount++;
            }
        }
        // Movement: Right
        else if (inputsThisFrame["Right"] > 0)
        {
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
            if (facingRightLastFrame)
            {
                moveForwardCount++;
            }
            else
            {
                moveBackwardCount++;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // to stop character on release
        }
        // Movement: Jump
        if (inputsThisFrame["Jump"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            action = actionDict["Jump"];
            currentActionFrame = 0;
            rb.velocity = new Vector2(0, 0);
            jumpCount++;
        }
    }

    //Ground Attack Logic
    private void GroundAttackLogic()
    {
        if (!crouch)
        {
            //Light attack
            if (inputsThisFrame["Attack1"] % 2 == 1) // check for state 1 and 3 (newly pressed)
            {
                if (facingRightLastFrame)
                {
                    action = actionDict["Attack_Light"];
                    currentActionFrame = 0;
                    rb.velocity = new Vector2(0, 0);
                }
                else
                {
                    action = actionDict["Attack_Light"];
                    currentActionFrame = 0;
                    rb.velocity = new Vector2(0, 0);
                }
                attackCount++;
                lightAttackCount++;
            }

            if (inputsThisFrame["Attack2"] % 2 == 1) // check for state 1 and 3 (newly pressed)
            {
                HeavyAttack();
                attackCount++;
            }

            //Guard
            if (inputsThisFrame["Guard"] == 1 || inputsThisFrame["Guard"] == 2 || inputsThisFrame["Guard"] == 3) // check for state 1 2 and 3
            {
                action = actionDict["Guard"];
                //stateGotChanged = ChangeAnimationState(State.Guard);
                currentActionFrame = 0;
                rb.velocity = new Vector2(0, 0);
                guardCount++;
            }
        }
        else
        {
            //Crouch Light attack
            if (inputsThisFrame["Attack1"] % 2 == 1) // check for state 1 and 3 (newly pressed)
            {
                if (facingRightLastFrame)
                {
                    action = actionDict["Crouch_Attack_Light"];
                    currentActionFrame = 0;
                    rb.velocity = new Vector2(0, 0);
                }
                else
                {
                    action = actionDict["Crouch_Attack_Light"];
                    currentActionFrame = 0;
                    rb.velocity = new Vector2(0, 0);
                }
                attackCount++;
                crouchLightAttackCount++;
            }

            //Crouch Heavy attack
            if (inputsThisFrame["Attack2"] % 2 == 1) // check for state 1 and 3 (newly pressed)
            {
                action = actionDict["Crouch_Attack_Heavy"];
                currentActionFrame = 0;
                rb.velocity = new Vector2(0, 0);
                attackCount++;
                crouchHeavyAttackCount++;
            }

            //Crouch Guard
            if (inputsThisFrame["Guard"] == 1 || inputsThisFrame["Guard"] == 2 || inputsThisFrame["Guard"] == 3) // check for state 1 2 and 3
            {
                action = actionDict["Crouch_Guard"];
                currentActionFrame = 0;
                rb.velocity = new Vector2(0, 0);
                crouchGuardCount++;
            }
        }
        

        //Grab
        if (inputsThisFrame["Grab"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            action = actionDict["Grab"];
            currentActionFrame = 0;
            rb.velocity = new Vector2(0, 0);
            attackCount++;
            grabCount++;
        }
    }

    //Air Attack Logic
    private void AirAttackLogic()
    {
        //Light attack
        if (inputsThisFrame["Attack1"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            if (facingRightLastFrame)
            {
                action = actionDict["Air_Attack_Light"];
                currentActionFrame = 0;
            }
            else
            {
                action = actionDict["Air_Attack_Light"];
                currentActionFrame = 0;
            }
            attackCount++;
            airLightAttackCount++;
        }

        //Heavy Attack
        if (inputsThisFrame["Attack2"] % 2 == 1) // check for state 1 and 3 (newly pressed)
        {
            if (facingRightLastFrame)
            {
                action = actionDict["Air_Attack_Heavy"];
                currentActionFrame = 0;
            }
            else
            {
                action = actionDict["Air_Attack_Heavy"];
                currentActionFrame = 0;
            }
            attackCount++;
            airHeavyAttackCount++;
        }
    }
    private void HeavyAttack()
    {
        // Attack Forward/Heavy Attack
        if (facingRightLastFrame)
        {
            if (inputsThisFrame["Right"] != 0)
            {
                action = actionDict["Attack_Forward"];
                currentActionFrame = 0;
                rb.velocity = new Vector2(0, 0);
                attackForwardCount++;
            }
            else
            {
                action = actionDict["Attack_Heavy"];
                currentActionFrame = 0;
                rb.velocity = new Vector2(0, 0);
                heavyAttackCount++;
            }
        }
        else
        {
            if (inputsThisFrame["Left"] != 0)
            {
                action = actionDict["Attack_Forward"];
                currentActionFrame = 0;
                rb.velocity = new Vector2(0, 0);
                attackForwardCount++;
            }
            else
            {
                action = actionDict["Attack_Heavy"];
                currentActionFrame = 0;
                rb.velocity = new Vector2(0, 0);
                heavyAttackCount++;
            }
        }
    }

    //Load Action
    private void ActionLoading()
    {
        if (action != null)
        {
            //if currentFrame < m_lastFrame
            if (action.NextStep(currentActionFrame, rb, this, facingRightLastFrame, inputsThisFrame))
            {
                currentActionFrame++;
            }
            else
            {
                action = action.GetNextAction(this);
                if (action == null)
                {
                    currentActionFrame = -1;
                }
                else { currentActionFrame = 0; }
            }
        }
    }

    //Take Damage
    public void TakeDamage(float damage, int hitStunt, int special)
    {
        if (iFrame == -1) //Not in iFrame
        {
            if (state != State.Guard || special == 2) //Unguarded or being grabbed
            {
                action = null;
                currentActionFrame = -1;
                currentHitStuntFrame = hitStunt;
                hitPoints -= damage;
                healthbarController.SetHealth(hitPoints);
                hitStuntState = special;
                if (!isGrounded)
                {
                    attackedOnAirCount++;
                }
            }
            else
            {
                action = null;
                currentActionFrame = -1;
                //currentHitStuntFrame = hitStunt;
                hitPoints -= damage / 5;
                healthbarController.SetHealth(hitPoints);
                hitStuntState = special;
            }
            if (state == State.Guard && special == 2)
            {
                grabbedOnGuardCount++;
            }
        }
    }

    //Set attacker
    public void SetAttacker(Character_Base enemy)
    {
        attacker = enemy;
    }

    //Set Animation
    private void SetAnimation()
    {
        StateAnimationLogic();
        SetTrigger();
    }



    private void StateAnimationLogic()
    {
        //Change state depends on attack
        if (currentHitStuntFrame >= 0 && iFrame == -1)
        {
            if (hitStuntState == 0)
            {
                stateGotChanged = ChangeAnimationState(State.Damaged);
                return;
            }
            else if (hitStuntState == 1)
            {
                stateGotChanged = ChangeAnimationState(State.Knocked);
                rb.velocity = new Vector2(0, 0);
                iFrame = 10;
                return;
            }
            else if (hitStuntState == 2)
            {
                stateGotChanged = ChangeAnimationState(State.Grabbed);
                return;
            }

        }
        if (action != null)
        {
            state = (State)Enum.Parse(typeof(State), reverseActionDict[action]);
            stateGotChanged = true;
            return;
        }
        if (isGrounded)
        {
            if (hitStuntState == -1)
            {
                if (crouch)
                {
                    stateGotChanged = ChangeAnimationState(State.Crouch);
                    return;
                }
                if (rb.velocity.x == 0)
                {
                    stateGotChanged = ChangeAnimationState(State.Idle);
                }
                else
                {
                    stateGotChanged = ChangeAnimationState(State.Walk);
                }
            }

        }
        else
        {
            if (hitStuntState == -1)
            {
                if (rb.velocity.y >= 0)
                {
                    stateGotChanged = ChangeAnimationState(State.Go_Up);
                }
                else
                {
                    stateGotChanged = ChangeAnimationState(State.Go_Down);
                }
            }
        }

    }







    private bool ChangeAnimationState(State animationState)
    {
        if (state != animationState)
        {
            //print("change state to -> " + animationState);
            state = animationState;
            return true;
        }
        return false;
    }

    //Change Animator State
    private void SetTrigger()
    {
        if (stateGotChanged)
        {
            //print("setTrigger: " + state);
            anim.SetTrigger(state.ToString());
            stateGotChanged = false;
        }
    }

    public void SavePlayer(float playTime)
    {
        SaveSystem.SavePlayer(this, playTime);
    }
}