using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public Transform spawmpoint; 

    bool stunned = false;
    [HideInInspector] public float oldSpeed;
    bool inMonolith = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(spawmpoint.position);
        oldSpeed = agent.speed;
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
        if (other.gameObject.CompareTag("Monolith"))
        {
            inMonolith = true;
            agent.speed = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Monolith"))
        {
            inMonolith = false;
            agent.speed = oldSpeed;
        }
    }

    IEnumerator StunEnemy(float waitTime)
    {
        agent.speed = 0;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        if (!inMonolith)
            agent.speed = oldSpeed;
        GetComponent<Collider>().enabled = true;
    }

}
