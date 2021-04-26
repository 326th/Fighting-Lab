﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterActionLoader : ActionLoader
{
    public override Dictionary<string, Action> GetDictionary()
    {
        JumpBufferHandle jumpBufferHandle = new JumpBufferHandle();
        InputBuffer buffer = new InputBuffer(5,9, jumpBufferHandle);
        Action jump = new Action(new List<Attack>(), new List<Movement>(), buffer, 9);
        actionDict.Add("Jump", jump);
        Dictionary<string, Action> jumpDict = jumpBufferHandle.GetActionDict();
        foreach (string key in jumpDict.Keys)
        {
            //actionDict.Add(key, jumpDict[key]);
        }
        List<Attack> attack_list = new List<Attack>();
        attack_list.Add(new Attack(7, 12, new Vector2(0.75f, 0.75f), new Vector2(1.25f, 0.5f), 0, 8, 9, "HurtBox"));
        Action attack = new Action(attack_list, new List<Movement>(), new InputBuffer(-1,-1,new BufferHandle()), 15);
        actionDict.Add("Attack_Neutral", attack);
        attack_list.Add(new Attack(15, 19, new Vector2(0.825f, 0f), new Vector2(1.75f, 0.7f), 0, 8, 9, "HurtBox"));
        Action attackForward = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 23);
        actionDict.Add("Attack_Forward", attackForward);
        return actionDict;
    }
}
