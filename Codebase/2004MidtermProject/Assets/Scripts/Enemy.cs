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
    [HideInInspector] public bool inMonolith = false;
    [SerializeField] Transform eyes;

    Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(spawmpoint.position);
        oldSpeed = agent.speed;
        playerCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !stunned)
        {
            agent.SetDestination(player.transform.position);
        }

        if (kill)
            KillPlayerAnim();
    }

    public void Stun(float time)
    {
        StartCoroutine(StunEnemy(time));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FlashLight"))
        {
            if(PlayerController.enemySeen)
                Stun(4);
        }
        if (other.gameObject.CompareTag("Monolith"))
        {
            if(Monolith.playerInArea)
            {
                inMonolith = true;
                agent.speed = 0;
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            kill = true;
            agent.speed = 0;
            PlayerController.Player.frozen = true;
            Quaternion targetRot = Quaternion.LookRotation(playerCam.transform.position - transform.position);
            targetRot.x = targetRot.x = 0;
            transform.rotation = targetRot;
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

    bool kill = false;
    bool restart = false;
    float speed = 1;
    void KillPlayerAnim()
    {
        Quaternion targetRot = Quaternion.LookRotation(eyes.position - playerCam.transform.position);
        playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed);
        speed += 0.025f;

        if (playerCam.transform.rotation == targetRot)
            restart = true;

        if (restart)
            GameState.ShowDeathMenu();
    }


}
