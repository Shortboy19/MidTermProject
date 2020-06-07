using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    [SerializeField] GameObject interactableCanvas;
    public GameObject trapObj;
    public GameObject button;
    public GameObject button1;
    public GameObject enemy;
    public Material armedMat;
    public Material unarmedMat;
    public float duration = 5;
    public GameObject[] fakeGhosts;
    Vector3[] telePoints;

   
    bool playerInRange = false;

    public bool armed = false;

    void Start()
    {
        interactableCanvas.SetActive(false);
        if (trapObj)
        {
            trapObj.SetActive(false);
        }
        telePoints = new Vector3[fakeGhosts.Length];
        for (int i = 0; i < fakeGhosts.Length; i++)
        {
            telePoints[i] = fakeGhosts[i].transform.position;
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
            
        }
    }
    void TurnOn()
    {
        if (enemy)
        {
            enemy.GetComponent<NavMeshAgent>().Warp(telePoints[Random.Range(0, telePoints.Length - 1)]);
        }
        if (trapObj)
        {
            trapObj.SetActive(true);
        }
    }
    void TurnOff()
    {
        if (trapObj)
        {
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
        if (other.CompareTag("Player")&& !armed)
        {
            interactableCanvas.SetActive(true);
            playerInRange = true;
        }
        if (other.CompareTag("Enemy")&&armed)
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

    IEnumerator TutorialAnim()
    {

    }

}
