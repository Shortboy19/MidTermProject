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
        agent.Warp(waypoints[0].position);
    }

    int i = 0;
    private void FixedUpdate()
    {
        if (i == waypoints.Length - 1)
            return;

        if( Vector3.Distance(waypoints[i].position, transform.position) < 0.05f)
        {
            i++;
            agent.Warp(waypoints[i].position);
        }
    }
}
