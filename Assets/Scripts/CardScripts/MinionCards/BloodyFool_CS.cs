using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class BloodyFool_CS : CardAbilities
{
    public override void OnEntry()
    {
        gameObject.GetComponent<CardDetails>().DestroyTarget();
        PlayerManager.CmdSummonMinion(1,1,false);
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
