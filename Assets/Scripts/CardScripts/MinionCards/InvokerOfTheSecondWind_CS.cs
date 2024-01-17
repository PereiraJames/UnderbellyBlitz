using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InvokerOfTheSecondWind_CS : CardAbilities
{
    public override void OnEntry()
    {

    }

    public override void OnEndTurn()
    {

    }

    public override void OnHit()
    {

    }
    
    public override void OnLastResort()
    {
        if(isOwned)
        {
            PlayerManager.CmdSummonMinion(3,3,true);
        }
    }

    public override void OnStartTurn()
    {
        
    }
}
