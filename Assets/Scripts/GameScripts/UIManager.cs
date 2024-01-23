using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;
    public SoundManager SoundManager;
    public GameObject Button;
    public GameObject PlayerHealthText;
    public GameObject EnemyHealthText;
    public GameObject EnemyDblText;
    public GameObject PlayerDblText;
    public GameObject DoubloonText;
    public GameObject currentSelectedCard;
    public bool isCardSelected;

    public Sprite KeaganImage, MarkImage, ChrisImage, DeionImage;

    public GameObject Canvas;
    public GameObject WinDisplay;
    public GameObject TurnDisplay; 

    public Sprite RedPlayerHighlight;
    public Sprite BluePlayerHighlight;
    public Sprite PurplePlayerHighlight;
    public Sprite NoPlayerHighlight;
    public Sprite PlayerTurnHighlight;
    public Sprite DamagedPlayerHighlight;

    public Sprite RedCardHighlight;
    public Sprite BlueCardHighlight;
    public Sprite GreenCardHighlight;
    public Sprite GreyCardHighlight;
    public Sprite PurpleCardHighlight;
    public Sprite NoCardHighlight;
    public Sprite DamagedCardHighlight;
    public Sprite DestroyCardHighlight;

    public Image PlayerHighlight;
    public Image EnemyHighlight;

    void Start()
    {
        SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Canvas = GameObject.Find("Main Canvas");
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UpdatePlayerText();

        PlayerHighlight = GameObject.Find("PlayerHighlight").GetComponent<Image>();
        EnemyHighlight = GameObject.Find("EnemyHighlight").GetComponent<Image>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient(); //Important for accessing the OnStartClient() method.
        Debug.Log("Player Joined");
    }

    public void DisplayWin(bool Win)
    {
        GameObject Display = Instantiate(WinDisplay, new Vector2(0, 0), Quaternion.identity);
        Display.transform.SetParent(Canvas.transform, false);

        if (Win)
        {
            foreach (Transform child in Display.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.name == "DisplayText")
                {
                    child.GetComponent<Text>().text = "You Win!";
                }
            } 
        }
        else
        {
            foreach (Transform child in Display.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.name == "DisplayText")
                {
                    child.GetComponent<Text>().text = "They Win!";
                }
            }  
        }
    }
    
    public void DisplayTurnDisplay()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        if(PlayerManager.IsMyTurn)
        {      
            Debug.Log(TurnDisplay);
            
            StartCoroutine(ActivateObjectForDuration());
        }

        IEnumerator ActivateObjectForDuration()
        {
            if(TurnDisplay != null)
            {
                SoundManager.PlayTurnFX();
                // Activate the GameObject
                TurnDisplay.SetActive(true);

                // Wait for 3 seconds
                yield return new WaitForSeconds(2f);

                // Deactivate the GameObject after 3 seconds
                TurnDisplay.SetActive(false);
            }
        }
    }
    
    public void UpdatePlayerText()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PlayerHealthText.GetComponent<Text>().text = GameManager.PlayerHealth.ToString();
        EnemyHealthText.GetComponent<Text>().text = GameManager.EnemyHealth.ToString();
        PlayerDblText.GetComponent<Text>().text =  GameManager.currentPlayerDoubloons + "/" + GameManager.totalPlayerDoubloons;
        EnemyDblText.GetComponent<Text>().text = GameManager.currentEnemyDoubloons + "/" + GameManager.totalEnemyDoubloons;

        DoubloonText.GetComponent<Text>().text = GameManager.TotalDoubloons.ToString();
    }

    public void UpdateButtonText(string gameState)
    {
        Button = GameObject.Find("Button");
        Button.GetComponentInChildren<Text>().text = gameState;
    }

    public void HighlightTurn()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            
        if (PlayerManager.IsMyTurn)
        {
            PlayerHighlight.sprite = PlayerTurnHighlight;
            EnemyHighlight.sprite = NoPlayerHighlight;
            Button.GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            EnemyHighlight.sprite = PlayerTurnHighlight;
            PlayerHighlight.sprite = NoPlayerHighlight;
            
            Button.GetComponentInChildren<Text>().color = Color.gray;
        }
    }

    // public void SetPlayerEffect(string color, bool forPlayer)
    // {
    //     Sprite chosenColor = NoPlayerHighlight;
    //     Image PlayerHighlight = GameObject.Find("PlayerHighlight").GetComponent<Image>();
    //     Image EnemyHighlight = GameObject.Find("EnemyHighlight").GetComponent<Image>();

    //     if(color == "blue")
    //     {
    //         chosenColor = BluePlayerHighlight;
    //     }
    //     else if (color == "red")
    //     {
    //         chosenColor = RedPlayerHighlight;
    //     }
    //     else if(color == "purple")
    //     {
    //         chosenColor = PurplePlayerHighlight;
    //     }
    //     else if(color == "noeffect")
    //     {
    //         chosenColor = NoPlayerHighlight;
    //     }

    //     if(forPlayer)
    //     {
    //         PlayerHighlight.sprite = chosenColor;
    //     }
    //     else 
    //     {
    //         EnemyHighlight.sprite = chosenColor;
    //     }
    // }
}
