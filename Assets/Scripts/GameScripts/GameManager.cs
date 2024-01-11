using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public UIManager UIManager;
    public GameObject PlayerSlot; 
    public PlayerManager PlayerManager;   
    public string GameState = "Ready";
    public int PlayerHealth = 20;
    public int EnemyHealth = 20;

    public int totalPlayerDoubloons = 2;
    public int currentPlayerDoubloons = 0;
    
    public int totalEnemyDoubloons = 2;   
    public int currentEnemyDoubloons = 0;

    public int PlayerHandSize = 0;
    public int EnemyHandSize = 0;

    public int EnemyDeckSize = 0;
    public int PlayerDeckSize = 0;

    public int TotalDoubloons = 20;

    private int ReadyClicks = 0;

    public string PlayerDeck;
    public string EnemyDeck;

    public bool PlayerReady;
    public bool EnemyReady;

    void Start()
    {
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        UIManager.UpdateButtonText(GameState); 
        PlayerSlot = GameObject.Find("PlayerSlot");

        currentEnemyDoubloons = totalEnemyDoubloons;
        currentPlayerDoubloons = totalPlayerDoubloons;
        UIManager.UpdatePlayerText();
    }

    public void CardPlayed()
    {
        UIManager.HighlightTurn();

        if(isOwned)
        {
            if (PlayerHealth <= 0)
            {
                UIManager.DisplayWin(false);
            }
            else if(EnemyHealth <= 0)
            {
                Debug.Log("You Wins!");
                UIManager.DisplayWin(true);
            }
        }
        else if (!isOwned)
        {
            if (PlayerHealth <= 0)
            {
                Debug.Log("They WINS!");
                UIManager.DisplayWin(false);
            }
            else if(EnemyHealth <= 0)
            {
                Debug.Log("You Wins!");
                UIManager.DisplayWin(true);
            }
        }

        UIManager.UpdatePlayerText();
    }

    public void HighLightStart()
    {
        UIManager.HighlightTurn();
    }

    public void EndTurn()
    {
        UIManager.HighlightTurn();

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                child.GetComponent<CardDetails>().AttackTurn(true);
            }
        }        
        
        currentPlayerDoubloons = totalPlayerDoubloons;
        currentEnemyDoubloons = totalEnemyDoubloons;
        UIManager.UpdatePlayerText();
    }

    public void ChangeBP(int playerBp, int enemyBp, bool isOwned)
    {
        if(isOwned)
        {
            PlayerHealth += playerBp;
            EnemyHealth -= enemyBp;
        }
        else
        {
            PlayerHealth -= playerBp;
            EnemyHealth += enemyBp;
        }
        UIManager.UpdatePlayerText();
    }

    public void UpdateDoubloons(int amount, bool isOwned, bool stealing)
    {
        if (TotalDoubloons < 1)
        {
            return;
        }

        if(amount >= TotalDoubloons)
        {
            amount = TotalDoubloons;
        }

        if(stealing)
        {
            if(isOwned)
            {
                totalPlayerDoubloons += amount;
                TotalDoubloons -= amount;
            }
            else
            {
                totalEnemyDoubloons += amount;
                TotalDoubloons -= amount;
            }
        }

        if(!stealing)
        {
            if(isOwned)
            {
                currentPlayerDoubloons -=amount;
            }
            else
            {
                currentEnemyDoubloons -= amount;
            }
        }

        UIManager.UpdatePlayerText();
    }

    public void SetPlayerHealth(int health, bool isOwned)
    {
        if(isOwned)
        {
            PlayerHealth = health;
        }
        else
        {
            EnemyHealth = health;
        }

        UIManager.UpdatePlayerText();
    }

    public void AdjustPlayerHealth(int health, bool isOwned) // For adjusting health of first-person player
    {
        if (isOwned)
        {
            PlayerHealth += health;
            if (PlayerHealth <= 0)
            {
                UIManager.DisplayWin(false);
            }
        }
        else
        {
            EnemyHealth += health;
            if (EnemyHealth <= 0)
            {
                UIManager.DisplayWin(true);
            }
        }
        UIManager.UpdatePlayerText();
    } 

    public void AdjustEnemyHealth(int health, bool isOwned) // For adjusting health of first-person enemy
    {
        if (isOwned)
        {
            EnemyHealth += health;
            if(EnemyHealth <= 0)
            {
                UIManager.DisplayWin(true);
            }
        }
        else
        {
            PlayerHealth += health;

            if (PlayerHealth <= 0)
            {
                UIManager.DisplayWin(false);
            }
        }
        UIManager.UpdatePlayerText();
    } 
}


