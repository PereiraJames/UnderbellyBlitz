using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

public class PlayerManager : NetworkBehaviour
{
    public GameObject WinText;

    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject PlayerSlot;
    public GameObject EnemySlot;

    public GameObject PlayerImage;
    public GameObject EnemyImage;

    public DrawCards DrawCards;

    public GameObject PlayerYard;
    public GameObject EnemyYard;
    public List <GameObject> PlayerSockets = new List<GameObject>();
    public List <GameObject> EnemySockets = new List<GameObject>();

    public List <GameObject> EnemyPlayedCards = new List<GameObject>();
    public List <GameObject> PlayerPlayedCards = new List<GameObject>();

    public int cardsPlayed = 0;

    public int handLimit = 8;

    public bool AttackBeingMade = false;
    public bool DestroyBeingMade = false;

    public bool AttackDisplayOpened = false;
    public bool DestroyDisplayOpen = false;

    private int currentPlayerCount = 0;


    public NetworkManager NetworkManager;
    private SoundManager SoundManager;

    [SyncVar]
    public GameObject AttackedTarget;

    [SyncVar]
    public GameObject AttackingTarget;

    public GameManager GameManager;
    public UIManager UIManager;

    public int CardsPlayed = 0;

    [SyncVar]
    public int PlayersReady = 0;

    public bool IsMyTurn = false;

    public List <string> PlayerDecks = new List<string>() {"Keagan", "Mark", "Deion", "Chris"};

    [SyncVar]
    public List <GameObject> cards = new List<GameObject>();

    [SyncVar]
    public List <GameObject> ChrisDeck = new List<GameObject>();

    [SyncVar]
    public List <GameObject> KeaganDeck = new List<GameObject>();

    [SyncVar]
    public List <GameObject> MarkDeck = new List<GameObject>();

    [SyncVar]
    public List <GameObject> DeionDeck = new List<GameObject>();

    private void HandleServerStarted()
    {
        Debug.Log("Server Started");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        DrawCards = GameObject.Find("Button").GetComponent<DrawCards>();

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");

        PlayerYard = GameObject.Find("PlayerYard");
        EnemyYard = GameObject.Find("EnemyYard");

        PlayerSlot = GameObject.Find("PlayerSlot");
        EnemySlot = GameObject.Find("EnemySlot");

        PlayerImage = GameObject.Find("PlayerImage");
        EnemyImage = GameObject.Find("EnemyImage");

        PlayerSockets.Add(PlayerSlot);
        EnemySockets.Add(EnemySlot);        

        if(isClientOnly)
        {
            CmdDestroyWaitingScreen();

            IsMyTurn = true;

            int ranNum = Random.Range(0,1);
            Debug.Log(ranNum);
            if(ranNum == 1)
            {
                RpcSwapTurn();
            } 
        }

        Debug.Log("MarkDeckSize : " + MarkDeck.Count);
        Debug.Log("ChrisDeckSize : " + ChrisDeck.Count);
        Debug.Log("KeaganDeckSize : " + KeaganDeck.Count);
        Debug.Log("DeionDeckSize : " + DeionDeck.Count);
        Debug.Log(NetworkServer.connections.Count);
    }

    public int Decksize(string deckName)
    {
        if (deckName == "Mark")
        {
            Debug.Log(MarkDeck.Count);
            return MarkDeck.Count;
        }
        else if(deckName == "Keagan")
        {
            Debug.Log(KeaganDeck.Count);
            return KeaganDeck.Count;
        }
        else if (deckName == "Chris")
        {
            Debug.Log(ChrisDeck.Count);
            return ChrisDeck.Count;
        }
        else if (deckName == "Deion")
        {
            Debug.Log(DeionDeck.Count);
            return DeionDeck.Count;
        }
        else
        {
            return cards.Count;
        }
    }

    public List<GameObject> CmdWhichDeck(string deckName)
    {
        if (deckName == "Mark")
        {
            return MarkDeck;
        }
        else if(deckName == "Keagan")
        {
            return KeaganDeck;
        }
        else if (deckName == "Chris")
        {
            return ChrisDeck;
        }
        else if (deckName == "Deion")
        {
            return DeionDeck;
        }
        else
        {
            return cards;
        }
    }

    [Server]
    public override void OnStartServer()
    {
        currentPlayerCount = 0;
        NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        foreach (GameObject card in NetworkManager.spawnPrefabs) //Auto puts prefabs into a list(or a deck)
        {
            if(card.tag == "Cards")
            {
                int amountOfEachCard = card.GetComponent<CardDetails>().amountOfEachCard;
                for (int i = 0; i < amountOfEachCard; i++)
                {
                    List<GameObject> PlayersDeck = CmdWhichDeck(card.GetComponent<CardDetails>().DeckTag);

                    PlayersDeck.Add(card);
                }
            }
        }
    }

    [ClientRpc]
    public void RpcSwapTurn()
    {
        PlayerManager pm = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        pm.IsMyTurn = !pm.IsMyTurn;
    }

    [Command]
    public void CmdClientStatus()
    {
        RpcClientStatus();
    }

