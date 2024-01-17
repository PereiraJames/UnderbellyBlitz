using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWin_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdGMEnemyHealth(-50);
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
