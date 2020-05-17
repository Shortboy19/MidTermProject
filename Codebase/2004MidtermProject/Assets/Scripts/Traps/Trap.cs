using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] GameObject interactableCanvas;
    public GameObject trapObj;
    public float duration = 5;

    bool activated = false;
    bool playerInRange = false;

    void Start()
    {
        interactableCanvas.SetActive(false);
        trapObj.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (!activated && Input.GetButtonDown("Interact"))
            StartCoroutine(Activate());
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
