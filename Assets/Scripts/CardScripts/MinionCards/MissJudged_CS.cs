using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MissJudged_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdGMEnemyHealth(4);
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
