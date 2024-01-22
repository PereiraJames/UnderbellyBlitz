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
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void PlayTurnFX()
    {
        TurnSource.clip = TurnSound;
        TurnSource.Play();
    }

    public void PlayDoubloonFX()
    {
        HeroSounds.clip = DoubloonSound;
        HeroSounds.Play();
    }
    
    public void PlayHurtFX()
    {
        CardSounds.clip = HurtSound;
        CardSounds.Play();
    }
    
    public void PlayHeroAbilityFX()
    {
        HeroAbility.clip = HeroAbilitySound;
        HeroAbility.Play();
    }

    public void PlayDeathFX()
    {
        CardSounds.clip = DeathSound;
        CardSounds.Play();
    }

    public void PlayDeckFX()
    {
        TurnSource.clip = DeckSelectedSound;
        TurnSource.Play();
    }
}
