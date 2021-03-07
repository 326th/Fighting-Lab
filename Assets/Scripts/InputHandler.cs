using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    List<string> inputs = new List<string>();
    public List<string> collect_input_key  = new List<string>();
    public List<string> collect_input_button = new List<string>();
    [SerializeField] List<CharacterLogic> player_control = new List<CharacterLogic>();

    void Update()
    {
        inputs.Clear();
        foreach (string input in collect_input_key)
        {
            if (Input.GetKey(input))
            {
                inputs.Add(input);
            }
        }
        foreach (string input in collect_input_button)
        {
            if (Input.GetButtonDown(input))
            {
                inputs.Add(input);
            }
        }
        foreach (CharacterLogic player in player_control)
        {
            player.inputs = inputs;
        }
    }

}
