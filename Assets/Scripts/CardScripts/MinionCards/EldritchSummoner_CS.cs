using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class EldritchSummoner_CS : CardAbilities
{
    public override void OnEntry()
    {

    }

    public override void OnEndTurn()
    {
        PlayerManager.CmdSummonMinion(1,1,true);
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
