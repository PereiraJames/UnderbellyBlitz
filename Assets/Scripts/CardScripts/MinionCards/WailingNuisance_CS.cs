using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WailingNuisance_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject EnemySlot = PlayerManager.EnemySlot;

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerManager.CmdDealDamage(child.gameObject, -3);
            }
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
