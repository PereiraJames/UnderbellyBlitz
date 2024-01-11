using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardDetails : NetworkBehaviour
{

    private GameManager GameManager;
    private GameObject Canvas;
    private PlayerManager PlayerManager;
    private RectTransform RectPlayerSlot;


    public GameObject EnemySlot;
    public GameObject PlayerSlot;
    public GameObject AttackingDisplay;
    public GameObject AttackingCard;
    
    public int MaxCardHealth = 1;
    public int MaxCardAttack = 1;

    public int CurrentCardHealth = 1;
    public int CurrentCardAttack = 1;
    public bool CanAttack = false;
    public int DoubloonCost = 1;
    public int amountOfEachCard = 1;

    public bool isFrozen = false;
    public bool isDamaged = false;

    public string DeckTag;

    public GameObject CardStats;

    void Start()
    {

        AttackingDisplay = GameObject.Find("AttackingDisplay");
        PlayerSlot = GameObject.Find("PlayerSlot");
        EnemySlot = GameObject.Find("EnemySlot");
        RectPlayerSlot = PlayerSlot.GetComponent<RectTransform>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Canvas = GameObject.Find("Main Canvas");
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        CurrentCardAttack = MaxCardAttack;
        CurrentCardHealth = MaxCardHealth;

        UpdateCardText();
    }

    public int GetCardHealth()
    {
        return CurrentCardHealth;
    }

    public int GetCardAttack()
    {
        return CurrentCardAttack;
    }

    public int GetMaxCardHealth()
    {
        return MaxCardHealth;
    }

    public int GetMaxCardAttack()
    {
        return MaxCardAttack;
    }

    public void SetCardHealth(int DamageDone)
    {
        CurrentCardHealth += DamageDone;
        UpdateCardText();
    }

    public void PermSetCardHealth(int newHealth)
    {
        int diffHealth = newHealth - MaxCardHealth;

        CurrentCardHealth += diffHealth;
        MaxCardHealth = newHealth;
        UpdateCardText();
    }

    public void SetCardAttack(int attackAmount)
    {
        CurrentCardAttack = attackAmount;
        UpdateCardText();
    }

    public void ChangeCardAttack(int attackAmount)
    {
        CurrentCardAttack += attackAmount;
        UpdateCardText();
    }

    public bool IsAbleToAttack()
    {
        return CanAttack;
    }

    public void AttackTurn(bool HasAttackedThisTurn)
    {
        CanAttack = HasAttackedThisTurn;
        Debug.Log(CanAttack);
        if(gameObject != null)
        {
            PlayerManager.CmdCardSleep(CanAttack, gameObject);
        }
        
    }

    public void UpdateCardText()
    {
        gameObject.GetComponentInChildren<Text>().text = CurrentCardAttack + " / " + CurrentCardHealth;
    }


    //CARD ATTACK START

    public void AttackTarget()
    {
        if (PlayerManager.IsMyTurn && isOwned && gameObject.GetComponent<CardDetails>().IsAbleToAttack()) //Must be on players turn, player must have authourity of card and the card must be able to attack
        {
            if (!PlayerManager.AttackBeingMade) //checks if attack is currently being made
            {   
                if(transform.parent == RectPlayerSlot) //This checks if card is in the PlayerSlot
                {
                    PlayerManager.CmdAttackingDetails(gameObject, 0);
                    PlayerManager.CmdShowAttackDisplay("OpenDisplay");
                }
            }
            else
            {
                PlayerManager.CmdShowAttackDisplay("CloseDisplay");
            }
        }
        else if(PlayerManager.IsMyTurn && PlayerManager.AttackDisplayOpened && isOwned) 
        {
            PlayerManager.CmdShowAttackDisplay("CloseDisplay");
        }
    }


    public void SelectedTarget() //In perspective of selected target
    {
        if(PlayerManager.AttackDisplayOpened)
        {
            if(PlayerManager.AttackBeingMade && PlayerManager.AttackingTarget != null && !isOwned)
            {
                if(gameObject != PlayerManager.AttackingTarget)
                {
                    PlayerManager.CmdAttackingDetails(gameObject, 1); 
                }
            }
            else if(PlayerManager.DestroyBeingMade && !isOwned)
            {
                Debug.Log("Destroying: " + gameObject);
                PlayerManager.CmdDestroyTarget(gameObject); 
                PlayerManager.CmdShowAttackDisplay("CloseDisplay");
                PlayerManager.DestroyBeingMade = false;
            }
            else
            {
                Debug.Log("DestroyBeingMade " + PlayerManager.DestroyBeingMade + " AttackBeingMade " + PlayerManager.AttackBeingMade);
            }
        }
    }

    public void DealAttack()
    {
        if(PlayerManager.AttackedTarget != null && PlayerManager.AttackBeingMade && gameObject.GetComponent<CardDetails>().IsAbleToAttack())
        {
            if(PlayerManager.IsMyTurn)
            {
                PlayerManager.CmdCardAttack();
                PlayerManager.CmdShowAttackDisplay("CloseDisplay");
            }
        }
    }

    public void DestroyTarget()
    {
        Debug.Log("Destroy");
        PlayerManager.DestroyBeingMade = true;
        PlayerManager.CmdShowAttackDisplay("OpenDisplay");
    }

    //CARD ATTACK END

    //CARD HIGHLIGHT START

    public bool isDrag()
    {
        return gameObject.GetComponent<DragDrop>().isDragging;
    }

    public void CardHover()
    {
        transform.localScale = transform.localScale + new Vector3(.3f, .3f, 0);
    }

    public void CardUnHover()
    {
        transform.localScale = transform.localScale + new Vector3(-.3f, -.3f, 0);
    }

    public void CardAttackHighlightOn()
    {
        if(PlayerManager.AttackingTarget == null)
        {
            gameObject.GetComponent<Outline>().effectColor = Color.blue;
            gameObject.GetComponent<Outline>().effectDistance = new Vector2(10,10);
        }
        else if(PlayerManager.AttackingTarget != null)
        {
            PlayerManager.AttackingTarget.GetComponent<CardDetails>().CardAttackHighlightOff();
            gameObject.GetComponent<Outline>().effectColor = Color.blue;
            gameObject.GetComponent<Outline>().effectDistance = new Vector2(10,10);
        }
        else
        {
            Debug.Log("ErrorCardAttackHighlight");
        }
        
    }

    public void CardAttackHighlightOff()
    {
        gameObject.GetComponent<Outline>().effectColor = Color.red;
        gameObject.GetComponent<Outline>().effectDistance = new Vector2(1,1);
    }
    
    //CARD HIGHLIGHT END
}
