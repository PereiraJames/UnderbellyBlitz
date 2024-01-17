using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostedAccount_CS : CardAbilities
{
    public override void OnEntry()
    {
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

    public override void OnStartTurn()
    {
        
    }
}
