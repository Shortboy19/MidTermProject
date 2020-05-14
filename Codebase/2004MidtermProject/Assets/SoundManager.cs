using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //audio players component.
    AudioSource EffectSound;
    AudioSource MusicSound;



    //singleton instance
    public static SoundManager Instance;

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
    public void PlayAtPoint(AudioClip sound, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(sound, point);
    }

    //Play a single Clip through the music source.
    public void PlayMusic(AudioClip song)
    {
        MusicSound.clip = song;
        MusicSound.Play(); 
    }
}
