using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrailMaker : MonoBehaviour
{
    NavMeshAgent agent = null;
    TrailRenderer trail = null;
    [SerializeField] LineRenderer trailPart = null;
    [SerializeField] Transform destination = null;
    [SerializeField] Transform[] waypoints = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //trail = GetComponent<TrailRenderer>();
        StartCoroutine(DrawPath());
    }

    IEnumerator DrawPath()
    {
        trailPart.positionCount = waypoints.Length;
        for (int i  = 0; i < waypoints.Length; i++)
        {
            //agent.Warp(waypoints[i].position);
            for(int j = i; j < waypoints.Length; j++)
            {
                trailPart.SetPosition(j, waypoints[i].position);
            }
            yield return new WaitForSeconds(0.025f);
        }
    }
}
