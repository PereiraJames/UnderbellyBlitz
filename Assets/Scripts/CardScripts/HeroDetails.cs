using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HeroDetails : NetworkBehaviour
{
    private GameManager GameManager;
    private GameObject Canvas;
    private PlayerManager PlayerManager;
    private RectTransform RectPlayerSlot;


    private GameObject EnemySlot;
    private GameObject PlayerSlot; 
    private GameObject AttackingDisplay;
    private GameObject AttackingCard;

    public string DeckTag = "";
    
    public bool CanHeroPower = true;

    public void OnSelected()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        if(PlayerManager.AttackDisplayOpened)
        {
            if(PlayerManager.AttackBeingMade && PlayerManager.AttackingTarget != null && !isOwned)
            {
                if(gameObject != PlayerManager.AttackingTarget)
                {
                    PlayerManager.CmdAttackingDetails(gameObject, 1);
                }
            }
            else
            {
                Debug.Log("DestroyBeingMade " + PlayerManager.DestroyBeingMade + " AttackBeingMade " + PlayerManager.AttackBeingMade);
            }
        }
    }
    
    public void OnHeroPowerHover()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        string HeroPowerText = "";
        string DeckTag = "";

        if(gameObject == GameObject.Find("PlayerHeroPower"))
        {
            DeckTag = GameManager.PlayerDeck;
        }
        else if (gameObject == GameObject.Find("EnemyHeroPower"))
        {
            DeckTag = GameManager.EnemyDeck;
        }

        if(DeckTag == "Keagan")
        {
            HeroPowerText = "Deal 2 Damage To Yourself. Gain 1 Doubloon.";
        }
        else if(DeckTag == "Mark")
        {
            HeroPowerText = "Restore 2 Health To Yourself.";
        }
        else if(DeckTag == "Chris")
        {
            HeroPowerText = "Draw A Card.";
        }
        else if (DeckTag == "Deion")
        {
            HeroPowerText = "Give A Random Minion +1/+1.";
        }

        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            child.GetComponent<Image>().enabled = true;
            child.GetComponentInChildren<Text>().enabled = true;
            child.GetComponentInChildren<Text>().text = HeroPowerText;
        }
    }

    public void OnHeroPowerUnHover()
    {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            child.GetComponent<Image>().enabled = false;
            child.GetComponentInChildren<Text>().enabled = false;
        } 
    }

    public void HeroPower()
    {

        GameManager GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        string DeckTag = GameManager.PlayerDeck;

        int currentPlayerDoubloons = GameManager.currentPlayerDoubloons;

        int heroPowerCost = 2;

        if(currentPlayerDoubloons >= heroPowerCost && PlayerManager.IsMyTurn)
        {
            if(DeckTag == "Keagan")
            {
                PlayerManager.CmdGMPlayerHealth(-2);
                PlayerManager.CmdUpdateDoubloons(1, true);
                HeroPowerInactive(heroPowerCost);
            }
            else if (DeckTag == "Mark")
            {
                PlayerManager.CmdDealCards(1, GameManager.PlayerDeck);
                HeroPowerInactive(heroPowerCost);
            }
            else if (DeckTag == "Chris")
            {
                PlayerManager.CmdGMPlayerHealth(2);
                HeroPowerInactive(heroPowerCost);
            }
            else if (DeckTag == "Deion")
            {
                GameObject PlayerSlot = PlayerManager.PlayerSlot;

                if(PlayerSlot.transform.childCount > 0)
                {
                    int targetRandom = Random.Range(0, PlayerSlot.transform.childCount);
                    int count = 0;
                    foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
                    {
                        if (child.gameObject.tag == "Cards")
                        {
                            if(count == targetRandom)
                            {
                                PlayerManager.CmdCardStatChange(1,1,child.gameObject);
                            }
                            count++;
                        }
                    }
                    HeroPowerInactive(heroPowerCost);
                }
            }
            else
            {
                Debug.Log("Missing HeroPower");
            }

        }
    }

    public void HeroPowerInactive(int heroPowerCost)
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        PlayerManager.CmdUpdateDoubloons(heroPowerCost,false);
        PlayerManager.CmdHeroPowerActive(false);
    }

    public void HeroPowerActive()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        PlayerManager.CmdHeroPowerActive(true);
    }
    
}
