using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    protected Dictionary<string, int> inputs = new Dictionary<string, int>();
    // button state for each input, this get update every frame
    // 0 not button press from last frame to now
    // 1 newly press but not held to the end
    // 2 held from last frame to this frame
    // 3 held from last frame button was repress
    public virtual Dictionary<string, int> GetInputs()
    {
        return inputs;
    }
}
