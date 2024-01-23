using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawCards : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;
    public NetworkManager NetworkManager;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        if (PlayerManager.IsMyTurn)
        {
            Endturn();
        }
    }

    void Endturn()
    {
        if(PlayerManager.DestroyDisplayOpen){return;}
        PlayerManager.CmdUpdateDoubloons(1,true);
        PlayerManager.CmdDealCards(1, GameManager.PlayerDeck);
        PlayerManager.CmdEndTurn();
    }
}