using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyOfJuan_CS : CardAbilities
{
    public override void OnEntry()
    {

    }

    public override void OnEndTurn()
    {
        PlayerManager.CmdSummonMinion(2,2,true);
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
