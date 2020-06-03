﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    public static int shardCharge = 0;
    float rotSpeed = 0.035f;
    [Range(0, 1)]
    [SerializeField] float floatAmount = 0.05f;

    [SerializeField] GameObject textObj;
    [SerializeField] GameObject trailMaker;
    Light[] lights;

    bool playerCanActivate = false;
    float floatSpeed = 0;
    Vector3 floatVec;

    void Start()
    {
        textObj.SetActive(false);
        lights = GetComponentsInChildren<Light>();
        floatVec = transform.position;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (playerCanActivate)
                Activate();
        }

        transform.Rotate(new Vector3(0, rotSpeed, 0));

        Bounce();
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
        StartCoroutine(Spin());
        textObj.SetActive(false);
        playerCanActivate = false;
        PlayerController.Player.hasShard = false;

        //activate shard here
        switch (shardCharge)
        {
            case 1:
                YellowShard();
                break;
            case 2:
                GreenShard();
                break;
            case 3:

                break;
            case 4:

                break;
            default:
                //Do nothing
                break;
        }
    }

    void Bounce()
    {
        transform.position = Vector3.Lerp(floatVec, new Vector3(0, floatVec.y * (1+floatAmount), 0), floatSpeed);

        floatSpeed = Mathf.PingPong(Time.time * 0.5f, 1);
    }

    float rotSpeedMod = 1;
    IEnumerator Spin()
    {
        while (rotSpeedMod > 0)
        {
            rotSpeed = Mathf.Lerp(0.035f, 5, rotSpeedMod);
            yield return new WaitForEndOfFrame();
            rotSpeedMod -= 0.005f;
            Debug.Log(rotSpeedMod);
        };
    }

    void YellowShard()
    {
        string message = "You just activated the <color=yellow>Yellow Monolith Shard</color>. Your minimap will now display the quickest possible path to the exit. \n\nBut be warned, the monster has now become <color=red>aggrivated</color> and will chase after you at accelerated speeds.";
        DialogBox.ShowWindow("Shard Activated", message);
        //put trail activation here
        trailMaker.SetActive(true);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.yellow;
        }
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);
        Enemy enemy = FindObjectOfType<Enemy>();
        enemy.agent.speed = 6.25f;
        enemy.oldSpeed = 6.25f;
    }

    void GreenShard()
    {
        string message = "You just activated the <color=green>Green Monolith Shard</color>. Your stamina and battery life have just been doubled and refilled. \n\nBut be warned, your stamina will now recover at <color=red>half</color> the rate it used to and batteries have become <color=red>less</color> effective.";
        DialogBox.ShowWindow("Shard Activated", message);
        //put trail activation here
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.green;
        }
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);

        PlayerController.Player.maxStamina *= 2;
        PlayerController.Player.maxBattery *= 2;

        PlayerController.Player.currStamina *= PlayerController.Player.maxStamina;
        PlayerController.Player.currBattery *= PlayerController.Player.maxBattery;

        PlayerController.Player.staminaRegenRate *= 0.5f;
    }
}
