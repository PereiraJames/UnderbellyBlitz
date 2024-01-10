using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FD_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdGMPlayerHealth(2);
        PlayerManager.CmdGMEnemyHealth(-1);
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
