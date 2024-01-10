using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class EternalDevourer_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject PlayerSlot = PlayerManager.PlayerSlot;

        int count = 0;

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards" && child.gameObject != gameObject)
            {
                PlayerManager.CmdDestroyTarget(child.gameObject);
                count++;
            }
        }

        for (int i =0; i < count; i++)
        {
            PlayerManager.CmdSummonMinion(3,1,true);
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
