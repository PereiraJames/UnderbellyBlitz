using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrritatedGuest_CS : CardAbilities
{
    public override void OnEntry()
    {

    }

    public override void OnEndTurn()
    {
        
    }

    public override void OnHit()
    {
        if(isOwned)
        {
            PlayerManager.CmdCardStatChange(2,0,gameObject);
        }
    }
    
    public override void OnLastResort()
    {

    }

    public override void OnStartTurn()
    {
        
    }
}
