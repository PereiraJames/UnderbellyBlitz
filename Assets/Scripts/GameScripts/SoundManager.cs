using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SoundManager : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;
    public UIManager UIManager;

    public AudioSource TurnSource;

    public AudioClip TurnSound;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void PlayTurnFX()
    {
        TurnSource.clip = TurnSound;
        TurnSource.Play();
    }
}
