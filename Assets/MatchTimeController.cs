using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchTimeController : MonoBehaviour
{
    public static MatchTimeController Instance;
    public int matchTime;
    public TextMeshProUGUI matchTimeDisplay;
    public int playTime;

    private void Start()
    {
        Instance = this;
    }
    
    public void StartDecreaseMatchTime()
    {
        StartCoroutine(DecreaseMatchTime());
    }
    public void StopDecreaseMatchTime()
    {
        StopCoroutine(DecreaseMatchTime());
    }
    IEnumerator DecreaseMatchTime()
    {
        while (matchTime > 0)
        {
            matchTimeDisplay.text = matchTime.ToString();

            yield return new WaitForSeconds(1f);

            matchTime--;
            playTime++;
        }

        StopDecreaseMatchTime();
    }
}
