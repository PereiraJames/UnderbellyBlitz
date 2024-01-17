using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class FriendlyFirer_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdGMPlayerHealth(-3);
        PlayerManager.CmdUpdateDoubloons(3, true);
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
