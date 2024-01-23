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
    public AudioSource CardSounds;
    public AudioSource HeroAbility;
    public AudioSource HeroSounds;

    public AudioClip TurnSound;
    public AudioClip HurtSound;
    public AudioClip DoubloonSound;
    public AudioClip HeroAbilitySound;
    public AudioClip DeathSound;

    public AudioClip DeckSelectedSound;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void PlayTurnFX()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        TurnSource.clip = TurnSound;
        TurnSource.Play();
    }

    public void PlayDoubloonFX()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        HeroSounds.clip = DoubloonSound;
        HeroSounds.Play();
    }
    
    public void PlayHurtFX()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        CardSounds.clip = HurtSound;
        CardSounds.Play();
    }
    
    public void PlayHeroAbilityFX()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        HeroAbility.clip = HeroAbilitySound;
        HeroAbility.Play();
    }

    public void PlayDeathFX()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        CardSounds.clip = DeathSound;
        CardSounds.Play();
    }

    public void PlayDeckFX()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        TurnSource.clip = DeckSelectedSound;
        TurnSource.Play();
    }
}
