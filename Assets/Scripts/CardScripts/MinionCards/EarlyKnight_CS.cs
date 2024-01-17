using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class EarlyKnight_CS : CardAbilities
{
    bool StartTurn = false;
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

    }

    public override void OnStartTurn()
    {
        if(StartTurn == true){return;}
        StartTurn = true;

        GameObject PlayerSlot = PlayerManager.PlayerSlot;
        GameObject EnemySlot = PlayerManager.EnemySlot;

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerManager.CmdDestroyTarget(child.gameObject);
            }
        }

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerManager.CmdDestroyTarget(child.gameObject);
            }
        }
    }
}
