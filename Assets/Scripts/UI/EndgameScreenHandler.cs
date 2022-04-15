using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class EndgameScreenHandler : MonoBehaviour
{
    public float hitRate;
    public TextMeshProUGUI hitRateText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI hitText;
    public TextMeshProUGUI missText;
    public Transform strengths_list;
    public Transform weaknesses_list;

    private int attackCount;
    private int hitCount;
    private int comboCount;
    private int lightAttackCount;
    private int heavyAttackCount;
    private int attackForwardCount;
    private int airLightAttackCount;
    private int airHeavyAttackCount;
    private int crouchLightAttackCount;
    private int crouchHeavyAttackCount;
    private int jumpCount;
    private int grabCount;
    private int guardCount;
    private int crouchGuardCount;
    private int moveForwardCount;
    private int moveBackwardCount;
    private int attackedOnAirCount;
    private int grabbedOnGuardCount;

    private int minCombo = 2;
    private int maxCombo = 5;
    private int diffMovingFrame = 75;
    private int repeatlyMoveThreshold = 15;
    private int repeatlyMoveFrameThreshold = 75;
    private int heavyAttackThreshold = 25;
    private int fewMoveThreshold = 5;
    private int fewGrabThreshold = 2;

    void Start()
    {
        PlayerStats matches = SaveSystem.LoadPlayer();
        List<int> lastestMatch = matches.matchesData.Last();

        attackCount = lastestMatch[0];
        hitCount = lastestMatch[1];
        comboCount = lastestMatch[2];
        lightAttackCount = lastestMatch[3];
        heavyAttackCount = lastestMatch[4];
        attackForwardCount = lastestMatch[5];
        airLightAttackCount = lastestMatch[6];
        airHeavyAttackCount = lastestMatch[7];
        crouchLightAttackCount = lastestMatch[8];
        crouchHeavyAttackCount = lastestMatch[9];
        jumpCount = lastestMatch[10];
        grabCount = lastestMatch[11];
        guardCount = lastestMatch[12];
        crouchGuardCount = lastestMatch[13];
        moveForwardCount = lastestMatch[14];
        moveBackwardCount = lastestMatch[15];
        attackedOnAirCount = lastestMatch[16];
        grabbedOnGuardCount = lastestMatch[17];
    }

    public void UpdateText()
    {
        

        // Calculate hitrate
        if (attackCount != 0) // Attack count != 0
        {
            hitRate = (float) hitCount / (float) attackCount;
            hitRate = hitRate * 100;
        }

        // Update hitrate text
        hitRateText.text = hitRate.ToString("0.00") + "%";
        hitText.text = hitCount.ToString();
        missText.text = (attackCount - hitCount).ToString();

        CheckStrengths();
        CheckWeaknesses();
        //AddStrength("Youre a god", "Youre too good!!!");
        //AddWeakness("Noob", "Try harder!!!");
        AddWeakness("Heavy Attacker", "You use too many big scale attack E.g.HeavyAttack, AirHeavyAttack, CrouchHeavyAttack, and AttackForward. Enemy can block and counter attack you. Becareful!");



    }

    public void AddStrength(string text, string desText)
    {
        GameObject textGO = new GameObject("strength");
        GameObject desGO = new GameObject("strengthdes");

        textGO.transform.SetParent(strengths_list);
        desGO.transform.SetParent(strengths_list);

        TextMeshProUGUI myText = textGO.AddComponent<TextMeshProUGUI>();
        myText.text = text;
        myText.fontSize = 15;
        myText.alignment = TextAlignmentOptions.Center;

        TextMeshProUGUI myDesText = desGO.AddComponent<TextMeshProUGUI>();
        myDesText.text = desText;
        myDesText.fontSize = 9;
        myDesText.alignment = TextAlignmentOptions.TopJustified;


    }

    public void AddWeakness(string text, string desText)
    {
        GameObject textGO = new GameObject("weakness");
        GameObject desGO = new GameObject("weaknessdes");

        textGO.transform.SetParent(weaknesses_list);
        desGO.transform.SetParent(weaknesses_list);

        TextMeshProUGUI myText = textGO.AddComponent<TextMeshProUGUI>();
        myText.text = text;
        myText.fontSize = 15;
        myText.alignment = TextAlignmentOptions.Center;

        TextMeshProUGUI myDesText = desGO.AddComponent<TextMeshProUGUI>();
        myDesText.text = desText;
        myDesText.fontSize = 9;
        myDesText.alignment = TextAlignmentOptions.TopJustified;
    }
    public void CheckStrengths()
    {
        if (comboCount >= maxCombo)
        {
            AddStrength("Combo Master!", "You are just too good at making combo!!");
        }
    }

    public void CheckWeaknesses()
    {
        if (comboCount <= minCombo)
        {
            AddWeakness("Comboless", "You use just a few combo, try using J->K more often!");
        }
       if (attackedOnAirCount != 0)
        {
            AddWeakness("Mid-air damaged", "You got attacked while in mid-air, You should avoid jumping that expose your weakpoint");
        }
       if (grabbedOnGuardCount != 0)
        {
            AddWeakness("Guard break", "You got grabbed while guarding, You should not guarding so long time to avoid opponent to grab you");
        }
       if (Mathf.Abs(moveForwardCount - moveBackwardCount) > diffMovingFrame)
        {
            if (moveForwardCount > moveBackwardCount)
            {
                AddWeakness("Too aggressive", "You approch to opponent too much, You should back up more often");
            }
            else
            {
                AddWeakness("Too defensive", "You move backward too much. You should try to approch to opponent more often");
            }
        }
        if (heavyAttackCount + airHeavyAttackCount + crouchHeavyAttackCount > heavyAttackThreshold)
        {
            AddWeakness("Heavy Attacker", "You use too many big scale attack E.g.HeavyAttack, AirHeavyAttack, CrouchHeavyAttack, and AttackForward. Enemy can block and counter attack you. Becareful!");
        }
        if (jumpCount <= fewMoveThreshold)
        {
            AddWeakness("Ground Fighter", "You jump too little, You should try to jump more often");
        }
        if (grabCount <= fewGrabThreshold)
        {
            AddWeakness("Fighter who doesn't grab", "You grab too little, You should try to grab more often");
        }

        //Check repeat moves
        if (lightAttackCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many light-attack", "You use light-attack repeatly. You should use varios move to be unpredictable");
        }
        if (heavyAttackCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many heavy-attack", "You use heavy-attack repeatly. You should use varios move to be unpredictable");
        }
        if (attackForwardCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many attack forward", "You use attack forward repeatly. You should use varios move to be unpredictable");
        }
        if (crouchLightAttackCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many crouch light-attack", "You use crouch light-attack repeatly. You should use varios move to be unpredictable");
        }
        if (crouchHeavyAttackCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many crouch heavy-attack", "You use crouch heavy-attack repeatly. You should use varios move to be unpredictable");
        }
        if (airLightAttackCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many air light-attack", "You use air light-attack repeatly. You should use varios move to be unpredictable");
        }
        if (airHeavyAttackCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many air heavy-attack", "You use air heavy-attack repeatly. You should use varios move to be unpredictable");
        }
        if (crouchLightAttackCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Too many crouch light-attack", "You use crouch light-attack repeatly, You should use varios move to be unpredictable");
        }
        if (guardCount >= repeatlyMoveFrameThreshold)
        {
            AddWeakness("Too many grard", "You use guard too often. You should use varios move to be unpredictable");
        }
        if (crouchGuardCount >= repeatlyMoveFrameThreshold)
        {
            AddWeakness("Too many crouch grard", "You use crouch guard too often, You should use varios move to be unpredictable");
        }
        if (jumpCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Jumper", "You use jump too often, You should use varios move to be unpredictable");
        }
        if (grabCount >= repeatlyMoveThreshold)
        {
            AddWeakness("Grabber", "You use grab too often. You should use varios move to be unpredictable");
        }

        

    }
}
