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
    public bool isCardSelected;

    public Sprite KeaganImage, MarkImage, ChrisImage, DeionImage;

    public GameObject Canvas;
    public GameObject WinDisplay;

    public Sprite RedPlayerHighlight;
    public string RedPlayerHighlightPath = "Assets/Assets/RedPlayerEffect.png";
    public Sprite BluePlayerHighlight;
    public string BluePlayerHighlightPath = "Assets/Assets/BluePlayerEffect.png";
    public Sprite PurplePlayerHighlight;
    public string PurplePlayerHighlightPath = "Assets/Assets/PurplePlayerEffect.png";
    public Sprite NoPlayerHighlight;
    public string NoPlayerHighlightPath = "Assets/Assets/NoPlayerEffect.png";

    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UpdatePlayerText();

        Texture2D Blue = LoadTexture(BluePlayerHighlightPath);
        Texture2D Purple = LoadTexture(PurplePlayerHighlightPath);
        Texture2D Red = LoadTexture(RedPlayerHighlightPath);
        Texture2D NoEffect = LoadTexture(NoPlayerHighlightPath);

        BluePlayerHighlight =  Sprite.Create(Blue, new Rect(0, 0, Blue.width, Blue.height), Vector2.one * 0.5f);
        PurplePlayerHighlight =  Sprite.Create(Purple, new Rect(0, 0, Purple.width, Purple.height), Vector2.one * 0.5f);
        RedPlayerHighlight =  Sprite.Create(Red, new Rect(0, 0, Red.width, Red.height), Vector2.one * 0.5f);
        NoPlayerHighlight =  Sprite.Create(NoEffect, new Rect(0, 0, NoEffect.width, NoEffect.height), Vector2.one * 0.5f);
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
        Image PlayerHighlight = GameObject.Find("PlayerHighlight").GetComponent<Image>();
        Image EnemyHighlight = GameObject.Find("EnemyHighlight").GetComponent<Image>();
            
        if (PlayerManager.IsMyTurn)
        {
            PlayerHighlight.sprite = BluePlayerHighlight;
            EnemyHighlight.sprite = NoPlayerHighlight;
            Button.GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            PlayerHighlight.sprite = NoPlayerHighlight;
            EnemyHighlight.sprite = BluePlayerHighlight;
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

    private Texture2D LoadTexture(string path)
    {
        // Load the texture from the file path
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // LoadImage overwrites the current texture with the image file data

        return texture;
    }
}
