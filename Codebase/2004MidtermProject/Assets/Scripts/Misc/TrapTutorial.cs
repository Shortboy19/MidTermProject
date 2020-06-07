using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrapTutorial : MonoBehaviour
{
    public GameObject enemy;
    public ObjectiveTracker objective;
    public GameObject audiosource;
    public GameObject invisableWall;
    bool triggered;
    void Start()
    {
        objective.DisplayNewObjective("");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggered)
        {
            triggered = true;
            SoundManager.Instance.PlayVoiceLine(9);
            objective.DisplayNewObjective("Press the button to arm the wall trap");
        }
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    objective.DisplayNewObjective("Arm the final trap");
        //    invisableWall.SetActive(false);
        //}
    }
}
