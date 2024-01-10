using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterPlanner_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdGMEnemyHealth(5);
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

    public override void OnSilenced()
    {
        
    }
}
