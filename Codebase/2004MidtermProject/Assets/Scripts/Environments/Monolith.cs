﻿using System.Collections;
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
        if (playerCanActivate && Input.GetButtonDown("Interact"))
            Activate();
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
        SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.MonolithActivate);
        Enemy enemy = FindObjectOfType<Enemy>();
        enemy.oldSpeed = 5.5f;
        enemy.agent.speed = 5.5f;
    }
}
