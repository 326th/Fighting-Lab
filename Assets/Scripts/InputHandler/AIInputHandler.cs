using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputHandler : InputHandler
{
    public List<string> inputButton = new List<string>();
    private Character_Base player;
    private Character_Base AI;
    //player
    [SerializeField] GameObject playerObject;

    [SerializeField] private float playerDistance;
    [SerializeField] GameObject AIObject;

    [SerializeField] private FacingController facing;

    private void Awake()
    {
        player = playerObject.GetComponent<Character_Base>();
        AI = AIObject.GetComponent<Character_Base>();
    }
    private void UpdatePlayerDistance()
    {
        float distance = player.rb.position.x - AI.rb.position.x;
        playerDistance = Mathf.Abs(distance);
    }

    private void Start()
    {
        foreach (string input in inputButton)
        {
            inputs.Add(input, 0);
        }
    }
    private void FixedUpdate()
    {
        UpdatePlayerDistance();
        foreach (string input in inputButton)
        {
            inputs[input] = 0;
        }
        if(playerDistance< 10)
        {
            inputs["right"] = 3;
        }
    }
}