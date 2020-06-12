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
            TutorialManager.PlayVoiceLine(9, true, "Traps", "Arm traps by walking up to it and pressing <color=yellow>E</color> when the prompt appears. Traps will arm and automatically trigger when the monster gets close to it, making it a useful tool to protect yourself with. If you see a trap, you should probably arm it.");
        }
    }
}
