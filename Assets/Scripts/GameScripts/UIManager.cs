using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;
    public GameObject Button;
    public GameObject PlayerHealthText;
    public GameObject EnemyHealthText;
    public GameObject EnemyDblText;
    public GameObject PlayerDblText;
    public GameObject DoubloonText;
    public GameObject currentSelectedCard;
    public GameObject PlayerIamge;
    public GameObject EnemyPicture;
    public bool isCardSelected;

    public Sprite KeaganImage, MarkImage, ChrisImage, DeionImage;

    public GameObject Canvas;
    public GameObject WinDisplay;

    Color blueColor = new Color32(17, 216, 238, 255);

    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UpdatePlayerText();
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
            PlayerManager.PlayerImage.GetComponent<Image>().color = Color.white;
            PlayerManager.EnemyImage.GetComponent<Image>().color = Color.red;
            Button.GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            PlayerManager.EnemyImage.GetComponent<Image>().color = Color.white;
            PlayerManager.PlayerImage.GetComponent<Image>().color = Color.red;
            Button.GetComponentInChildren<Text>().color = Color.gray;
        }
    }
}
