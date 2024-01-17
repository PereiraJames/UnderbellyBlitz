using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TohPeng_CS : CardAbilities
{
    public override void OnEntry()
    {

    }

    public override void OnEndTurn()
    {
        
    }

    public override void OnHit()
    {

    }
    
    public override void OnLastResort()
    {
        GameObject EnemySlot = PlayerManager.EnemySlot;

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerManager.CmdDealDamage(child.gameObject, -2);
            }
        }
    }

    public override void OnStartTurn()
    {
        
    }
}
