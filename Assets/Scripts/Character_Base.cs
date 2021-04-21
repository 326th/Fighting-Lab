﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : ClassScript
{
    [SerializeField] private float hitPoints = 100;
    //Input getter
    [SerializeField] private InputHandler inputhandler;
    [SerializeField] private Dictionary<string, int> inputsThisFrame = new Dictionary<string, int>();
    //PLayer Components
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    //Finite States Machine for animation
    private enum State { Idle, Walk, Jump, Go_Up, Go_Down, Attack_Neutral, Damaged } // all states
    private State state = State.Idle; // starting state
    private bool stateGotChanged = false; // to prevent multiple trigger
    //Inspector variable
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask hurtBox;
    [SerializeField] private float SPEED = 5f;
    //Game logic constant
    private float PADDING = 0.05f;
    //ground checker
    private RaycastHit2D groundCast;
    private bool isGrounded;
    // logic action variable
    private Action action = null;
    private int currentActionFrame = -1;
    // logic action constant
    private ActionLoader actionLoader;
    private Dictionary<string, Action> ACTION_DICT = new Dictionary<string, Action>();
    private Dictionary<Action, string> REVERSE_ACTION_DICT = new Dictionary<Action, string>();
    // hit stunt variable
    private int currentHitStuntFrame = -1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        actionLoader = GetComponent<ActionLoader>();
        ACTION_DICT = actionLoader.GetDictionary();
        REVERSE_ACTION_DICT = actionLoader.GetReverseDictionary();
    }
    private void FixedUpdate()
    {
        inputsThisFrame = inputhandler.GetInputs();
        if (currentActionFrame >= 0)
        {
            ActionLoading();
            return;
        }
        if (currentHitStuntFrame >= 0)
        {
            currentHitStuntFrame--;
        }
        else
        {
            CheckGround();
            if (isGrounded)
            {
                GroundOption();
            }
        }
        // Logic that requires inputs
        SetAnimation();
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
            action = ACTION_DICT["Attack_Neutral"];
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
    private void SetAnimation()
    {
        StateAnimationLogic();
        SetTrigger();
    }
    private void StateAnimationLogic()
    {
        if (currentHitStuntFrame >= 0)
        {
            stateGotChanged = ChangeAnimationState(State.Damaged);
            return;
        }
        if (action != null)
        {
            state = (State)Enum.Parse(typeof(State), REVERSE_ACTION_DICT[action]);
            stateGotChanged = true;
            return;
        }
        if (isGrounded)
        {
            if (rb.velocity.x == 0)
            {
                stateGotChanged = ChangeAnimationState(State.Idle);
            }
            else
            {
                stateGotChanged = ChangeAnimationState(State.Walk);
            }
        }
        else
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
    private bool ChangeAnimationState(State animationState)
    {
        if (state != animationState)
        {
            state = animationState;
            return true;
        }
        return false;
    }
    private void SetTrigger()
    {
        if (stateGotChanged)
        {
            anim.SetTrigger(state.ToString());
        }
    }
}