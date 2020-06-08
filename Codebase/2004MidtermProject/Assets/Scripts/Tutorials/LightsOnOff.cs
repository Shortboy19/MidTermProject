using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOnOff : MonoBehaviour
{
    Light[] tutorailLights;
    [SerializeField] Light RedLight;
    bool RedLightActive = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FlashLight"))
        {
            //for(int i = 0; i < tutorailLights.Length; i++)
            //{
            //    tutorailLights[i].gameObject.SetActive(false);
            //    //PlayerController.Player.frozen = true;
            //}         
            RedLight.enabled = false;
            SoundManager.Instance.PlayVoiceLine(7);
            StartCoroutine(Voiceline()); 
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (RedLightActive)
            {
                SoundManager.Instance.PlayVoiceLine(8);
            }
        }
    }

    IEnumerator Voiceline()
    {
        PlayerController.Player.frozen = true;
        while(SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }

        RedLightActive = true;
        PlayerController.Player.frozen = false;
       // DialogBox.ShowWindow("Interaction", "The flashlight can be used on all <color=red>glowing red objects</color> to trigger interactions."); 
    }
}
