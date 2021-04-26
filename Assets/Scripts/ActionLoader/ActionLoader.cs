using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLoader : ClassScript
{
    protected Dictionary<string, Action> actionDict = new Dictionary<string, Action>();
    private Dictionary<Action, string> reverseActionDoct = new Dictionary<Action, string>();

    public virtual Dictionary<string, Action> GetDictionary()
    {
        return new Dictionary<string, Action>();
    }
    public Dictionary<Action, string> GetReverseDictionary()
    {
        foreach(string actionName in actionDict.Keys)
        {
            reverseActionDoct.Add(actionDict[actionName],actionName);
        }
        return reverseActionDoct;
    }
}
