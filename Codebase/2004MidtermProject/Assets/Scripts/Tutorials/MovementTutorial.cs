using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTutorial : MonoBehaviour
{
    [SerializeField] bool start = false;
    [SerializeField] bool ledge = false;
    [SerializeField] bool slide = false;
    bool triggered = false;

    private void Start()
    {
        PlayerController.Player.currBattery = 0;
        SoundManager.Instance.VoiceLineSound.Stop();
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggered)
        {
            if (start)
            {
                StartCoroutine("StartLine");
                triggered = true;
            }
            if(ledge)
            {
                StopCoroutine("StartLine");
                StartCoroutine("SlideLine");
                triggered = true;
            }
            if(slide)
            {
                StopCoroutine("SlideLine");
                SoundManager.Instance.PlayVoiceLine(3);
                triggered = true;
            }
        }
    }

    IEnumerator StartLine()
    {
        PlayerController.Player.frozen = true;
        SoundManager.Instance.PlayVoiceLine(0);
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        SoundManager.Instance.PlayVoiceLine(1);
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        PlayerController.Player.frozen = false;
        DialogBox.ShowWindow("Movement", "Use <color=yellow>WASD</color> to move around. Holding <color=yellow>SHIFT</color> causes you to sprint. Press <color=yellow>SPACEBAR</color> to jump.", false);
        SoundManager.Instance.VoiceLineSound.Stop();
        StopAllCoroutines();
    }

    IEnumerator SlideLine()
    {
        PlayerController.Player.frozen = true;
        SoundManager.Instance.PlayVoiceLine(2);
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        PlayerController.Player.frozen = false;
        DialogBox.ShowWindow("Sliding", "Use <color=yellow>LEFT CTRL</color> to slide forward. Sliding allows you to move under and trough small spaces.", false);
        SoundManager.Instance.VoiceLineSound.Stop();
        StopAllCoroutines();
    }
}
