using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperControl : MonoBehaviour
{
    [SerializeField] AudioSource whisper = null;          
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        whisper.volume = 1 * SoundManager.EffectsVolume; 
    }
}
