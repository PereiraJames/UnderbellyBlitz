using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class BattleMedic_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject PlayerSlot = PlayerManager.PlayerSlot;

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerManager.CmdCardStatChange(1,1,child.gameObject);
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
