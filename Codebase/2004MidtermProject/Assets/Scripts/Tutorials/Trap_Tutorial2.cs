using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Tutorial2 : MonoBehaviour
{
    public ObjectiveTracker objective;
    public GameObject invisibleWall;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            objective.DisplayNewObjective("Continue down the path");
            invisibleWall.SetActive(false);
        }
    }
}
