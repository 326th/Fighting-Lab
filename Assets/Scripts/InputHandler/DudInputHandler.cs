using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudInputHandler : InputHandler
{
    public List<string> inputButton = new List<string>();
    // what buttons will we read, not in use    

    private void Start()
    {
        foreach (string input in inputButton)
        {
            inputs.Add(input, 0);
        }
    }
}