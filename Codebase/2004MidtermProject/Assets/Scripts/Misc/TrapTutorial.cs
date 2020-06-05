using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrapTutorial : MonoBehaviour
{
    public GameObject enemy;
    public GameObject warpPoint;
    public ObjectiveTracker objective;
    public GameObject audiosource;
    public GameObject invisableWall;
    bool triggered = false;
    void Start()
    {
        objective.DisplayNewObjective("Turn on the hallway lights");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objective.DisplayNewObjective("Press the button to arm the wall trap");
            SoundManager.Instance.PlayThunderEffect(audiosource.GetComponent<AudioSource>().clip, 100);
            enemy.GetComponent<NavMeshAgent>().Warp(warpPoint.transform.position);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            objective.DisplayNewObjective("Arm the final trap");
            invisableWall.SetActive(false);
        }
    }
}
