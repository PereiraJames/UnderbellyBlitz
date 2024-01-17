using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnseenDevastation : CardAbilities
{
    public override void OnEntry()
    {
       GameObject EnemySlot = PlayerManager.EnemySlot;
       GameObject PlayerSlot = PlayerManager.PlayerSlot;

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                if(child.GetComponent<CardDetails>().GetCardHealth() < 5)
                {
                    PlayerManager.CmdDestroyTarget(child.gameObject);
                }
            }
        }
        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                if(child.GetComponent<CardDetails>().GetCardHealth() < 5)
                {
                    PlayerManager.CmdDestroyTarget(child.gameObject);
                }
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