    [ClientRpc]
    public void RpcClientStatus()
    {
        Debug.Log("isServer: " + isServer + " isClient" + isClient);
        if(NetworkServer.connections.Count == 1)
        {
            NetworkManager.StopHost();
        }
    }

    [Command]
    public void CmdConcedeGame()
    {
        RpcConcedeGame();
    }

    [ClientRpc]
    public void RpcConcedeGame()
    {
        CmdGMPlayerHealth(-1000);
    }

    [Command]
    public void CmdDestroyWaitingScreen()
    {
        RpcDestroyWaitingScreen();
    }

    [ClientRpc]
    public void RpcDestroyWaitingScreen()
    {
        Destroy(GameObject.Find("WaitingScreenUI"));
    }

    [Command]
    public void CmdDealCards(int cardAmount, string deckName)
    {
        int currentHandsize = 0;
        if(isOwned)
        {
            foreach(Transform child in PlayerArea.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.tag == "Cards")
                {
                    currentHandsize ++;
                }
            }
        }
        else
        {
            foreach(Transform child in EnemyArea.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.tag == "Cards")
                {
                    currentHandsize ++;
                }
            }
        }
        
        for (int i = 0; i < cardAmount; i++)
        {
            List<GameObject> deck = CmdWhichDeck(deckName);
            if(deck.Count != 0)
            {
                int ranNum = Random.Range(0,deck.Count - 1);
                GameObject card = Instantiate(deck[ranNum], new Vector3(0,0,0), Quaternion.identity);
                NetworkServer.Spawn(card, connectionToClient);
                if (isOwned && currentHandsize + 1 < handLimit)
                {
                    RpcSetParentCard(card);
                    deck.RemoveAt(ranNum);
                    currentHandsize ++;
                }
                else if (!isOwned && currentHandsize + 1 < handLimit)
                {
                    RpcSetParentCard(card);
                    deck.RemoveAt(ranNum);
                    currentHandsize ++;
                }
                else
                {
                    Destroy(card);
                    Debug.Log(card + "Destroyed");
                }
            }
        }
    }

    [ClientRpc]
    void RpcSetParentCard(GameObject card)
    {
        if(card != null)
        {
            if(isOwned)
            {
                card.transform.SetParent(PlayerArea.transform, false);
                card.GetComponent<CardDetails>().inHand = true;
                GameManager.PlayerDeckSize --;
                GameManager.PlayerHandSize ++;
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
                card.GetComponent<CardFlipper>().Flip();
                GameManager.EnemyDeckSize --;
                GameManager.EnemyHandSize ++;
            }
        }
    }

    [Command]
    public void CmdCardSleep(bool CanAttack, GameObject card)
    {
        if(card != null)
        {
            RpcCardSleep(CanAttack, card);
        }
    }

    [ClientRpc]
    public void RpcCardSleep(bool CanAttack, GameObject card)
    {
        if(card != null)
        {
            if(CanAttack)
            {
                card.GetComponent<Image>().color = new Color(255f, 255f, 255f);
            }
            else
            {
                card.GetComponent<Image>().color = new Color(200/255f, 200/255f, 200/255f);
            }
            card.GetComponent<CardDetails>().UpdateCardText();
        }
    }

    [Command]
    public void CmdHeroPowerActive(bool CanHeroPower)
    {
        RpcHeroPowerActive(CanHeroPower);
    }

    [ClientRpc]
    public void RpcHeroPowerActive(bool CanHeroPower)
    {
        if(isOwned)
        {
            GameObject HeroPower = GameObject.Find("PlayerHeroPower");
            if(CanHeroPower)
            {
                HeroPower.GetComponent<Image>().color = new Color(255f, 255f, 255f);
            }
            else
            {
                HeroPower.GetComponent<Image>().color = new Color(100/255f, 100/255f, 100/255f);
            }
        }
    }


    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
        card.GetComponent<CardAbilities>().OnEntry();
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if (isOwned && GameManager.PlayerHandSize + 1 < 8)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else if (!isOwned && GameManager.PlayerHandSize + 1 < 8)
            {
                card.transform.SetParent(EnemyArea.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
            else
            {
                Destroy(card);
                Debug.Log(card + "Destroyed");
            }
        }
        else if (type == "Played")
        {
            if (CardsPlayed == 5)
            {
                CardsPlayed = 0;
            }
            if (isOwned)
            {
                card.transform.SetParent(PlayerSlot.transform, false); //Make sure its the right dropzone variable
                CmdGMCardPlayed();
                card.GetComponent<CardDetails>().inHand = false;
                card.GetComponent<CardDetails>().CardUnHover();
                GameManager.PlayerHandSize --;
            }
            if(!isOwned)
            {
                card.transform.SetParent(EnemySlot.transform, false); //Make sure its the right dropzone variable
                card.GetComponent<CardFlipper>().Flip();
                GameManager.EnemyHandSize --;
            }
            card.GetComponent<CardDetails>().AttackTurn(false);
            CardsPlayed++;
        }
        else
        {
            Debug.Log("ASJKASJKDNAJISBNDASBDJ");
        }

        CmdUpdateAllCardText();
    }

    [Command]
    public void CmdRemoveConnectionUI()
    {
        RpcRemoveConnectionUI();
    }

    [ClientRpc]
    public void RpcRemoveConnectionUI()
    {
        GameObject ConnectionScreenUI = GameObject.Find("ConnectionScreenUI");
        Destroy(ConnectionScreenUI);
    }

    [Command]
    public void CmdShowDestoryDisplay(string state)
    {
        RpcShowDestroyDisplay(state);
    }

    [ClientRpc]
    public void RpcShowDestroyDisplay(string state)
    {
        if(isOwned)
        {
            foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.tag == "Cards")
                {
                    EnemyPlayedCards.Add(child.gameObject);
                }
            }

            if (!DestroyBeingMade && state == "OpenDisplay" && EnemyPlayedCards.Count > 0)
            {
                Debug.Log("OpenedDisplay");
                DestroyDisplayOpen = true;
                if(EnemyPlayedCards.Count > 0)
                {
                    foreach (GameObject card in EnemyPlayedCards)
                    {
                        // card.GetComponent<CardDetails>().CardHighlight("red", true);
                        CmdCardHighlight("red", true, card);
                    }
                }
                else
                {
                    Debug.Log("EnemyPlayedCards is empty");
                }

                DestroyBeingMade = true;
                AttackingTarget.GetComponent<CardDetails>().CardAttackHighlightOn();
            }
            else if (state == "CloseDisplay")
            {
                foreach (GameObject card in EnemyPlayedCards)
                {
                    // card.GetComponent<CardDetails>().CardHighlight("red", false);
                    CmdCardHighlight("red", false, card);
                }
                
                Debug.Log("ClosedDisplay");
                Debug.Log(AttackingTarget);
                
                // if(AttackingTarget != null)
                // {
                //     AttackingTarget.GetComponent<CardDetails>().CardAttackHighlightOff();
                // }
    
                AttackBeingMade = false;
                DestroyBeingMade = false;
                AttackDisplayOpened = false;
                AttackingTarget = null;
                AttackedTarget = null;
            }
            else
            {
                AttackBeingMade = false;
                DestroyBeingMade = false;
                AttackDisplayOpened = false;
                AttackingTarget = null;
                AttackedTarget = null;
                Debug.Log("Did not do anythin - Display");
            }
            EnemyPlayedCards.Clear();
        }
        else
        {
            Debug.Log("NODISPLAY");
        }
    }

    [Command]
    public void CmdShowAttackDisplay(string state)
    {
        RpcShowAttackDisplay(state);
    }

    [ClientRpc]
    public void RpcShowAttackDisplay(string state)
    {
        if(isOwned)
        {
           
            foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.tag == "Cards")
                {
                    EnemyPlayedCards.Add(child.gameObject);
                }
            }
            if (!AttackBeingMade && state == "OpenDisplay" && EnemyPlayedCards.Count > 0)
            {
                Debug.Log("OpenedDisplay");
                AttackDisplayOpened = true;
                if(EnemyPlayedCards.Count > 0)
                {
                    foreach (GameObject card in EnemyPlayedCards)
                    {
                        // card.GetComponent<CardDetails>().CardHighlight("red", true);
                        CmdCardHighlight("red", true, card);
                    }
                }
                else
                {
                    Debug.Log("EnemyPlayedCards is empty");
                }

                

                CmdHighlightHero("red", true, false);
                AttackBeingMade = true;
                AttackingTarget.GetComponent<CardDetails>().CardAttackHighlightOn();
            }
            else if (EnemyPlayedCards.Count == 0 && state == "OpenDisplay") // For attacking, if no minions - you have the option to attack enemy hero.
            {
                CmdHighlightHero("red", true, false);
                AttackDisplayOpened = true;
                AttackBeingMade = true;
                AttackingTarget.GetComponent<CardDetails>().CardAttackHighlightOn();
            }
            else if (state == "CloseDisplay")
            {
                foreach (GameObject card in EnemyPlayedCards)
                {
                    // card.GetComponent<CardDetails>().CardHighlight("red", false);
                    CmdCardHighlight("red", false, card);
                    card.transform.SetParent(EnemySlot.transform, false);
                }
                if(UIManager.EnemyHighlight.sprite == UIManager.RedPlayerHighlight || UIManager.EnemyHighlight.sprite == UIManager.PurplePlayerHighlight)
                {
                    CmdHighlightHero("red", false, false);
                }
                
                Debug.Log("ClosedDisplay");
                Debug.Log(AttackingTarget);
                
                // if(AttackingTarget != null)
                // {
                //     AttackingTarget.GetComponent<CardDetails>().CardAttackHighlightOff();
                // }
    
                AttackBeingMade = false;
                DestroyBeingMade = false;
                AttackDisplayOpened = false;
                AttackingTarget = null;
                AttackedTarget = null;
            }
            else
            {
                Debug.Log("Did not do anythin - Display");
            }
            EnemyPlayedCards.Clear();
        }
        else
        {
            Debug.Log("Error! RpcShowAttackDisplay");
        }
    }

    [Command]
    public void CmdEndTurn()
    {
        RpcEndTurn();
    }

    [ClientRpc]
    public void RpcEndTurn()
    {
        if(isOwned)
        {
            foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    child.GetComponent<CardAbilities>().OnEndTurn();
                }
            }
            GameObject HeroPowerButton = GameObject.Find("PlayerHeroPower");
            HeroPowerButton.GetComponent<HeroDetails>().HeroPowerActive();
        }

        CmdShowAttackDisplay("CloseDisplay");
        PlayerManager pm = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        pm.IsMyTurn = !pm.IsMyTurn;
        GameManager.EndTurn();
        UIManager.DisplayTurnDisplay();

        if(IsMyTurn && isOwned) 
        {
            foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    child.GetComponent<CardAbilities>().OnStartTurn();
                }
            }
        }
    }

    [Command]
    public void CmdResetAttackTurns()
    {
        RpcResetAttackTurns();
    }

    [ClientRpc]
    public void RpcResetAttackTurns()
    {
        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                child.GetComponent<CardDetails>().AttackTurn(true);
            }
        }

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                child.GetComponent<CardDetails>().AttackTurn(true);
            }
        }

    }

    [Command]
    void CmdGMCardPlayed()
    {
        RpcGMCardPlayed();
    }

    [ClientRpc]
    void RpcGMCardPlayed()
    {
        GameManager.CardPlayed();
    }

    [Command]
    public void CmdPlayerReadyUp()
    {
        SoundManager.PlayDeckFX();
        RpcPlayerReadyUp();
    }

    [ClientRpc]
    public void RpcPlayerReadyUp()
    {
        if(isOwned)
        {
            GameManager.PlayerReady = true;
        }
        else
        {
            GameManager.EnemyReady = true;
        }
    }

    [Command]
    public void CmdAmountOfPlayers()
    {

    }

    [Command]
    public void CmdDeckSelection(string DeckTag)
    {
        RpcDeckSelection(DeckTag);
    }

    [ClientRpc]
    public void RpcDeckSelection(string DeckTag)
    {
        string SelectedDeck = DeckTag;
        GameObject PlayerImage = GameObject.Find("PlayerImage");
        GameObject EnemyImage = GameObject.Find("EnemyImage");

        Debug.Log("RPCSelectedDeck: " + SelectedDeck);

        if (isOwned)
        {
            GameManager.PlayerDeck = SelectedDeck;
            if(SelectedDeck == "Mark")
            {
                PlayerImage.GetComponent<Image>().sprite = UIManager.MarkImage;
                GameManager.PlayerDeckSize = MarkDeck.Count;
            }
            else if (SelectedDeck == "Keagan")
            {
                PlayerImage.GetComponent<Image>().sprite = UIManager.KeaganImage;
                GameManager.PlayerDeckSize = KeaganDeck.Count;
            }
            else if (SelectedDeck == "Deion")
            {
                PlayerImage.GetComponent<Image>().sprite = UIManager.DeionImage;
                GameManager.PlayerDeckSize = DeionDeck.Count;
            }
            else if (SelectedDeck == "Chris")
            {
                PlayerImage.GetComponent<Image>().sprite = UIManager.ChrisImage;
                GameManager.PlayerDeckSize = ChrisDeck.Count;
            }
        }
        else
        {
            GameManager.EnemyDeck = SelectedDeck;
            if(SelectedDeck == "Mark")
            {
                EnemyImage.GetComponent<Image>().sprite = UIManager.MarkImage;
                GameManager.EnemyDeckSize = MarkDeck.Count;
            }
            else if (SelectedDeck == "Keagan")
            {
                EnemyImage.GetComponent<Image>().sprite = UIManager.KeaganImage;
                GameManager.EnemyDeckSize = KeaganDeck.Count;
            }
            else if (SelectedDeck == "Deion")
            {
                EnemyImage.GetComponent<Image>().sprite = UIManager.DeionImage;
                GameManager.EnemyDeckSize = DeionDeck.Count;
            }
            else if (SelectedDeck == "Chris")
            {
                EnemyImage.GetComponent<Image>().sprite = UIManager.ChrisImage;
                GameManager.EnemyDeckSize = ChrisDeck.Count;
            }
        }
    }

    //CARD ABILITIES
    [Command]
    public void CmdDealDamage(GameObject Target, int Damage)
    {
        if(Target != null)
        {
            RpcDealDamage(Target, Damage);
        }
    }

    [ClientRpc]
    public void RpcDealDamage(GameObject Target, int Damage)
    {
        if(Target == null) {return;}
        Target.GetComponent<CardDetails>().SetCardHealth(Damage);
        // if(0 > Damage)
        // {
        //     CmdCardDamageEffect(Target);
        // }

        int TargetHealth = Target.GetComponent<CardDetails>().GetCardHealth();
        CmdUpdateAllCardText();
        if(isOwned)
        {
            Target.GetComponent<CardAbilities>().OnHit(); 
        }    
        
        if(TargetHealth < 1)
        {
            SoundManager.PlayHurtFX();
            Debug.Log("RPCDealDamage() : " + Target);
            CmdDeathAnimation(Target);
        }
    }

    [Command]
    public void CmdDestroyTarget(GameObject Target)
    {
        if(Target == null) {return;}
        RpcDestoryTarget(Target);
    }

    [ClientRpc]
    public void RpcDestoryTarget(GameObject Target)
    {
        if(Target == null) {return;}
        CmdDeathAnimation(Target);
    }

    [Command]
    public void CmdDestoryDeckSelectionUI()
    {
        RpcDestroyDeckSelectionUI();
    }

    [ClientRpc]
    public void RpcDestroyDeckSelectionUI()
    {
        Destroy(GameObject.Find("DeckSelectionUI"));
        GameManager.HighLightStart();
    }

    [Command]
    public void CmdAttackingDetails(GameObject target, int targetNum)
    {
        if(target == null) {return;}
        RpcAttackingDetails(target, targetNum);
    }

    [ClientRpc]
    public void RpcAttackingDetails(GameObject target, int targetNum)
    {
        if(target == null) {return;}
        if(targetNum == 1)
        {
            AttackedTarget = target;
            Debug.Log("Attacked Target: " + AttackedTarget);
        }
        else if(targetNum == 0)
        {
            AttackingTarget = target;
            Debug.Log("Attacking Target: " + AttackingTarget);
        }
        
        AttackingTarget.GetComponent<CardDetails>().DealAttack();
    }

    [Command]
    public void CmdPermSetCardHealth(GameObject card, int newHealth)
    {
        if(card == null) {return;}
        RpcPermSetCardHealth(card, newHealth);
    }

    [ClientRpc]
    public void RpcPermSetCardHealth(GameObject card, int newHealth)
    {
        if(card == null) {return;}
        int currentCardHealth = card.GetComponent<CardDetails>().GetCardHealth();
        int MaxCardHealth = card.GetComponent<CardDetails>().GetMaxCardHealth();

        card.GetComponent<CardDetails>().PermSetCardHealth(newHealth);
    }
   
    [Command]
    public void CmdHighlightHero(string color, bool On, bool forPlayer)
    {
        RpcHighlightHero(color, On, forPlayer);
    }

    [ClientRpc]
    public void RpcHighlightHero(string color, bool On, bool forPlayer)
    {
        Image TargetImage;
        if(isOwned)
        {
            if(forPlayer)
            {
                TargetImage = UIManager.PlayerHighlight;
            }
            else
            {
                TargetImage = UIManager.EnemyHighlight;
            }
        }
        else
        {
            if(forPlayer)
            {
                TargetImage = UIManager.EnemyHighlight;
            }
            else
            {
                TargetImage = UIManager.PlayerHighlight;
            }
        }
        

        if(On)
        {
            if(color == "turn")
            {
                TargetImage.sprite = UIManager.PlayerTurnHighlight;
            }
            else if(color == "blue")
            {
                TargetImage.sprite = UIManager.BluePlayerHighlight;
            }
            else if (color == "red")
            {
                TargetImage.sprite = UIManager.RedPlayerHighlight;
            }
            else if (color == "green")
            {
                // TargetImage.sprite = UIManager.Gree;
            }
            else if(color == "grey")
            {
                // TargetImage.sprite = UIManager.RedPlayerHighlight;
            }
            else if(color == "purple")
            {
                TargetImage.sprite = UIManager.PurplePlayerHighlight;
            }
            else
            {
                TargetImage.sprite = UIManager.NoPlayerHighlight;
            }
        }
        else
        {
            TargetImage.sprite = TargetImage.sprite = UIManager.NoPlayerHighlight;
        }
    }

    [Command]
    public void CmdCardHighlight(string color, bool On, GameObject card)
    {
        if(card == null) {return;}
        RpcCardHighlight(color, On, card);
    }

    [ClientRpc]
    public void RpcCardHighlight(string color, bool On, GameObject card)
    {   

        if(card != null)
        {
            CardDetails cardDetails = card.GetComponent<CardDetails>();
            if(cardDetails.cardHighlightImage.sprite == cardDetails.DamagedHighlight || cardDetails.cardHighlightImage.sprite == cardDetails.DestroyHighlight )
            {
                return;
            }
            if(On)
            {
                cardDetails.cardHighlightImage.enabled = true;
                if(color == "blue")
                {
                    cardDetails.cardHighlightImage.sprite = cardDetails.BlueHighlight;
                }
                else if (color == "red")
                {
                    cardDetails.cardHighlightImage.sprite = cardDetails.RedHightlight;
                }
                else if (color == "green")
                {
                    cardDetails.cardHighlightImage.sprite = cardDetails.GreenHighlight;
                }
                else if(color == "grey")
                {
                    cardDetails.cardHighlightImage.sprite = cardDetails.GreyHighlight;
                }
                else if(color == "purple")
                {
                    cardDetails.cardHighlightImage.sprite = cardDetails.PurpleHighlight;
                }
                else if(color == "damaged")
                {
                    cardDetails.cardHighlightImage.sprite = cardDetails.DamagedHighlight;
                }
                else if(color == "destroy")
                {
                    cardDetails.cardHighlightImage.sprite = cardDetails.DestroyHighlight;
                }
            }
            else
            {
                cardDetails.cardHighlightImage.sprite = cardDetails.NoHightlight;
                cardDetails.cardHighlightImage.enabled = false;
            }
        }        
    }

    [Command]
    public void CmdCardAttack()
    {
        RpcCardAttack();
    }


    [ClientRpc]
    public void RpcCardAttack()
    {
        int PlayerAttackDamage;
        int PlayerCardHealth;

        int EnemyCardHealth;
        int EnemyAttackDamage;

        PlayerAttackDamage = AttackingTarget.GetComponent<CardDetails>().GetCardAttack();
        PlayerCardHealth = AttackingTarget.GetComponent<CardDetails>().GetCardHealth();

        if(AttackedTarget.gameObject.tag == "Cards")
        {          
            EnemyAttackDamage = AttackedTarget.GetComponent<CardDetails>().GetCardAttack();
            EnemyCardHealth = AttackedTarget.GetComponent<CardDetails>().GetCardHealth();

            PlayerCardHealth -= EnemyAttackDamage;
            EnemyCardHealth -= PlayerAttackDamage;

            AttackingTarget.GetComponent<CardAbilities>().OnHit();
            SoundManager.PlayHurtFX();
            AttackedTarget.GetComponent<CardAbilities>().OnHit();
            SoundManager.PlayHurtFX();
            

            AttackingTarget.GetComponent<CardDetails>().AttackTurn(false);
            AttackedTarget.GetComponent<CardDetails>().SetCardHealth(-PlayerAttackDamage);
            AttackingTarget.GetComponent<CardDetails>().SetCardHealth(-EnemyAttackDamage);

            CmdUpdateAllCardText();

            if(EnemyCardHealth < 1)
            {
                CmdDeathAnimation(AttackedTarget);
            }
            else
            {
                CmdCardDamageEffect(AttackedTarget);
            }
            if(PlayerCardHealth < 1)
            {
                CmdDeathAnimation(AttackingTarget);
            }
            else
            {
                CmdCardDamageEffect(AttackingTarget);
            }
            
        }
        else
        {
            SoundManager.PlayHurtFX();
            CmdGMEnemyHealth(-PlayerAttackDamage);
            AttackingTarget.GetComponent<CardDetails>().AttackTurn(false);
        }

        
    }

    [Command]
    public void CmdCardSpecial(GameObject card)
    {
        if(card == null) {return;}
        RpcCardSpecial(card);
    }

    [ClientRpc]
    void RpcCardSpecial(GameObject card)
    {
        // card.GetComponent<CardAbilities>().OnSpec();
    }

    [Command]
    public void CmdUpdateDoubloons(int amount, bool stealing)
    {
        RpcUpdateDoubloons(amount, stealing);
    }

    [ClientRpc]
    public void RpcUpdateDoubloons(int amount, bool stealing)
    {
        CmdHeroItemAnimation(true, "doubloon");
        if(stealing)
        {
            SoundManager.PlayDoubloonFX();
        }
        GameManager.UpdateDoubloons(amount, isOwned, stealing);
    }

    [Command]
    public void CmdCardStatChange(int attack, int health, GameObject card)
    {
        if(card == null) {return;}
        RpcCardStatChange(attack, health, card);
    }

    [ClientRpc]
    public void RpcCardStatChange(int attack, int health, GameObject card)
    {
        if(card != null)
        {
            int CardHealth = card.GetComponent<CardDetails>().GetCardHealth();
            
            if(CardHealth < 1)
            {
                CmdDeathAnimation(card);
            }

            card.GetComponent<CardDetails>().SetCardHealth(health);
            card.GetComponent<CardDetails>().ChangeCardAttack(attack);
            CmdCardEffect(card);
            CmdUpdateAllCardText();
        }
    }

    [Command]
    public void CmdSetCardStats(int attack, int health, GameObject card)
    {
        if(card == null) {return;}
        RpcSetCardStats(attack,health,card);
    }

    [ClientRpc]
    public void RpcSetCardStats(int attack, int health, GameObject card)
    {
        if(card != null)
        {
            card.GetComponent<CardDetails>().MaxCardAttack = attack;
            card.GetComponent<CardDetails>().MaxCardHealth = health;
            card.GetComponent<CardDetails>().CurrentCardAttack = attack;
            card.GetComponent<CardDetails>().CurrentCardHealth = health;
            CmdCardEffect(card);
        }
    }

    
    [Command]
    public void CmdSetPlayerHealth(int health)
    {
        RpcSetPlayerHealth(health);
    }

    [ClientRpc]
    public void RpcSetPlayerHealth(int health)
    {
        GameManager.SetPlayerHealth(health, isOwned);
    }

    [Command]
    public void CmdGMPlayerHealth(int health)
    {
        RpcGMPlayerHealth(health);
    }

    [ClientRpc]
    public void RpcGMPlayerHealth(int health)
    {
        if(0 > health)
        {
            SoundManager.PlayHurtFX();
        }
        CmdHeroItemAnimation(true, "health");
        GameManager.AdjustPlayerHealth(health, isOwned);
    }

    [Command]
    public void CmdGMEnemyHealth(int health)
    {
        RpcGMPEnemyHealth(health);
    }

    [ClientRpc]
    public void RpcGMPEnemyHealth(int health)
    {
        if(0 > health)
        {
            SoundManager.PlayHurtFX();
        }
        CmdHeroItemAnimation(false, "health");
        GameManager.AdjustEnemyHealth(health, isOwned);
    }

    [Command]
    public void CmdUpdateAllCardText()
    {
        RpcUpdateAllCardText();
    }

    [ClientRpc]
    public void RpcUpdateAllCardText()
    {
        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                child.GetComponent<CardDetails>().UpdateCardText();
            }
        }

        foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                child.GetComponent<CardDetails>().UpdateCardText();
            }
        }
    }

    [Server]
    public void GiveOtherPlayerAuthority(GameObject card)
    { 
        if(card == null) {return;}
        NetworkIdentity objNetworkIdentity = card.GetComponent<NetworkIdentity>();

        if (objNetworkIdentity.isOwned)
        {
            // Get all connections (players) on the server
            foreach (var connection in NetworkServer.connections)
            {
                // Skip the server connection (connectionId = 0)
                if (connection.Key == 0)
                    continue;

                // Skip the current owner's connection
                if (connection.Value == objNetworkIdentity.connectionToClient)
                    continue;

                // Assign authority to the next available player
                objNetworkIdentity.RemoveClientAuthority();
                objNetworkIdentity.AssignClientAuthority(connection.Value);
                
                Debug.Log($"Authority transferred from {connectionToClient} to {connection.Value}");
                return; // Authority transferred, exit the loop
            }

            Debug.Log("No other players to transfer authority to.");
        }
        else
        {
            Debug.Log("The GameObject doesn't have authority to transfer.");
        }
    }

    [Command]
    public void CmdSummonMinion(int health, int attack, bool forPlayer)
    {
        GameObject card = Instantiate(cards[0], new Vector3(0,0,0), Quaternion.identity);
        NetworkServer.Spawn(card, connectionToClient);
        RpcSummonMinion(health, attack, forPlayer, card);
    }

    [ClientRpc]
    public void RpcSummonMinion(int health, int attack, bool forPlayer, GameObject card)
    {
        if(card == null) {return;}
        if(!forPlayer)
        {
            GiveOtherPlayerAuthority(card);
        }
        
        card.GetComponent<CardDetails>().MaxCardHealth = health;
        card.GetComponent<CardDetails>().MaxCardAttack = attack;
        card.GetComponent<CardDetails>().CurrentCardHealth = health;
        card.GetComponent<CardDetails>().CurrentCardAttack = attack;
        card.GetComponent<CardDetails>().CanAttack = false;
        CmdCardSleep(false, card);
        
        CmdUpdateAllCardText();

        if(forPlayer)
        {
            if(isOwned)
            {
                if(AbleToPlaceCard(PlayerSlot))
                {
                    card.transform.SetParent(PlayerSlot.transform, false);
                }
            }
            else
            {
                if(AbleToPlaceCard(EnemySlot))
                {
                    card.transform.SetParent(EnemySlot.transform, false);
                }
            }  
        }
        else
        {
            if(isOwned)
            {
                if(AbleToPlaceCard(EnemySlot))
                {
                    card.transform.SetParent(EnemySlot.transform, false);
                }
            }
            else
            {
                if(AbleToPlaceCard(PlayerSlot))
                {
                    card.transform.SetParent(PlayerSlot.transform, false);
                }
            }  
        }
        

    }

    public bool AbleToPlaceCard(GameObject Slot)
    {        
        int count = 0;
        if(Slot.gameObject.name == "EnemySlot")
        {
            foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    count++;
                }
            }
        }
        else if (Slot.gameObject.name == "PlayerSlot")
        {
            foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    count++;
                }
            }
        }

        if(count < 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DiscardCards(int amount, bool forPlayer)
    {
        if(isOwned)
        {
            for (int i = 0; i < amount; i ++)
            {
                int ranNum = Random.Range(0,PlayerArea.transform.childCount);
                int count = 0;

                foreach (Transform child in PlayerArea.GetComponentInChildren<Transform>())
                {
                    if (child.gameObject.tag == "Cards")
                    {
                        if(count == ranNum)
                        {
                            CmdDiscardCards(child.gameObject);
                        }
                        count++;
                    }
                }
            }
        }
    }

    [Command]
    public void CmdDiscardCards(GameObject card)
    {
        if(card == null) {return;}
        RpcDiscardCards(card);
    }

    [ClientRpc]
    public void RpcDiscardCards(GameObject card)
    {
        if(card == null) {return;}
        Destroy(card);
    }


    //ANIMATIONS

    [Command]
    public void CmdHeroItemAnimation(bool forPlayer, string item)
    {
        RpcHeroItemAnimation(forPlayer, item);
    }

    [ClientRpc]
    public void RpcHeroItemAnimation(bool forPlayer, string item)
    {
        GameObject TargetImage = null;
        float animationDuration = 0.3f;

        if(isOwned)
        {
            if(forPlayer)
            {
                if(item == "health")
                {
                    TargetImage = GameObject.Find("PlayerHealthImage");
                }
                else if(item == "doubloon")
                {
                    TargetImage = GameObject.Find("PlayerDoubloonImage");
                }
            }
            else
            {
                if(item == "health")
                {
                    TargetImage = GameObject.Find("EnemyHealthImage");
                }
                else if(item == "doubloon")
                {
                    TargetImage = GameObject.Find("EnemyDoubloonImage");
                }
            }
        }
        else
        {
            if(forPlayer)
            {
                if(item == "health")
                {
                    TargetImage = GameObject.Find("EnemyHealthImage");
                }
                else if(item == "doubloon")
                {
                    TargetImage = GameObject.Find("EnemyDoubloonImage");
                }
            }
            else
            {
                if(item == "health")
                {
                    TargetImage = GameObject.Find("PlayerHealthImage");
                }
                else if(item == "doubloon")
                {
                    TargetImage = GameObject.Find("PlayerDoubloonImage");
                }
            }
        }

        if(TargetImage != null)
        {
            LeanTween.scale(TargetImage, new Vector3(1.2f, 1.2f, 1f), animationDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                // After the scale-up animation, start the scale-down animation
                LeanTween.scale(TargetImage, new Vector3(1f, 1f, 1f), animationDuration)
                    .setEase(LeanTweenType.easeOutQuad);
            });
        }
        else
        {
            Debug.Log(TargetImage);
        }
    }

    // [Command]
    // public void CmdTurnDisplay()
    // {
    //     RpcTurnDisplay();
    // }

    [Command]
    public void CmdCardDamageEffect(GameObject card)
    {
        RpcCardDamageEffect(card);
    }

    [ClientRpc]
    public void RpcCardDamageEffect(GameObject card)
    {
        Debug.Log(card);
        if(card == null){return;}
        
        Sprite currentHighlight = card.GetComponent<CardDetails>().cardHighlightImage.sprite;
        // CmdCardHighlight("damaged", true, card);
        Debug.Log(card + " DAMAGED!");

        StartCoroutine(DisplayDamagedCard());

        IEnumerator DisplayDamagedCard()
        {
            // Activate the GameObject
            CmdCardHighlight("damaged", true, card);

            // Wait for 3 seconds
            yield return new WaitForSeconds(1f);
            
            if(card != null)
            {
                card.GetComponent<CardDetails>().cardHighlightImage.sprite = card.GetComponent<CardDetails>().BlueHighlight;
            }
            // Deactivate the GameObject after 3 seconds
        }
    }

    [Command]
    public void CmdCardEffect(GameObject card)
    {
        RpcCardEffect(card);
    }

    [ClientRpc]
    public void RpcCardEffect(GameObject card)
    {
        float animationDuration = 0.3f;
        CmdCardHighlight("green", true, card);
        LeanTween.scale(card, new Vector3(1.2f, 1.2f, 1f), animationDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                // After the scale-up animation, start the scale-down animation
                LeanTween.scale(card, new Vector3(1f, 1f, 1f), animationDuration)
                    .setEase(LeanTweenType.easeOutQuad);
            });
        CmdCardHighlight("noeffect", false, card);
    }

    [Command]
    public void CmdDeathAnimation(GameObject card)
    {
        RpcDeathAnimation(card);
    }

    [ClientRpc]
    public void RpcDeathAnimation(GameObject card)
    {
        float animationDuration = 0.3f;
        CmdCardHighlight("destroy", true, card);       

        StartCoroutine(DeathAnimation(card));

        IEnumerator DeathAnimation(GameObject card)
        {
            LeanTween.scale(card, new Vector3(1.2f, 1.2f, 1f), animationDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                // After the scale-up animation, start the scale-down animation
                LeanTween.scale(card, new Vector3(1f, 1f, 1f), animationDuration)
                    .setEase(LeanTweenType.easeOutQuad);
            });

            yield return new WaitForSeconds(1f);
            
            card.GetComponent<CardZoom>().CardDead = true;
            card.GetComponent<CardAbilities>().OnLastResort();
            SoundManager.PlayDeathFX();
            
            yield return new WaitForSeconds(0.1f);
            card.GetComponent<CardZoom>().OnHoverExit();
            Destroy(card);
        }
    }

    // [Command]
    // public void CmdSummonAnimation(GameObject card)
    // {
    //     RpcSummonAnimation(card);
    // }

    // [ClientRpc]
    // public void RpcSummonAnimation(GameObject card)
    // {
        
    // }

    //ANIMATIONS
}
