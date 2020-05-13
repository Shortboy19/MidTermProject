using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public Transform spawmpoint; 

    bool stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(spawmpoint.position); 
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !stunned)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public void Stun(float time)
    {
        StartCoroutine(StunEnemy(time));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FlashLight"))
        {
            Stun(4);
        }
    }

    IEnumerator StunEnemy(float waitTime)
    {
        float oldSpeed = agent.speed;
        agent.speed = 0;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        agent.speed = oldSpeed;
        GetComponent<Collider>().enabled = true;
    }

}
