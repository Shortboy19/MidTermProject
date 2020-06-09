using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOnOff : MonoBehaviour
{
    public Light[] tutorailLights;
    [SerializeField] Light RedLight;
    bool triggered = false;
    FlashLight_Tutorial tut;

    private void Start()
    {
        tut = FindObjectOfType<FlashLight_Tutorial>();
        tutorailLights = tut.tutorialLights;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FlashLight") && !triggered)
        {
            for (int i = 0; i < tutorailLights.Length; i++)
            {
                tutorailLights[i].gameObject.SetActive(true);
            }
            RedLight.enabled = false;
            SoundManager.Instance.PlayVoiceLine(7);
            triggered = true;
            StartCoroutine(Voiceline());
        }
    }

    IEnumerator Voiceline()
    {
        PlayerController.Player.frozen = true;
        while(SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        SoundManager.Instance.PlayVoiceLine(8);
        PlayerController.Player.frozen = false;
        DialogBox.ShowWindow("Interaction", "The flashlight can be used on all <color=red>glowing red objects</color> to trigger interactions.", false);
    }

    IEnumerator VoiceLine2()
    {
        PlayerController.Player.frozen = true;
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        PlayerController.Player.frozen = false;
    }
}
