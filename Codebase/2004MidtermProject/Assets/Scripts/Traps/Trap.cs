using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] GameObject interactableCanvas;
    [SerializeField] GameObject trapObj;
    [SerializeField] float duration = 5;
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
            Activate();
    }

    void Activate()
    {
        trapObj.SetActive(true);
        activated = true;
        StartCoroutine(Deactivate());
    }
    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(duration);
        trapObj.SetActive(false);
        activated = false;
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
