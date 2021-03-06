using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    [SerializeField] GameObject interactableCanvas = null;
    public GameObject trapObj = null;
    public GameObject button = null;
    public GameObject button1 = null;
    public GameObject enemy = null;
    public Material armedMat = null;
    public Material unarmedMat = null;
    public float duration = 5;
    public GameObject[] fakeGhosts = null;
    Vector3[] telePoints = null;
    string trapName="";
    Thunder thunder = null;
    [SerializeField] GameObject lightning = null;

   
    bool playerInRange = false;

    public bool armed = false;

    void Start()
    {
        thunder = FindObjectOfType<Thunder>();
        interactableCanvas.SetActive(false);
        if (trapObj)
        {
            trapObj.SetActive(false);
            if (button1 != null)
            {
                trapName = "Wall Trap";
            }
            else
            {
                trapName = "Light Trap";
            }
        }
        if (fakeGhosts.Length>0)
        {
            trapName = "Tele Trap";
            telePoints = new Vector3[fakeGhosts.Length];
            for (int i = 0; i < fakeGhosts.Length; i++)
            {
                telePoints[i] = fakeGhosts[i].transform.position;
            }
        }
        else if (trapName =="")
        {
            trapName = "Slow Trap";
        }
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (!armed && Input.GetButtonDown("Interact"))
            Armed();
    }
    void Armed()
    {
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.TrapButton, transform.position); 
        interactableCanvas.SetActive(false);
        playerInRange = false;
        armed = true;
        button.GetComponent<MeshRenderer>().material = armedMat;
        if(button1!=null)
            button1.GetComponent<MeshRenderer>().material = armedMat;

        if(isTutorialTrap)
        {
            StartCoroutine(TutorialAnim1());
        }
    }
    void TurnOn()
    {
        if (trapObj != null)
        {
            if (isTutorialTrap)
            {
                StartCoroutine(TutorialAnim2());
            }
            trapObj.SetActive(true);
        }
        else if (enemy != null)
        {
            enemy.GetComponent<NavMeshAgent>().Warp(telePoints[Random.Range(0, telePoints.Length - 1)]);
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.TeleportTrap, transform.position, 10);
            StartCoroutine(Lightning());
        }

    }
    void TurnOff()
    {
        if (trapObj)
        {
            if (trapName== "Wall Trap")
            {
                trapObj.GetComponent<Animator>().SetBool("IsOff", true);
            }
            if(!isTutorialTrap)
            trapObj.SetActive(false);
        }
        armed = false;
        button.GetComponent<MeshRenderer>().material = unarmedMat;
        if (button1 != null)
            button1.GetComponent<MeshRenderer>().material = unarmedMat;
    }

    IEnumerator Activate()
    {
        TurnOn();
        yield return new WaitForSeconds(duration);
        TurnOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !armed)
        {
            interactableCanvas.SetActive(true);
            interactableCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Press E or RMB to Arm " + trapName;
            playerInRange = true;
        }
        if (other.CompareTag("Enemy") && armed)
        {
            StartCoroutine(Activate());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactableCanvas.SetActive(false);
            playerInRange = false;
        }
    }

    public bool isTutorialTrap = false;

    IEnumerator TutorialAnim1()
    {
        GameState.canPause = false;
        Camera playerCam = Camera.main;
        float speed = 1;
        Enemy enemyComp = enemy.GetComponent<Enemy>();
        enemyComp.agent.speed = 1f;
        enemyComp.agent.Warp(new Vector3(0, 1.5f, 6.5f));
        PlayerController.Player.frozen = true;

        SoundManager.Instance.PlayVoiceLine(10);
        PlayerController.Player.objective.DisplayNewObjective("RUN!!!");

        Quaternion targetRot = Quaternion.LookRotation(enemy.transform.position - playerCam.transform.position);
        while (speed < 1.2f)
        {
            targetRot = Quaternion.LookRotation(enemy.transform.position - playerCam.transform.position);
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed * 3);
            speed += 0.05f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while(SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        enemyComp.agent.speed = 4;
        PlayerController.Player.frozen = false;
        GameState.canPause = true;
    }

    IEnumerator TutorialAnim2()
    {
        GameState.canPause = false;
        Camera playerCam = Camera.main;
        float speed = 1;
        Enemy enemyComp = enemy.GetComponent<Enemy>();
        enemyComp.agent.speed = 0;
        enemyComp.anim.SetBool("Stunned", true);
        PlayerController.Player.frozen = true;

        SoundManager.Instance.PlayVoiceLine(11);
        PlayerController.Player.objective.DisplayNewObjective("Leave the tutorial");

        Quaternion targetRot = Quaternion.LookRotation(enemy.transform.position - playerCam.transform.position);
        while (speed < 1.2f)
        {
            targetRot = Quaternion.LookRotation(enemy.transform.position - playerCam.transform.position);
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed * 3);
            speed += 0.05f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        enemyComp.agent.Warp(enemyComp.PickSpawnPoint().position);
        PlayerController.Player.frozen = false;
        GameState.canPause = true;
    }

    IEnumerator Lightning()
    {
        if (thunder)
        {
            thunder.PlayThunderEffect();
            if (lightning)
            {
                GameObject temp = Instantiate(lightning, new Vector3(transform.position.x, 85, transform.position.z), Quaternion.identity);
                yield return new WaitForSeconds(0.03f);
                Destroy(temp);
            }
        }
        else
            yield break;
    }
}
