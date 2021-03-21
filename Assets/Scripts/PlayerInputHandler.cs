using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler
{
    public List<string> input_button = new List<string>();
    // what buttons will we read, not in use    
    private Dictionary<string, bool[]> collect_input_button = new Dictionary<string, bool[]>();

    private void Start()

    {
      foreach (string input in input_button)
        {
            collect_input_button.Add(input, new bool[] { false,false,false }); //button, up, last frame
            inputs.Add(input, 0);
        }
    }
    void Update()
    {
        foreach (string input in input_button)
        {
            if (Input.GetButton(input))
            {
                collect_input_button[input][0] = true;
            }
            if (Input.GetButtonDown(input))
            {
                collect_input_button[input][1] = true;
            }
        }
    }

    public override Dictionary<string, int>  GetInputs()
    {
        CalculateInputs();
        return inputs;
    }
    private void CalculateInputs()
    {
        StateLogic();
        GetFrameHeld();
    }

    private void StateLogic()
    {
        foreach (string input in input_button)
        {
            // these logic are fromm kanugh map
            if (!collect_input_button[input][0] && !collect_input_button[input][1])
            {
                inputs[input] = 0;
            }
            if (collect_input_button[input][1] && !collect_input_button[input][2])
            {
                inputs[input] = 1;
            }
            if (collect_input_button[input][0] && !collect_input_button[input][2])
            {
                inputs[input] = 1;
            }
            if (collect_input_button[input][0] && !collect_input_button[input][1] && collect_input_button[input][2])
            {
                inputs[input] = 2;
            }
            if (collect_input_button[input][1] && collect_input_button[input][2])
            {
                inputs[input] = 3;
            }
        }
    }
    private void GetFrameHeld()
    {
        foreach (string input in input_button)
        {
            collect_input_button[input] = new bool[] { false, false, false };
            if (Input.GetButton(input))
            {
                collect_input_button[input][2] = true;
            }
        }
    }
}
