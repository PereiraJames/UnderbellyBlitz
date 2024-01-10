using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class AWhippedPerson_CS : CardAbilities
{
    public override void OnEntry()
    {
    }

    public override void OnEndTurn()
    {
        PlayerManager.CmdSummonMinion(1,1,false);
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
