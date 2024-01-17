using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnragedBeast_CS : CardAbilities
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
        PlayerManager.CmdGMPlayerHealth(-3);
    }

    public override void OnStartTurn()
    {
        
    }
}
