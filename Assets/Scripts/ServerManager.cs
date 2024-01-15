using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerManager : NetworkManager
{
    public void StopServer()
    {
        // Stop the server by destroying the NetworkManager object
        NetworkManager.singleton.StopServer();
        Destroy(gameObject);
    }
}
