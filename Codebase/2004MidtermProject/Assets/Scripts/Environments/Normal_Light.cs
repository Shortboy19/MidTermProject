using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Light : MonoBehaviour
{
    [SerializeField] GameObject interactableCanvas = null;
    public GameObject button = null;
    public float duration = 5;
    public GameObject[] lights = null;
    public ObjectiveTracker objective = null;
    public GameObject invisibleWall = null;

    bool activated = false;
    bool playerInRange = false;

    void Start()
    {
        interactableCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetButtonDown("Interact"))
            if (activated)
            {
                StartCoroutine(TurnOff());
            }
            else
            {
                StartCoroutine(TurnOn());
            }
    }
    IEnumerator TurnOn()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(true);
            yield return new WaitForSeconds(duration);
        }
        if (objective)
        {
            objective.DisplayNewObjective("Explore the house");
            invisibleWall.SetActive(false);
        }
        activated = true;
    }
    IEnumerator TurnOff()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
            yield return new WaitForSeconds(duration);
        }
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
