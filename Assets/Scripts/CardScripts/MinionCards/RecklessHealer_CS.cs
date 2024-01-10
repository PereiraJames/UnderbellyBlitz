using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RecklessHealer_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject PlayerSlot = PlayerManager.PlayerSlot;
        
        int targetRandom = Random.Range(0, PlayerSlot.transform.childCount);
        int count = 0;
        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                if(count == targetRandom)
                {
                    PlayerManager.CmdCardStatChange(1,1,child.gameObject);
                }
                count++;
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
