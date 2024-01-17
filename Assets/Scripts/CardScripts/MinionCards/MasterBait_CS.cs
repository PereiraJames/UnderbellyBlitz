using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBait_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject EnemySlot = PlayerManager.EnemySlot;
        int amountOfEnemies = 0;
        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                amountOfEnemies ++;
            }
        }

        if(amountOfEnemies >= 2)
        {
            PlayerManager.CmdSummonMinion(1,2,true);
        }
    }

    public override void OnEndTurn()
    {
        
    }

    public override void OnHit()
    {

    }
    
    public override void OnLastResort()
    {

    }

    public override void OnStartTurn()
    {
        
    }
}
