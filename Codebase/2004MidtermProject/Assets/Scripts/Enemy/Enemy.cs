using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent = null;
    public GameObject player = null;
    public int SpawnLocationNumber = -1;
    public Transform[] spawnPoints = null;
    [HideInInspector] public Renderer rend = null; 
    public Renderer StunRend = null;
    [HideInInspector] public Animator anim = null;

    [HideInInspector] public bool stunned = false;
    [HideInInspector] public bool scared = false;
    [HideInInspector] public float oldSpeed = 0;
    public Transform eyes = null;

    Light[] lights = null; 
    Light[] eyelights = null; 

    Camera playerCam = null;

    GameObject lifeSaver = null;

    Material normalMat = null;
    [SerializeField] Material transMat = null;
    [SerializeField] Vector3 defPos = Vector3.zero;
    //public static bool firstFreeze = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(PickSpawnPoint().position);
        oldSpeed = agent.speed;
        playerCam = Camera.main;
        rend = GetComponent<Renderer>();
        lights = GetComponentsInChildren<Light>();
        eyelights = eyes.GetComponentsInChildren<Light>();
        agent.updateRotation = false;
        anim = GetComponentInChildren<Animator>();

        lifeSaver = GameObject.Find("Monolith");
        normalMat = StunRend.material;
    }

    public Transform PickSpawnPoint()
    {
        SpawnLocationNumber = Random.Range(0, spawnPoints.Length);
        return spawnPoints[SpawnLocationNumber];
    }

    void Update()
    {
        if (GameState.gamePaused)
        {
            agent.SetDestination(transform.position);
            return;
        }

        if (player != null && !stunned && !kill && !scared)
        {
            if (PlayerController.Player.isOnNavMesh)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.SetDestination(defPos);
            }
        }

        if (kill)
            KillPlayerAnim();
            
    }

    private void LateUpdate()
    {
        if(!stunned && !kill)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    public void Stun(float time)
    {
        StartCoroutine(StunEnemy(time));
    }

    float stunDuration = 4;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HalfWall"))
        {
            if( Vector3.Distance(transform.position, player.transform.position) < 10)
                SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.GhostPassThroughWalls);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if(!PlayerController.Player.hasBlueLife)
            {
                if(!kill)
                {
                    anim.SetBool("Stunned", true);
                    agent.SetDestination(transform.position);
                    stunned = true;
                    //agent.Warp(PlayerController.Player.transform.position - playerCam.transform.forward);
                    Quaternion targetRot = Quaternion.LookRotation(playerCam.transform.position - transform.position);
                    targetRot.x = targetRot.z = 0;
                    transform.rotation = targetRot;
                    kill = true;
                    StartCoroutine(CapturePlayer());
                }
            }
            else
            {
                lifeSaver.GetComponent<LifeSaver>().SaveFromMonster();
            }
        }

        if (other.gameObject.CompareTag("FlashLight") && !scared && !stunned)
        {
            if (PlayerController.enemySeen )
            {
                if (PlayerController.Player.UVFlashlight)
                {
                    if (ScareRoutine == null)
                    {
                        ScareRoutine = Scare(4);
                        StartCoroutine(ScareRoutine);
                    }
                }
                else
                {
                    if (StunRoutine == null)
                    {
                        StunRoutine = StunEnemy(stunDuration);
                        StartCoroutine(StunRoutine);
                        //if(!firstFreeze)
                        //{
                        //    DialogBox.ShowWindow("Stunning", "You <color=yellow>stunned</color> the monster. Stunning the monster causing it to become <color=yellow>frozen</color> and <color=yellow>harmless</color> for a short while.\n\nBe careful though, stunning the monster causes him to ramain stunned for a <color=red>shorter</color> duration next time.");
                        //    firstFreeze = true;
                        //}
                    }
                }
            }
        }
    }

    IEnumerator StunRoutine;
    IEnumerator ScareRoutine;
    IEnumerator StunEnemy(float waitTime)
    {
        agent.speed = 0;
        GetComponent<Collider>().enabled = false;
        anim.SetBool("Stunned", true);
        rend.material.color = Color.blue;
        StunRend.material = transMat;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.white;
        }
        yield return new WaitForSeconds(waitTime);

        agent.speed = oldSpeed;
        rend.material.color = Color.white;
        StunRend.material = normalMat;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.red;
        }
        anim.SetBool("Stunned", false);
        stunDuration -= 0.5f;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = true;
        stunned = false;
        StunRoutine = null;
    }

    public bool kill = false;
    float speed = 1;
    void KillPlayerAnim()
    {
        Quaternion targetRot = Quaternion.LookRotation(eyes.position - playerCam.transform.position);
        playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed);
        speed += 0.025f;
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(3);
        GameState.ShowDeathMenu();
    }

    IEnumerator Scare(float waitTime)
    {
        scared = true;
        GetComponent<Collider>().enabled = false;
        agent.SetDestination(new Vector3(0, 0, 0));
        anim.SetBool("Stunned", true);
        rend.material.color = new Color(1, 0, 1);
        StunRend.material = transMat;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = new Color(1, 0.5f, 0);
        }
        yield return new WaitForSeconds(waitTime);
        scared = false;
        rend.material.color = Color.white;
        StunRend.material = normalMat;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.red;
        }
        anim.SetBool("Stunned", false);
        GetComponent<Collider>().enabled = true;
    }

    public IEnumerator SaviorScare(Vector3 destination)
    {
        scared = true;
        GetComponent<Collider>().enabled = false;
        agent.SetDestination(destination);
        anim.SetBool("Stunned", true);
        rend.material.color = new Color(1, 0, 1);
        StunRend.material = transMat;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = new Color(1, 0.5f, 0);
        }
        yield return new WaitForSeconds(6);
        scared = false;
        rend.material.color = Color.white;
        StunRend.material = normalMat;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.red;
        }
        anim.SetBool("Stunned", false);
        GetComponent<Collider>().enabled = true;
    }

    IEnumerator CapturePlayer()
    {
        GameState.canPause = false;
        PlayerController.Player.frozen = true;
        PlayerController.Player.inDeathAnim = true;
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.StopAllCoroutines();
        }
        SoundManager.Instance.VoiceLineSound.Stop();
        speed = 0;
        SoundManager.Instance.PlayGaze();

        for (int i = 0; i < eyelights.Length; i++)
        {
            eyelights[i].intensity = 5;
            eyelights[i].range = 0;
        }

        Vector3 targVec = eyes.transform.position - playerCam.transform.position;

        Quaternion targRot = Quaternion.LookRotation(targVec);
        float t = 0;
        speed = 0.9f;
        while(t < 1)
        {
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targRot, t);
            t += speed * Time.deltaTime;
            yield return null;
        }

        speed = 0;
        while (eyelights[0].intensity < 20)
        {
            for (int i = 0; i < eyelights.Length; i++)
            {
                eyelights[i].intensity = Mathf.Lerp(5, 20, Time.deltaTime * speed);
                eyelights[i].range = Mathf.Lerp(0, 0.15f, Time.deltaTime * speed);
            }
            speed += 0.15f;
            yield return null;
        }

        while (SoundManager.Instance.GazeSound.isPlaying)
        {
            yield return null;
        }
        SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.PlayerHurt);
        GameState.ShowDeathMenu();
        GameState.canPause = true;
    }
}
