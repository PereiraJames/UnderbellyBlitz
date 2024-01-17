using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class CardAbilities : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;

    void Start()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public abstract void OnEntry();

    public abstract void OnEndTurn();

    public abstract void OnHit();
    
    public abstract void OnLastResort();

    public abstract void OnStartTurn();
}
