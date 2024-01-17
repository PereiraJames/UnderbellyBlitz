using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameSame_CS : CardAbilities
{
    public override void OnEntry()
    {
        int health = gameObject.GetComponent<CardDetails>().GetCardAttack();
        int attack = gameObject.GetComponent<CardDetails>().GetCardHealth();

        PlayerManager.CmdSummonMinion(health,attack,true);
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
