using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudInputHandler : InputHandler
{
    public List<string> inputButton = new List<string>();
    public string heldButton = "";
    public int inputType = 0;
    // what buttons will we read, not in use    

    private void Start()
    {
        foreach (string input in inputButton)
        {
            if(input == heldButton)
            {
                inputs.Add(input, inputType);
            }
            else
            {
                inputs.Add(input, 0);
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (string input in inputButton)
        {
            if (input == heldButton)
            {
                inputs[input] = inputType;
            }
            else
            {
                inputs[input] = 0;
            }
        }
    }
}