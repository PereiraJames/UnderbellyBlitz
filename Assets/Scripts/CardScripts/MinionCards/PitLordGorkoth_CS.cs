using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class PitLordGorkoth_CS : CardAbilities
{
    public override void OnEntry()
    {
        int totalDead = 0;

        GameObject EnemySlot = PlayerManager.EnemySlot;
        GameObject PlayerSlot = PlayerManager.PlayerSlot;

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                if(child.gameObject.GetComponent<CardDetails>().GetCardHealth() == 1)
                {
                    totalDead ++;
                }
                PlayerManager.CmdDealDamage(child.gameObject, 1);
            }
        }

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                if(child.gameObject.GetComponent<CardDetails>().GetCardHealth() == 1)
                {
                    totalDead ++;
                }
                PlayerManager.CmdDealDamage(child.gameObject, 1);
            }
        }

        PlayerManager.CmdDealCards(totalDead, GameManager.PlayerDeck);

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
