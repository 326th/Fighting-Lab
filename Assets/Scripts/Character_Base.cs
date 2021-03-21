using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    //Input getter
    [SerializeField] private InputHandler inputhandler;
    [SerializeField] private Dictionary<string, int>  inputs = new Dictionary<string, int>();
    //Finite States Machine
    private enum State { Idle, Walk, Jump, Go_Up, Go_Down, Normal_Attack } // all states
    private State state = State.Idle; // starting state
    //PLayer Components
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    //Inspector variable
    [SerializeField] private LayerMask ground;
    [SerializeField] private float SPEED = 5f;
    [SerializeField] private float JUMP_VEL = 10f;
    //Game logic constant
    private float PADDING = 0.05f;
    //ground checker
    private RaycastHit2D ground_cast;
    private bool is_grounded;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
        inputs = inputhandler.GetInputs();
        CheckGround();
        GroundOption();
    }
    private void GroundOption()
    {   
        Movement();
    }
    private void CheckGround()
    {
        ground_cast = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + PADDING, ground); // check all ground layer collider beneath player
        if (ground_cast.collider != null) { is_grounded = true; } else { is_grounded = false; }
    }
    private void Movement()
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
            if (is_grounded)
            {
                state = State.Jump;
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

}
