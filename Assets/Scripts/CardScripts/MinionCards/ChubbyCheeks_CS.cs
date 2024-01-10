using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChubbyCheeks_CS : CardAbilities
{
    public override void OnEntry()
    {
    }

    public override void OnEndTurn()
    {
        
    }

    public override void OnHit()
    {

    }
    
    public override void OnLastResort()
    {
        PlayerManager.CmdDealCards(1, GameManager.PlayerDeck);
    }

    public override void OnSilenced()
    {
        
    }
}
