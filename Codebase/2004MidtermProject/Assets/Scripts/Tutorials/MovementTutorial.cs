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
        SoundManager.Instance.PlayVoiceLine(0);
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        SoundManager.Instance.PlayVoiceLine(1);
        DialogBox.ShowWindow("Movement", "Use <color=yellow>WASD</color> to move around. Hold <color=yellow>SHIFT</color> to sprint.", false);
    }

    IEnumerator SlideLine()
    {
        SoundManager.Instance.PlayVoiceLine(2);
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        DialogBox.ShowWindow("Sliding", "Use <color=yellow>LEFT CTRL</color> to slide forward. Sliding allows you to move under and trough small spaces.", false);
    }
}
