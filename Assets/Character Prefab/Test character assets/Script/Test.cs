using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
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
    [SerializeField] private bool busy = false; //during busy state (hit lag or hit stunt), player cannot move. this state is freed by animator
    //Game logic
    private float PADDING = 1f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!busy) 
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("State", (int)state);
        print(anim.GetInteger("State"));
    }

    public void Damage(float damage, float hurt_force)
    {
        print(damage);
    }

    private void AnimationState()
    {
        if (state == State.Jump)
        {
            return;
        }
        if (state == State.Go_Up)
        {
            if(rb.velocity.y < 0.1f)
            {
                state = State.Go_Down;
            }
        }
        if (state == State.Go_Down)
        {
            if (rb.velocity.y == 0f)
            {
                state = State.Idle;
                busy = false;
            }
        }
    }
    private void Movement()
    {
        // go left
        if (Input.GetKey("left")) //using left right to make the character stop immediately, horizontal axis > 0.9 can be consider
        {
            rb.velocity = new Vector2(-1 * SPEED, rb.velocity.y);
        }
        // go right
        else if (Input.GetKey("right"))
        {
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // to stop character on release
        }
        // jump
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + PADDING, ground); // check if there is ground beneath
            if (hit.collider != null) {
                state = State.Jump;
                busy = true;
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKey("right"))
        {
            rb.velocity = new Vector2(SPEED,JUMP_VEL);
        }
        else if (Input.GetKey("left"))
        {
            rb.velocity = new Vector2(-1*SPEED, JUMP_VEL);
        }
        else
        {
            rb.velocity = new Vector2(0, JUMP_VEL);
        }
        state = State.Go_Up;
    }

}
