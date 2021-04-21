using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterActionLoader : ActionLoader
{
    public override Dictionary<string, Action> GetDictionary()
    {
        List<Movement> movement_list = new List<Movement>();
        movement_list.Add(new Movement(9, new Vector2(0, JUMP_VEL)));
        Action jump = new Action(new List<Attack>(), movement_list, 9);
        ACTION_DICT.Add("Jump", jump);
        List<Attack> attack_list = new List<Attack>();
        attack_list.Add(new Attack(9, 9, new Vector2(0, 0), new Vector2(10, 10), 0, 8, 9, "HurtBox"));
        Action attack = new Action(attack_list, new List<Movement>(), 12);
        ACTION_DICT.Add("Attack_Neutral", attack);
        return ACTION_DICT;
    }
}
