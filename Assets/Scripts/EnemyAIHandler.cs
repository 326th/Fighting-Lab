using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIHandler : InputHandler
{
    public List<string> inputButton = new List<string>();
    private Character_Base player;
    private Character_Base AI;
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject AIObject;
    [SerializeField] float playerDistance;

    //AI decision time
    private static int decisionTime = 5;
    private int currentDecisionTime = decisionTime;

    //Return random value between 0 to 1
    float randomFloat;

    //Back
    private bool backLastDecision = false;
    private float chanceToBackAgain = 0.5f;

    //Crouch
    private float chanceToCrouch = 0.4f;

    //Block
    private bool guardLastDecision = false;
    private float chanceToGuardAgain = 0.8f;


    void Awake()
    {
        player = playerObject.GetComponent<Character_Base>();
        AI = AIObject.GetComponent<Character_Base>();
    }

    void Start()
    {
        foreach (string input in inputButton)
        {
            inputs.Add(input, 0);
        }
    }

    void FixedUpdate()
    {
        randomFloat = Random.value;
        if (currentDecisionTime == -1)
        {
            currentDecisionTime = decisionTime;
        }
        UpdatePlayerDistance();
        foreach (string input in inputButton)
        {
            inputs[input] = 0;
        }

        
        //if (currentDecisionTime == decisionTime)
        //{
        //    inputs["Attack1"] = 1;
        //}
        if (playerDistance <= 2.5)
        {
            NearLogic();
        }
        else if (playerDistance <= 7)
        {
            MedLogic();
        }
        else
        {
            FarLogic();
        }
        currentDecisionTime--;
    }

    private void UpdatePlayerDistance()
    {
        float distance = player.rb.position.x - AI.rb.position.x;
        playerDistance = Mathf.Abs(distance);
    }

    private void NearLogic()
    {
        //if backLastDecision, have a chance to back again
        if (currentDecisionTime == decisionTime && randomFloat > chanceToBackAgain)
        {
            backLastDecision = false;
            //Set new random value
            randomFloat = Random.value;
        }
        if (backLastDecision == true)
        {
            Back();
            return;
        }

        //if guardLastDecision, have a chance to guard again
        if (currentDecisionTime == decisionTime && randomFloat > chanceToGuardAgain)
        {
            guardLastDecision = false;
            //Set new random value
            randomFloat = Random.value;
        }
        if (guardLastDecision == true)
        {
            inputs["Guard"] = 2;
            return;
        }

        //back
        if (randomFloat < 0.2f && currentDecisionTime == decisionTime && guardLastDecision == false)
        {
            Back();
            backLastDecision = true;
            return;
        }

        if (playerDistance >= 2.2)
        {
            Walk();
        }
        else if (playerDistance >= 1.5)
        {
            if (currentDecisionTime == decisionTime)
            {
                if (randomFloat < chanceToCrouch)
                {
                    inputs["Down"] = 2;
                    randomFloat = Random.value;
                }
                if (randomFloat < 0.2f)
                {
                    inputs["Guard"] = 2;
                    guardLastDecision = true;
                }
                if (randomFloat < 0.65f)
                {
                    inputs["Attack2"] = 3;
                }
                else
                {
                    //TODO: Forward Kick
                    inputs["Attack2"] = 3;
                }
            }
            else
            {
                Walk();
            }
        }
        else if (playerDistance >= 1.2)
        {
            if (currentDecisionTime == decisionTime)
            {
                //Crouch
                if (randomFloat < chanceToCrouch)
                {
                    inputs["Down"] = 2;
                    randomFloat = Random.value;
                }
                //Attack
                if (randomFloat < 0.25f)
                {
                    inputs["Attack2"] = 3;
                }
                else if (randomFloat < 0.5f)
                {
                    //TODO: Forward Kick
                    inputs["Attack2"] = 3;
                }
                else
                {
                    inputs["Attack1"] = 3;
                }
            }
        }
        else
        {
            if (currentDecisionTime == decisionTime)
            {
                //Crouch
                if (randomFloat < chanceToCrouch)
                {
                    inputs["Down"] = 2;
                    randomFloat = Random.value;
                }
                //Attack
                if (randomFloat < 0.1f)
                {
                    inputs["Attack2"] = 3;
                }
                else if (randomFloat < 0.2f)
                {
                    //TODO: Forward Kick
                    inputs["Attack2"] = 3;
                }
                else if (randomFloat < 0.6f)
                {
                    inputs["Attack1"] = 3;
                }
                else
                {
                    inputs["Grab"] = 3;
                }
            }
        }
    }

    private void MedLogic()
    {
        if (guardLastDecision == true)
        {
            guardLastDecision = false;
        }

        //if backLastDecision, have a chance to back again
        if (currentDecisionTime == decisionTime && randomFloat > chanceToBackAgain)
        {
            backLastDecision = false;
            //Set new random value
            randomFloat = Random.value;
        }
        if (backLastDecision == true)
        {
            Back();
            return;
        }

        if (currentDecisionTime == decisionTime)
            {
                //print(randomNumber);
                if (randomFloat < 0.35f)
                {
                    Jump();
                }
                else
                {
                    Walk();
                }
            }
            else
            {
                Walk();
            }
    }

    private void FarLogic()
    {
        if(backLastDecision == true)
        {
            backLastDecision = false;
        }
        Walk();
    }
    private void Walk()
    {
        if (AI.facingRightLastFrame)
        {
            inputs["Right"] = 3;
        }
        else
        {
            inputs["Left"] = 3;
        }
    }
    private void WalkBackward()
    {
        if (AI.facingRightLastFrame)
        {
            inputs["Left"] = 3;
        }
        else
        {
            inputs["Right"] = 3;
        }
    }
    private void Jump()
    {
        inputs["Jump"] = 3;
    }

    private void Back()
    {
        backLastDecision = true;
        WalkBackward();
    }
}
