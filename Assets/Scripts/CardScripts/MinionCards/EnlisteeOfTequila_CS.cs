using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class EnlisteeOfTequila_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdGMPlayerHealth(-3);
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
