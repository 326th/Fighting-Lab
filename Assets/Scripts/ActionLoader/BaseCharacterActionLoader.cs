using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterActionLoader : ActionLoader
{
    public override Dictionary<string, Action> GetDictionary()
    {
        JumpBufferHandle jumpBufferHandle = new JumpBufferHandle();
        //print(JsonUtility.ToJson(jumpBufferHandle));
        InputBuffer buffer = new InputBuffer(5,9, jumpBufferHandle);
        Action jump = new Action(new List<Attack>(), new List<Movement>(), buffer, 9);
        actionDict.Add("Jump", jump);
        Dictionary<string, Action> jumpDict = jumpBufferHandle.GetActionDict();
        List<Attack> attack_list = new List<Attack>();
        attack_list.Add(new Attack(7, 12, new Vector2(0.75f, 0.75f), new Vector2(1.25f, 0.5f), 0, 8, 9, "HurtBox", 0));
        Action attack = new Action(attack_list, new List<Movement>(), new InputBuffer(-1,-1,new BufferHandle()), 15);
        actionDict.Add("Attack_Neutral", attack);
        /// ต้อง empty list ทุกรอบ!!!
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(15, 19, new Vector2(0.825f, 0f), new Vector2(1.75f, 0.7f), 0, 16, 20, "HurtBox", 1));
        Action attackForward = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 23);
        actionDict.Add("Attack_Forward", attackForward);
        Action recovery = new Action(new List<Attack>(), new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()),20);
        actionDict.Add("Recovery", recovery);
        return actionDict;
    }
}
