
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof (Button))]
public class ButtonSounds : MonoBehaviour
{
    public AudioClip sound = null;
    public AudioClip sound2 = null;
    private AudioSource source { get { return GetComponent<AudioSource>(); } } 

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.priority = 0;
    }

     public void PlayButton()
     {
        //SoundManager.Instance.PlayEffectAtPoint(source.clip, transform.position, 1);
        source.volume = 1 * SoundManager.EffectsVolume;
        source.PlayOneShot(sound);
     } 

    public void PlaySecondSound()
    {
        source.volume = 1 * SoundManager.EffectsVolume;
        source.PlayOneShot(sound2); 
    }
    

   
}
