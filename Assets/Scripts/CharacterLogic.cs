using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    //Input getter
    public List<string> inputs = new List<string>();
    //Finite States Machine
    private enum State { Idle, Walk, Jump, Go_Up, Go_Down, Normal_Attack } // all states
    private List<State> AIR_STATE = new List<State> { State.Go_Up };
    private List<State> LANDING_STATE = new List<State> { State.Go_Down };
    private List<State> GROUND_STATE = new List<State> { State.Idle, State.Walk};
    
    private State state = State.Idle; // starting state
    //PLayer Components
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    //Inspector variable
    [SerializeField] private LayerMask ground;
    [SerializeField] private float SPEED = 5f;
    [SerializeField] private float JUMP_VEL = 10f;
    //Game logic
    private float PADDING = 0.05f;
    //ground checker
    private RaycastHit2D ground_cast;
    private bool is_grounded;
    [SerializeField] bool input_lock = false; //during busy state (hit lag or hit stunt), player cannot move.
    [SerializeField] bool state_lock = false; //for easier handling of state locking

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    void Update()
    {
        CheckGround();
        if (!input_lock) 
        {
            Movement();
            if (inputs.Contains("Fire1"))
            {
                AtaackLogic();
            }
        }
        if (!state_lock)
        {
            AnimationState();
        }
        anim.SetInteger("State", (int)state);
    }
    private void ReturnToIdle()
    {
        state = State.Idle;
        input_lock = false;
    }
    private void AtaackLogic()
    {
        state = State.Normal_Attack;
        //input_lock = true;
    }
    public void Damage(float damage, float hurt_force)
    {
        print(damage);
    }
    private void CheckGround()
    {
        ground_cast = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + PADDING, ground); // check all ground layer collider beneath player
        if (ground_cast.collider != null) { is_grounded = true; } else { is_grounded = false; }
    }
    private void AnimationState()
    {
        // air logic
        if (AIR_STATE.Contains(state)) { AirStateLogic(); }
        // landing logic
        if (LANDING_STATE.Contains(state)) { LandingStateLogic(); }
        // grounded logic
        if (GROUND_STATE.Contains(state)) { GroundStateLogic(); }
    }
    private void LandingStateLogic()
    {
        if (is_grounded)
        {
            ReturnToIdle();
        }
    }
    private void AirStateLogic()
    {
        if (rb.velocity.y < 0.1f)
         {
            state = State.Go_Down;
         }
    }
    private void GroundStateLogic()
    {
        if (Mathf.Abs(rb.velocity.x) > PADDING)
        {
            state = State.Walk;
        }
        else
        {
            state = State.Idle;
        }
    }
    private void Movement()
    {
        // go left
        if (inputs.Contains("left")) //using left right to make the character stop immediately, horizontal axis > 0.9 can be consider
        {
            rb.velocity = new Vector2(-1 * SPEED, rb.velocity.y);
        }
        // go right
        else if (inputs.Contains("right"))
        {
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // to stop character on release
        }
        // jump
        if (inputs.Contains("Jump"))
        {
            if (is_grounded) {
                state = State.Jump;
                input_lock = true;
                state_lock = true;
                rb.velocity = new Vector2(0, 0);
            }
        }
    }
    private void Jump()
    {
        if (inputs.Contains("right"))
        {
            rb.velocity = new Vector2(SPEED,JUMP_VEL);
        }
        else if (inputs.Contains("left"))
        {
            rb.velocity = new Vector2(-1*SPEED, JUMP_VEL);
        }
        else
        {
            rb.velocity = new Vector2(0, JUMP_VEL);
        }
        state = State.Go_Up;
        state_lock = false;
    }

}
