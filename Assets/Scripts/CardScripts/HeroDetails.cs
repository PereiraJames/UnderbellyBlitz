using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HeroDetails : NetworkBehaviour
{
    private GameManager GameManager;
    private PlayerManager PlayerManager;
    private UIManager UIManager;
    private GameObject Canvas;
    private RectTransform RectPlayerSlot;


    private GameObject EnemySlot;
    private GameObject PlayerSlot; 
    private GameObject AttackingDisplay;
    private GameObject AttackingCard;

    public string DeckTag = "";
    
    public bool CanHeroPower = true;

    void Start()
    {
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

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

    public void SetHighlightSprite()
    {
        
    }

    public void OnHover()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        Image TargetHighlight = null;
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            if(child.gameObject.name == "EnemyHighlight" || child.gameObject.name == "PlayerHighlight")
            {
                TargetHighlight = child.GetComponent<Image>();
            }
        }
        
        if(TargetHighlight != null)
        {
            if(TargetHighlight.sprite == UIManager.RedPlayerHighlight)
            {
                PlayerManager.CmdHighlightHero("purple", true, false);
            }
            else
            {
                TargetHighlight.sprite = UIManager.BluePlayerHighlight;
            }
        }
    }

    public void OnHoverExit()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        
        Image TargetHighlight = null;
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            if(child.gameObject.name == "EnemyHighlight" || child.gameObject.name == "PlayerHighlight")
            {
                TargetHighlight = child.GetComponent<Image>();
            }
        }

        if(TargetHighlight != null)
        {
            if(TargetHighlight.sprite == UIManager.PurplePlayerHighlight)
            {
                PlayerManager.CmdHighlightHero("red", true, false);
            }
            else if(PlayerManager.IsMyTurn) 
            {
                PlayerManager.CmdHighlightHero("turn", true, true);
                if(TargetHighlight.sprite == UIManager.BluePlayerHighlight)
                {
                    TargetHighlight.sprite = UIManager.NoPlayerHighlight;
                }
            }
            else if(!PlayerManager.IsMyTurn)
            {
                PlayerManager.CmdHighlightHero("turn", true, false);
                if(TargetHighlight.sprite == UIManager.BluePlayerHighlight)
                {
                    TargetHighlight.sprite = UIManager.NoPlayerHighlight;
                }
            }
            else if(TargetHighlight.sprite == UIManager.BluePlayerHighlight)
            {
                TargetHighlight.sprite = UIManager.NoPlayerHighlight;
            }
            else
            {
                PlayerManager.CmdHighlightHero("blue",false, false);
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
                PlayerManager.CmdGMPlayerHealth(2);
                HeroPowerInactive(heroPowerCost);
            }
            else if (DeckTag == "Chris")
            {
                PlayerManager.CmdDealCards(1, GameManager.PlayerDeck);
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
