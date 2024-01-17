using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class SummonerOfTheDeep_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdSummonMinion(1,1,true);
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

    public override void OnStartTurn()
    {
        
    }
}
