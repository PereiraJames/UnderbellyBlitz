using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class BearerOfBadNews_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject PlayerSlot = PlayerManager.PlayerSlot;

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards" && child.gameObject != gameObject)
            {
                PlayerManager.CmdDealDamage(child.gameObject, -1);
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
