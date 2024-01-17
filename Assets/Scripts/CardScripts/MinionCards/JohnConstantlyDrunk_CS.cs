using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class JohnConstantlyDrunk_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdSummonMinion(1,1,false);
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

    public override void OnStartTurn()
    {
        
    }
}
