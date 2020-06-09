using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrapTutorial : MonoBehaviour
{
    public GameObject enemy;
    public GameObject audiosource;
    public GameObject invisableWall;
    bool triggered;
    void Start()
    {
        //PlayerController.Player.objective.DisplayNewObjective("");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggered)
        {
            triggered = true;
            SoundManager.Instance.PlayVoiceLine(9);
            //PlayerController.Player.objective.DisplayNewObjective("Press the button to arm the wall trap");
            StartCoroutine(TrapPrompt());
        }
    }

    IEnumerator TrapPrompt()
    {
        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        DialogBox.ShowWindow("Traps", "Activate traps by walking up to it and pressing <color=yellow>E</color> when the prompt appears. Traps will arm when activate and automatically trigger when the monster gets close to it.", false);
    }

}
