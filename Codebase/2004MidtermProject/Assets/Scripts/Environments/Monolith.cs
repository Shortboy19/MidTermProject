using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    [SerializeField] GameObject trailMaker;
    Light[] lights;

    bool playerCanActivate = false;

    void Start()
    {
        textObj.SetActive(false);
        lights = GetComponentsInChildren<Light>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (playerCanActivate)
                Activate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(PlayerController.Player.hasShard && PlayerController.Player.hasKey)
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
        trailMaker.SetActive(true);
        for(int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.red;
        }
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);
        Enemy enemy = FindObjectOfType<Enemy>();
        enemy.agent.speed = 6.25f;
        enemy.oldSpeed = 6.25f;
    }
}
