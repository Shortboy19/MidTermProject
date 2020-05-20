using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public int SpawnLocationNumber;
    public Transform[] spawnPoints;
    public Renderer rend; 
    public Animator anim;

    bool stunned = false;
    [HideInInspector] public float oldSpeed;
    [SerializeField] Transform eyes;

    Light[] eyelights; 

    Camera playerCam;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(PickSpawnPoint().position);
        oldSpeed = agent.speed;
        playerCam = Camera.main;
        rend = GetComponent<Renderer>();
        eyelights = GetComponentsInChildren<Light>();
        agent.updateRotation = false;
        anim = GetComponentInChildren<Animator>();
    }

    Transform PickSpawnPoint()
    {
        SpawnLocationNumber = Random.Range(1, spawnPoints.Length);
        return spawnPoints[SpawnLocationNumber];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.gameWon)
        {
            agent.speed = 0;
            return;
        }

        if (player != null && !stunned)
        {
            agent.SetDestination(player.transform.position);
        }

        if (kill)
            KillPlayerAnim();
    }

    private void LateUpdate()
    {
        if(!stunned)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
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
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Stunned", true);
            kill = true;
            agent.speed = 0;
            stunned = true;
            PlayerController.Player.frozen = true;
            SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.PlayerHurt);
            Quaternion targetRot = Quaternion.LookRotation(playerCam.transform.position - transform.position);
            targetRot.x = targetRot.x = 0;
            transform.rotation = targetRot;
        }
    }

    IEnumerator StunEnemy(float waitTime)
    {
        anim.SetBool("Stunned", true);
        rend.material.color = Color.blue;
        for (int i = 0; i < eyelights.Length; i++)
        {
            eyelights[i].color = Color.white;
        }
        agent.speed = 0;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        agent.speed = oldSpeed;
        GetComponent<Collider>().enabled = true;
        rend.material.color = Color.white;
        for (int i = 0; i < eyelights.Length; i++)
        {
            eyelights[i].color = Color.red;
        }
        anim.SetBool("Stunned", false);
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
