using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DeckSelection : NetworkBehaviour
{
    public GameObject DeckSelectionUI;
    public PlayerManager PlayerManager;
    public GameManager GameManager;
    private SoundManager SoundManager;

    public string DeckTag;

    public void OnSelected()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        PlayerManager.CmdDeckSelection(DeckTag);
        
        DeckSelectionUI = GameObject.Find("DeckSelectionUI");

        UIManager UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void PlayerReady()
    {
        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        DeckSelectionUI = GameObject.Find("DeckSelectionUI");

        if(GameManager.PlayerDeck != "")
        {
            SoundManager.PlayDeckFX();
            PlayerManager.CmdDealCards(5,GameManager.PlayerDeck);
            PlayerManager.CmdPlayerReadyUp();
            foreach (Transform child in DeckSelectionUI.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.tag == "DeckUI")
                {
                    child.GetComponent<Button>().interactable = false;
                }
            }
        }

        if(GameManager.EnemyReady == true)
        {
            PlayerManager.CmdDestoryDeckSelectionUI();
        }  

        gameObject.GetComponent<Button>().interactable = false;
    } 
}