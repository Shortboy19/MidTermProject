using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof (Button))]
public class ButtonSounds : MonoBehaviour
{
        public AudioClip sound; 
        
    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } } 

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>(); 
        source.clip = sound;
        source.playOnAwake = false;

       // button.onClick.AddListener(() => PlayButton()); 

    }

     public void PlayButton()
     {
        //SoundManager.Instance.PlayEffectAtPoint(source.clip, transform.position, 1);

        source.PlayOneShot(sound); 
     }
}
