using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    //public float hitRate;

    //public int attackCount;
    //public int hitCount;
    //public int comboCount;
    //public int lightAttackCount;
    //public int heavyAttackCount;
    //public int attackForwardCount;
    //public int airLightAttackCount;
    //public int airHeavyAttackCount;
    //public int crouchLightAttackCount;
    //public int crouchHeavyAttackCount;
    //public int jumpCount;
    //public int grabCount;
    //public int guardCount;
    //public int crouchGuardCount;

    public List<List<int>> matchesData = new List<List<int>>();

    //public int[] moveForward;
    //public int[] moveBackward;
    //public int[] crouch;

    public PlayerStats (Character_Base player)
    {
        matchesData.Add( new List<int> { player.attackCount, player.hitCount, player.comboCount, player.lightAttackCount,
                                        player.heavyAttackCount, player.attackForwardCount, player.airLightAttackCount,
                                        player.airHeavyAttackCount, player.crouchLightAttackCount, player.crouchHeavyAttackCount,
                                        player.jumpCount, player.grabCount, player.guardCount, player.crouchGuardCount,
                                        player.moveForwardCount, player.moveBackwardCount, player.attackedOnAirCount,
                                        player.grabbedOnGuardCount
                                        });
        //attackCount = player.attackCount;
        //hitCount = player.hitCount;
        //comboCount = player.comboCount;
        //lightAttackCount = player.lightAttackCount;
        //heavyAttackCount = player.heavyAttackCount;
        //attackForwardCount = player.attackForwardCount;
        //airLightAttackCount = player.airLightAttackCount;
        //airHeavyAttackCount = player.airHeavyAttackCount;
        //crouchLightAttackCount = player.crouchLightAttackCount;
        //crouchHeavyAttackCount = player.crouchHeavyAttackCount;
        //jumpCount = player.jumpCount;
        //grabCount = player.grabCount;
        //guardCount = player.guardCount;
        //crouchGuardCount = player.crouchGuardCount;
    }

}
