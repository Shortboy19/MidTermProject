using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public static bool pitDeath = false;
    Collider[] blockers;

    private void Awake()
    {
        //singleton management
        if (Instance == null) { Instance = this; } else { if (Instance != this) Destroy(Instance.gameObject); Instance = this; }
    }

    private void Start()
    {
        PlayerController.Player.currBattery = 0;
        SoundManager.Instance.VoiceLineSound.Stop();
        StopAllCoroutines();
    }

    public static void PlayVoiceLine(int i, bool prompt = false, string title = "", string message = "", bool freezePlayer = false)
    {
        Instance.StartCoroutine(Instance.QueueRoutine(Instance.VoiceLine(i, prompt, title, message, freezePlayer)));
    }

    IEnumerator voiceRoutine;

    public IEnumerator VoiceLine(int i, bool prompt, string title = "", string message = "", bool freezePlayer = false)
    {
        if(freezePlayer){PlayerController.Player.frozen = true;}
        SoundManager.Instance.PlayVoiceLine(i);
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        PlayerController.Player.frozen = false;
        if(prompt)
        {
            DialogBox.ShowWindow(title, message, false);
        }
        SoundManager.Instance.VoiceLineSound.Stop();
        voiceRoutine = null;
    }

    public List<IEnumerator> queuedRoutines = new List<IEnumerator>();
    public IEnumerator QueueRoutine(IEnumerator routine)
    {
        queuedRoutines.Add(routine);

        do
        {
            yield return null;
        } while (voiceRoutine != null);

        if (queuedRoutines[0] != null)
        {
            voiceRoutine = queuedRoutines[0];
            queuedRoutines.RemoveAt(0);
            StartCoroutine(voiceRoutine);
        }
    }

    public static void PlayCutOffLine()
    {
        Instance.StopAllCoroutines();
        Instance.queuedRoutines.Clear();
        SoundManager.Instance.VoiceLineSound.Stop();
        SoundManager.Instance.PlayVoiceLine(16);
        Instance.voiceRoutine = null;

    }

}
