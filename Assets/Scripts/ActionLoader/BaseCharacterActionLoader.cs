using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterActionLoader : ActionLoader
{
    public override Dictionary<string, Action> GetDictionary()
    {
        //Action: Jump
        JumpBufferHandle jumpBufferHandle = new JumpBufferHandle();
        //print(JsonUtility.ToJson(jumpBufferHandle));
        InputBuffer buffer = new InputBuffer(5,9, jumpBufferHandle);
        Action jump = new Action(new List<Attack>(), new List<Movement>(), buffer, 9);
        actionDict.Add("Jump", jump);
        Dictionary<string, Action> jumpDict = jumpBufferHandle.GetActionDict();

        //Action: Light Attack
        List<Attack> attack_list = new List<Attack>();
        attack_list.Add(new Attack(7, 12, new Vector2(0.75f, 0.75f), new Vector2(1.25f, 0.5f), 0, 8, 9, "HurtBox", 0));
        Action attack = new Action(attack_list, new List<Movement>(), new InputBuffer(-1,-1,new BufferHandle()), 15);
        actionDict.Add("Attack_Light", attack);
        /// ต้อง empty list ทุกรอบ!!!

        //Action: Heavy Attack
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(15, 19, new Vector2(0.825f, 0f), new Vector2(1.75f, 0.7f), 0, 16, 20, "HurtBox", 1));
        Action attackHeavy = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 20);
        actionDict.Add("Attack_Heavy", attackHeavy);

        //Action: Attack Forward
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(15, 19, new Vector2(0.825f, 0f), new Vector2(1.75f, 0.7f), 0, 16, 20, "HurtBox", 1));
        Action attackForward = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 20);
        actionDict.Add("Attack_Forward", attackForward);

        //Action: Air Light Attack
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(7, 12, new Vector2(0.75f, -1.25f), new Vector2(1.25f, 1.5f), 0, 8, 9, "HurtBox", 0));
        Action air_light_attack = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 15);
        actionDict.Add("Air_Attack_Light", air_light_attack);

        //Action: Air Heavy Attack
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(15, 19, new Vector2(0.75f, -1.25f), new Vector2(1.25f, 1.5f), 0, 16, 20, "HurtBox", 1));
        Action air_heavy_attack = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 20);
        actionDict.Add("Air_Attack_Heavy", air_heavy_attack);

        //Action: Crouch Light Attack
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(7, 12, new Vector2(0.75f, -1.25f), new Vector2(1.25f, 1.5f), 0, 8, 9, "HurtBox", 0));
        Action crouch_light_attack = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 15);
        actionDict.Add("Crouch_Attack_Light", crouch_light_attack);

        //Action: Crouch Heavy Attack
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(15, 19, new Vector2(0.75f, -1.25f), new Vector2(1.25f, 1.5f), 0, 16, 20, "HurtBox", 1));
        Action crouch_heavy_attack = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 20);
        actionDict.Add("Crouch_Attack_Heavy", crouch_heavy_attack);

        //Action: Recovery
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(0, 0, new Vector2(0f, 0f), new Vector2(0f, 0f), 0, 0, 10, "HurtBox", 0));
        Action recovery = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 10);
        actionDict.Add("Recovery", recovery);

        //Action: Grab
        attack_list = new List<Attack>();
        attack_list.Add(new Attack(10, 20, new Vector2(0.5f, 0f), new Vector2(1f, 0f), 0, 20, 40, "HurtBox", 2));
        Action grab = new Action(attack_list, new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 50);
        actionDict.Add("Grab", grab);

        //Action: Guard
        Action guard = new Action(new List<Attack>(), new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 5);
        actionDict.Add("Guard", guard);

        //Action: Crouch Guard
        Action crouchGuard = new Action(new List<Attack>(), new List<Movement>(), new InputBuffer(-1, -1, new BufferHandle()), 5);
        actionDict.Add("Crouch_Guard", crouchGuard);

        return actionDict;  

        //Box colider 2d change offset,size to fix character hurtbox when crouch, jump bra bra bra
    }
}
