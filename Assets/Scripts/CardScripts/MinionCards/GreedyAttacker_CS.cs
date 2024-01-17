using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GreedyAttacker_CS : CardAbilities
{
    public override void OnEntry()
    {
    }

    public override void OnEndTurn()
    {
        PlayerManager.CmdGMEnemyHealth(-1);
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
