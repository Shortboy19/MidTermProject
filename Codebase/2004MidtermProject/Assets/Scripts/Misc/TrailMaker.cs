using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrailMaker : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform destination;

    [SerializeField] Transform[] waypoints;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(DrawPath());
    }

    IEnumerator DrawPath()
    {
        for (int i  = 0; i < waypoints.Length; i++)
        {
            agent.Warp(waypoints[i].position);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
