using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndgameScreenHandler : MonoBehaviour
{
    public float hitRate;
    public Text hitRateText;
    void Start()
    {
        
    }

    public void UpdateText()
    {
        PlayerStats data = SaveSystem.LoadPlayer();
        print(data.hitCount);
        print(data.attackCount);
        if (data.attackCount != 0)
        {
            hitRate = (float)data.hitCount / (float)data.attackCount;
        }
        
        hitRateText.text = "Hit Rate (%): " + hitRate.ToString();
    }
}
