using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class BoreHunter_CS : CardAbilities
{
    public override void OnEntry()
    {
        gameObject.GetComponent<CardDetails>().DestroyTarget();
        PlayerManager.CmdSummonMinion(2,2,true);
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
