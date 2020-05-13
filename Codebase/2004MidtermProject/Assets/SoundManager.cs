using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //audio players component.
    public AudioSource EffectsSounds;
    public AudioSource MusicSounds;

    //singleton instance
    public static SoundManager Instance = null;

    //Initialize the singleton instance
    private void Awake()
    {
        //If there is not already an instance of SoundManager, set it to this.
        if(Instance = null)
        {
            Instance = this;
        }
        //If an instance already exist, desotry whatever the object is to enforce the singleton.
        else if(Instance != this)
        {
            Destroy(gameObject); 
        }

        //Set SoundManager to DontDestoryOnLoad so that it wont be destroyed when reloaded.
        DontDestroyOnLoad(gameObject); 
    }

    //Play a single Clip through the sound effects source.
    public void Play(AudioClip clip)
    {
        EffectsSounds.clip = clip;
        EffectsSounds.Play(); 
    }

    //Play a single Clip through the music source.
    public void PlayMusic(AudioClip song)
    {
        MusicSounds.clip = song;
        MusicSounds.Play(); 
    }
}
