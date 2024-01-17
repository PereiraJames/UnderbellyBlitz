using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleExplorer_CS : CardAbilities
{
    public override void OnEntry()
    {

    }

    public override void OnEndTurn()
    {   
        PlayerManager.CmdUpdateDoubloons(1, true);
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
