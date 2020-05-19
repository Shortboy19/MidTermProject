using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //audio players component.
    [SerializeField] AudioSource EffectSound;
    [SerializeField] AudioSource MusicSound;
    public AudioSource[] ThunderClap;
    public AudioSource PlayerWalking;
    public AudioSource PlayerRunning;
    public AudioSource Rain;
    public AudioSource MetalGate;
    public AudioSource GhostPassThroughWalls;
    public AudioSource GhostBreath;
    public AudioSource FlashLightClick;
    public AudioSource MetalGateClose;
    public AudioSource GhostFade;
    public AudioSource Dark; 

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

    public void PlayEffect(AudioClip clip)
    {
        EffectSound.clip = clip;
        EffectSound.Play();
    }

    //Play a single Clip through the music source.
    public void PlayMusic(AudioClip song)
    {
        MusicSound.clip = song;
        MusicSound.Play(); 
    }
}
