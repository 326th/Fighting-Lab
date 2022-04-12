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
        hitRate = data.hitCount / data.attackCount;
        hitRateText.text = hitRate.ToString();
    }
}
