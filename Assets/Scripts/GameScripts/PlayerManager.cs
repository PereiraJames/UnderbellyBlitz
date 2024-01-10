using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

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
    public GameObject AttackingDisplay;

    private int currentPlayerCount = 0;


    public NetworkManager NetworkManager;

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

    public override void OnStartClient()
    {
        base.OnStartClient();

        DrawCards = GameObject.Find("Button").GetComponent<DrawCards>();

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

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

        AttackingDisplay = GameObject.Find("AttackingDisplay");
        

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
        Debug.Log("CardDealt");
        for (int i = 0; i < cardAmount; i++)
        {
            List<GameObject> deck = CmdWhichDeck(deckName);
            if(deck.Count != 0)
            {
                int ranNum = Random.Range(0,deck.Count - 1);
                GameObject card = Instantiate(deck[ranNum], new Vector3(0,0,0), Quaternion.identity);
                NetworkServer.Spawn(card, connectionToClient);
                if (isOwned && GameManager.PlayerHandSize + 1 < handLimit)
                {
                    RpcSetParentCard(card);
                    deck.RemoveAt(ranNum);
                }
                else if (!isOwned && GameManager.PlayerHandSize + 1 < handLimit)
                {
                    RpcSetParentCard(card);
                    deck.RemoveAt(ranNum);
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
        if(isOwned)
        {
            card.transform.SetParent(PlayerArea.transform, false);
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
                GameManager.PlayerHandSize --;
            }
            if(!isOwned)
            {
                card.transform.SetParent(EnemySlot.transform, false); //Make sure its the right dropzone variable
                card.GetComponent<CardFlipper>().Flip();
                GameManager.EnemyHandSize --;
            }
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
                        card.transform.SetParent(AttackingDisplay.transform, false);
                    }
                }
                else
                {
                    Debug.Log("EnemyPlayedCards is empty");
                }
                AttackBeingMade = true;
            }
            else if (state == "CloseDisplay")
            {
                foreach (Transform child in AttackingDisplay.GetComponentsInChildren<Transform>())
                {
                    if(child.gameObject.tag == "Cards")
                    {
                        EnemyPlayedCards.Add(child.gameObject);
                    }
                }
                foreach (GameObject card in EnemyPlayedCards)
                {
                    card.transform.SetParent(EnemySlot.transform, false);
                }
                Debug.Log("ClosedDisplay");
                AttackBeingMade = false;
                DestroyBeingMade = false;
                AttackDisplayOpened = false;

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
        }

        CmdShowAttackDisplay("CloseDisplay");
        PlayerManager pm = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        pm.IsMyTurn = !pm.IsMyTurn;
        GameManager.EndTurn();
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
        RpcDealDamage(Target, Damage);
    }

    [ClientRpc]
    public void RpcDealDamage(GameObject Target, int Damage)
    {
        Target.GetComponent<CardDetails>().SetCardHealth(Damage);

        int TargetHealth = Target.GetComponent<CardDetails>().GetCardHealth();
        CmdUpdateAllCardText();
        if(isOwned)
        {
            Target.GetComponent<CardAbilities>().OnHit(); 
        }    
        
        if(TargetHealth < 1)
        {
            Target.GetComponent<CardZoom>().OnHoverExit();
            Debug.Log("RPCDealDamage() : " + Target);
            Target.GetComponent<CardAbilities>().OnLastResort();
            Destroy(Target);
        }
    }

    [Command]
    public void CmdDestroyTarget(GameObject Target)
    {
        RpcDestoryTarget(Target);
    }

    [ClientRpc]
    public void RpcDestoryTarget(GameObject Target)
    {
        Target.GetComponent<CardZoom>().OnHoverExit();
        Debug.Log("RPCDestoryTarget: " + Target);
        Target.GetComponent<CardAbilities>().OnLastResort();
        Destroy(Target);
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
        RpcAttackingDetails(target, targetNum);
    }

    [ClientRpc]
    public void RpcAttackingDetails(GameObject target, int targetNum)
    {
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
        RpcPermSetCardHealth(card, newHealth);
    }

    [ClientRpc]
    public void RpcPermSetCardHealth(GameObject card, int newHealth)
    {
        card.GetComponent<CardDetails>().PermSetCardHealth(newHealth);
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

        EnemyAttackDamage = AttackedTarget.GetComponent<CardDetails>().GetCardAttack();
        EnemyCardHealth = AttackedTarget.GetComponent<CardDetails>().GetCardHealth();

        PlayerCardHealth -= EnemyAttackDamage;
        EnemyCardHealth -= PlayerAttackDamage;
        AttackedTarget.GetComponent<CardDetails>().SetCardHealth(EnemyCardHealth);
        AttackingTarget.GetComponent<CardDetails>().AttackTurn(false);

        AttackingTarget.GetComponent<CardDetails>().SetCardHealth(PlayerCardHealth);

        AttackingTarget.GetComponent<CardAbilities>().OnHit();
        AttackedTarget.GetComponent<CardAbilities>().OnHit();

        if(EnemyCardHealth < 1)
        {
            AttackedTarget.GetComponent<CardZoom>().OnHoverExit();
            Debug.Log("RPCardAttack(): " + gameObject);
            AttackedTarget.GetComponent<CardAbilities>().OnLastResort();
            Destroy(AttackedTarget);
        }
        if(PlayerCardHealth < 1)
        {
            AttackingTarget.GetComponent<CardZoom>().OnHoverExit();
            Debug.Log("RPCardAttack(): " + gameObject);
            AttackingTarget.GetComponent<CardAbilities>().OnLastResort();
            Destroy(AttackingTarget);
        }        
        
        CmdUpdateAllCardText();

        AttackingTarget = null;
        AttackedTarget = null;
    }

    [Command]
    public void CmdCardSpecial(GameObject card)
    {
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
        GameManager.UpdateDoubloons(amount, isOwned, stealing);
    }

    [Command]
    public void CmdChangeBP(int playerBp, int enemyBp)
    {
        RpcChangeBP(playerBp, enemyBp);
    }

    [ClientRpc]
    public void RpcChangeBP(int playerBp, int enemyBp)
    {
        GameManager.ChangeBP(playerBp, enemyBp, isOwned);
    }

    [Command]
    public void CmdCardStatChange(int attack, int health, GameObject card)
    {
        RpcCardStatChange(attack, health, card);
    }

    [ClientRpc]
    public void RpcCardStatChange(int attack, int health, GameObject card)
    {
        card.GetComponent<CardDetails>().SetCardHealth(health);
        card.GetComponent<CardDetails>().ChangeCardAttack(attack);

        int CardHealth = card.GetComponent<CardDetails>().GetCardHealth();
        CmdUpdateAllCardText();

        if(CardHealth < 1)
        {
            card.GetComponent<CardZoom>().OnHoverExit();
            card.GetComponent<CardAbilities>().OnLastResort();
            Destroy(card);
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
        card.GetComponent<CardDetails>().CardHealth = health;
        card.GetComponent<CardDetails>().CardAttack = attack;
        CmdUpdateAllCardText();
        if(isOwned)
        {
            if(forPlayer)
            {
                card.transform.SetParent(PlayerSlot.transform, false);
                Debug.Log("A");
            }
            else
            {
                card.transform.SetParent(EnemySlot.transform, false);
                Debug.Log("B");
            }
        }
        else
        {
            if(forPlayer)
            {
                card.transform.SetParent(EnemySlot.transform, false);
                Debug.Log("C");

            }
            else
            {
                card.transform.SetParent(PlayerSlot.transform, false);
                Debug.Log("D");
            }
        }
    }

    public void DiscardCards(int amount, bool forPlayer)
    {
        if(isOwned)
        {
            for (int i = 0; i < amount; i ++)
            {
                Debug.Log(PlayerArea.transform.childCount);
                int ranNum = Random.Range(0,PlayerArea.transform.childCount);
                int count = 0;

                foreach (Transform child in PlayerArea.GetComponentInChildren<Transform>())
                {
                    if (child.gameObject.tag == "Cards")
                    {
                        if(count == ranNum)
                        {
                            Debug.Log(child.gameObject);
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
        RpcDiscardCards(card);
    }

    [ClientRpc]
    public void RpcDiscardCards(GameObject card)
    {
        Destroy(card);
    }
}
