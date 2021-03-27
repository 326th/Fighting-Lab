using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler
{
    public List<string> inputButton = new List<string>();
    // what buttons will we read, not in use    
    private Dictionary<string, bool[]> collectInputButton = new Dictionary<string, bool[]>();

    private void Start()
    {
      foreach (string input in inputButton)
        {
            collectInputButton.Add(input, new bool[] { false,false,false }); //button, up, last frame
            inputs.Add(input, 0);
        }
    }
    void Update()
    {
        foreach (string input in inputButton)
        {
            if (Input.GetButton(input))
            {
                collectInputButton[input][0] = true;
            }
            if (Input.GetButtonDown(input))
            {
                collectInputButton[input][1] = true;
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
        foreach (string input in inputButton)
        {
            // these logic are fromm kanugh map
            if (!collectInputButton[input][0] && !collectInputButton[input][1])
            {
                inputs[input] = 0;
            }
            if (collectInputButton[input][1] && !collectInputButton[input][2])
            {
                inputs[input] = 1;
            }
            if (collectInputButton[input][0] && !collectInputButton[input][2])
            {
                inputs[input] = 1;
            }
            if (collectInputButton[input][0] && !collectInputButton[input][1] && collectInputButton[input][2])
            {
                inputs[input] = 2;
            }
            if (collectInputButton[input][1] && collectInputButton[input][2])
            {
                inputs[input] = 3;
            }
        }
    }
    private void GetFrameHeld()
    {
        foreach (string input in inputButton)
        {
            collectInputButton[input] = new bool[] { false, false, false };
            if (Input.GetButton(input))
            {
                collectInputButton[input][2] = true;
            }
        }
    }
}
