using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class GameUIButtons : NetworkBehaviour
{
    public PlayerManager PlayerManager;

    public void ConcedeGame()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        PlayerManager.CmdConcedeGame();
    }

    public void LoadMainMenu()
    {
        NetworkManager NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        if(isServer)
        {
            Debug.Log("Server");
            NetworkManager.StopHost();
        }
        if(isClient)
        {
            NetworkManager.StopClient();
            Debug.Log("Client");
        }
        Debug.Log("isServer: " + isServer + " isClient" + isClient);
        SceneManager.LoadScene("MainMenu");
        Debug.Log("isServer: " + isServer + " isClient" + isClient);
    }
}
