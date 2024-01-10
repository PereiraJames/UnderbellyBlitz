using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TripleThreat_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject PlayerSlot = PlayerManager.PlayerSlot;
        int count = 0;

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                if(child.gameObject.name == "TripleThreat(Clone)")
                {
                    if(count == 0)
                    {
                        PlayerManager.CmdSummonMinion(4,4,true);
                        count++;
                    }
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

    public override void OnSilenced()
    {
        
    }
}
