using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernut_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdSetPlayerHealth(5);
        PlayerManager.CmdGMEnemyHealth(-5);
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
