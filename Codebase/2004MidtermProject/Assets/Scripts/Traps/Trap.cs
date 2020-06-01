using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] GameObject interactableCanvas;
    public GameObject trapObj;
    public GameObject button;
    public GameObject button1;
    public Material armedMat;
    public Material unarmedMat;
    public float duration = 5;

    bool activated = false;
    bool playerInRange = false;
    bool armed = false;

    void Start()
    {
        interactableCanvas.SetActive(false);
        trapObj.SetActive(false);
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
        armed = true;
        button.GetComponent<MeshRenderer>().material = armedMat;
        if(button1!=null)
            button1.GetComponent<MeshRenderer>().material = armedMat;
    }
    void TurnOn()
    {
        trapObj.SetActive(true);
        activated = true;
    }
    void TurnOff()
    {
        trapObj.SetActive(false);
        activated = false;
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
        if (other.CompareTag("Player"))
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
}
