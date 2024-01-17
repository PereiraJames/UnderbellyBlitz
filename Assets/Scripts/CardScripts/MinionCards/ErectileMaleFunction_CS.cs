using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErectileMaleFunction_CS : CardAbilities
{
    public override void OnEntry()
    {
        gameObject.GetComponent<CardDetails>().DestroyTarget();
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
