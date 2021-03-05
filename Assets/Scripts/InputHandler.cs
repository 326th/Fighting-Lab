using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    List<string> input_keys = new List<string>();
    public List<string> collect_inputKey  = new List<string>();
    [SerializeField] List<CharacterLogic> player_control = new List<CharacterLogic>();

    void Update()
    {
        input_keys.Clear();
        foreach (string input in collect_inputKey)
        {
            if (Input.GetKey(input))
            {
                input_keys.Add(input);
            }
        }
        foreach (CharacterLogic player in player_control)
        {
            player.input_keys = input_keys;
        }
    }

}
