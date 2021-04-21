using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLoader : ClassScript
{
    protected Dictionary<string, Action> ACTION_DICT = new Dictionary<string, Action>();
    private Dictionary<Action, string> REVERSE_ACTION_DICT = new Dictionary<Action, string>();
    [SerializeField] protected float JUMP_VEL = 10f;

    public virtual Dictionary<string, Action> GetDictionary()
    {
        return new Dictionary<string, Action>();
    }
    public Dictionary<Action, string> GetReverseDictionary()
    {
        foreach(string actionName in ACTION_DICT.Keys)
        {
            REVERSE_ACTION_DICT.Add(ACTION_DICT[actionName],actionName);
        }
        return REVERSE_ACTION_DICT;
    }
}
