using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableOrge_CS : CardAbilities
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
            PlayerManager.DiscardCards(1,true);
        }
    }
    
    public override void OnLastResort()
    {

    }

    public override void OnStartTurn()
    {
        
    }
}
