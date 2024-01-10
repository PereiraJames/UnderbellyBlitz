using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MisguidedArtillerist_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject EnemySlot = PlayerManager.EnemySlot;
        GameObject PlayerSlot = PlayerManager.PlayerSlot;

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerManager.CmdDealDamage(child.gameObject, -1);
            }
        }

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
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

    public override void OnSilenced()
    {
        
    }
}
