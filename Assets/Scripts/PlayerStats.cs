using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{

    public List<List<float>> matchesData = new List<List<float>>();

    public PlayerStats (Character_Base player, float playTime)
    {
        matchesData.Add( new List<float> { player.attackCount, player.hitCount, player.comboCount, player.lightAttackCount,
                                        player.heavyAttackCount, player.attackForwardCount, player.airLightAttackCount,
                                        player.airHeavyAttackCount, player.crouchLightAttackCount, player.crouchHeavyAttackCount,
                                        player.jumpCount, player.grabCount, player.guardCount, player.crouchGuardCount,
                                        player.moveForwardCount, player.moveBackwardCount, player.attackedOnAirCount,
                                        player.grabbedOnGuardCount, player.hitPoints, playTime
                                        });
    }

}
