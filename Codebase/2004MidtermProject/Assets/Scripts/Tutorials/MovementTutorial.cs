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
                if(!TutorialManager.pitDeath)
                {
                    TutorialManager.PlayVoiceLine(0);
                    TutorialManager.PlayVoiceLine(1, true, "Movement", "Use <color=yellow>WASD</color> to move around. Holding <color=yellow>SHIFT</color> causes you to sprint. Press <color=yellow>SPACEBAR</color> to jump. Sprinting requires stamina and using sprint will drain a portion of you stamina. Don't worry though, as long as your not using it, stamina will regenerate over time.");
                }
                else
                {
                    TutorialManager.PlayVoiceLine(17);
                }
                triggered = true;
            }
            if(ledge)
            {
                TutorialManager.PlayVoiceLine(2, true, "Sliding", "Use <color=yellow>LEFT CTRL</color> to slide forward. Sliding allows you to move under and through small spaces. Sliding requires stamina and using a slide will drain a portion of you stamina.");
                triggered = true;
            }
            if(slide)
            {
                TutorialManager.PlayVoiceLine(3);
                triggered = true;
            }
        }
    }
}
