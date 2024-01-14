using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class HalfPint_CS : CardAbilities
{
    public override void OnEntry()
    {
        PlayerManager.CmdDealCards(1, GameManager.PlayerDeck);
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
