using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    [SerializeField] GameObject textObj;

    bool activated = false;
    bool playerCanActivate = false;

    void Start()
    {
        textObj.SetActive(false);
    }

    private void Update()
    {
        if (playerCanActivate && Input.GetButtonDown("Interact"))
            Activate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(PlayerController.Player.hasShard)
            {
                textObj.SetActive(true);
                playerCanActivate = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObj.SetActive(false);
            playerCanActivate = false;
        }
    }

    void Activate()
    {
        textObj.SetActive(false);
        playerCanActivate = false;
        PlayerController.Player.hasShard = false;

        //put trail activation here
    }
}
