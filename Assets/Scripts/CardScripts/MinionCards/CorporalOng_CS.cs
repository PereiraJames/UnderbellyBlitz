using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CorporalOng_CS : CardAbilities
{
    public override void OnEntry()
    {
        return;
    }

    public override void OnEndTurn()
    {
        
    }

    public override void OnHit()
    {
        if(isOwned)
        {
            PlayerManager.CmdDealCards(1,GameManager.PlayerDeck);
        }
    }
    
    public override void OnLastResort()
    {

    }

    public override void OnSilenced()
    {
        
    }
}
