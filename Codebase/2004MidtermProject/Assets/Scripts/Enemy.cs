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
    public Renderer rend; 

    bool stunned = false;
    [HideInInspector] public float oldSpeed;
    [HideInInspector] public bool inMonolith = false;
    [SerializeField] Transform eyes;

    Light[] eyelights; 

    Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(spawmpoint.position);
        oldSpeed = agent.speed;
        playerCam = Camera.main;
        rend = GetComponent<Renderer>();
        eyelights = GetComponentsInChildren<Light>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.gameWon)
            return;

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

    float stunDuration = 4;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FlashLight"))
        {
            if(PlayerController.enemySeen)
                Stun(stunDuration);
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

    IEnumerator StunEnemy(float waitTime)
    {
        rend.material.color = Color.blue;
        for (int i = 0; i < eyelights.Length; i++)
        {
            eyelights[i].color = Color.white;
        }
        agent.speed = 0;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        if (!inMonolith)
            agent.speed = oldSpeed;
        GetComponent<Collider>().enabled = true;
        rend.material.color = Color.white;
        for (int i = 0; i < eyelights.Length; i++)
        {
            eyelights[i].color = Color.red;
        }
        stunDuration -= 0.5f;
    }

    bool kill = false;
    float speed = 1;
    void KillPlayerAnim()
    {
        Quaternion targetRot = Quaternion.LookRotation(eyes.position - playerCam.transform.position);
        playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed);
        speed += 0.025f;

        if (speed >= 2)
            GameState.ShowDeathMenu();
    }


}
