﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //Finite States Machine
    private enum State { Idle, Walk, Jump, Go_Up, Go_Down, Normal_Attack } // all states
    private List<State> GroundState = new List<State> { State.Idle, State.Walk, State.Go_Down };
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
    private float PADDING = 1f;
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
        }
        if (!state_lock)
        {
            AnimationState();
        }
        anim.SetInteger("State", (int)state);
        print(anim.GetInteger("State").ToString()  +  is_grounded.ToString() + rb.velocity.y.ToString());
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
        if (!GroundState.Contains(state)) { AirStateLogic(); }
        //grounded logic
        else { GroundStateLogic(); }
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
        if (is_grounded)
        {
            if(state == State.Go_Down)
            {
                state = State.Idle;
                input_lock = false;
            }
            if (Mathf.Abs(rb.velocity.x) > PADDING)
            {
                state = State.Walk;
            }
            else
            {
                state = State.Idle;
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
        state_lock = false;
    }

}
