using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Monolith : MonoBehaviour
{
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

    void Bounce()
    {
        transform.position = Vector3.Lerp(floatVec, new Vector3(0, floatVec.y * (1+floatAmount), 0), floatSpeed);

        floatSpeed = Mathf.PingPong(Time.time, 1);
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
}
