using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AuchentoshanFiend_CS : CardAbilities
{
    public override void OnEntry()
    {
    }

    public override void OnEndTurn()
    {
        GameObject EnemySlot = PlayerManager.EnemySlot;
        GameObject PlayerSlot = PlayerManager.PlayerSlot;
        int ranNum = Random.Range(0,3);

        if(ranNum == 0)
        {
            PlayerManager.CmdGMEnemyHealth(-2);
        }
        else if (ranNum == 1)
        {
            PlayerManager.CmdGMPlayerHealth(-2);
        }
        else if(ranNum == 2)
        {
            int targetRandom = Random.Range(0, EnemySlot.transform.childCount);
            int count = 0;
            foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    if(targetRandom == count)
                    {
                        PlayerManager.CmdDealDamage(child.gameObject, 2);
                    }
                    count++;
                }
            }
        }
        else
        {
            int targetRandom = Random.Range(0, PlayerSlot.transform.childCount);
            int count = 0;
            foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    if(targetRandom == count)
                    {
                        PlayerManager.CmdDealDamage(child.gameObject, 2);
                    }
                    count++;
                }
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
