using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class TheonSlothBorne_CS : CardAbilities
{
    public override void OnEntry()
    {
      
    }

    public override void OnEndTurn()
    {
        GameObject EnemySlot = PlayerManager.EnemySlot;
        GameObject PlayerSlot = PlayerManager.PlayerSlot;

        int amountofPlayerCards = 0;

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                amountofPlayerCards++;
            }
        } 

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerManager.CmdDealDamage(child.gameObject, amountofPlayerCards);
            }
        } 
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
