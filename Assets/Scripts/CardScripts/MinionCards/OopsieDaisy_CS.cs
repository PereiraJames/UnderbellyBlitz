using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OopsieDaisy_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdSummonMinion(2,4,false);
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
