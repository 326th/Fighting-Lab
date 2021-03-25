using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudInputHandler : InputHandler
{
    public List<string> input_button = new List<string>();
    // what buttons will we read, not in use    
    private Dictionary<string, bool[]> collect_input_button = new Dictionary<string, bool[]>();

    private void Start()
    {
        foreach (string input in input_button)
        {
            collect_input_button.Add(input, new bool[] { false, false, false }); //button, up, last frame
            inputs.Add(input, 0);
        }
    }
}