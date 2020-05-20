using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //audio players component.
    [SerializeField] AudioSource EffectSound;
    [SerializeField] AudioSource MusicSound;
    [SerializeField] AudioSource RainSound;

    public AudioClip[] ThunderClap;
    public AudioClip[] PlayerWalking;
    public AudioClip PlayerRunning;
    public AudioClip Rain;
    public AudioClip MetalGate;
    public AudioClip GhostPassThroughWalls;
    public AudioClip GhostBreath;
    public AudioClip FlashLightClick;
    public AudioClip MetalGateClose;
    public AudioClip GhostFade;
    public AudioClip PlayerSliding;
    public AudioClip TrapButton;
    public AudioClip PlayerHurt; 

    public AudioClip[] effects;

    //singleton instance
    public static SoundManager Instance;

    //Initialize the singleton instance
    private void Awake()
    {
        //If there is not already an instance of SoundManager, set it to this.
        if (Instance == null) { Instance = this; } else if (Instance != this) { Debug.LogError("0 or multiple " + this + " in the scene."); }
    }

    //Play a single Clip through the sound effects source.
    public void PlayEffectAtPoint(AudioClip sound, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(sound, point);
    }
    public void PlayEffectAtPoint(AudioClip sound, Vector3 point, float volume)
    {
        AudioSource.PlayClipAtPoint(sound, point, volume);
    }

    public void PlayGlobalEffect(AudioClip clip)
    {
        EffectSound.clip = clip;
        EffectSound.Play();
    }

    public void PlayEffect(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    //Play a single Clip through the music source.
    public void PlayMusic(AudioClip song)
    {
        MusicSound.clip = song;
        MusicSound.Play(); 
    }

    public void StartRain()
    {
        RainSound.clip = Rain;
        RainSound.loop = true;
        RainSound.Play();
    }
}
