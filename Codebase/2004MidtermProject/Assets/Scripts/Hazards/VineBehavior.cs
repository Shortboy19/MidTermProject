﻿using System;
using System.Collections;
using UnityEngine;

public class VineBehavior : MonoBehaviour
{
   
    public Vector3 originalVineLoc;
    public Vector3 finalVineLoc;
    public Vector3 currVineLoc;
    //public float slowPlayerAmount = 1.3f;
    public float speed = 1.0f;
    public float durationOfEffect = 5;

    void Start()
    {
        originalVineLoc = transform.position;
        originalVineLoc.y = -1.8f;

        finalVineLoc = transform.position;
        finalVineLoc.y = 1.7f;

        transform.position = finalVineLoc;
    }

    private void Update()
    {
        speed = Mathf.Clamp01(speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Player.walkSpeed = 1.5f;    
        }
        if (other.gameObject.CompareTag("FlashLight"))
        {
            if (LerpRoutine == null)
            {
                LerpRoutine = VineLerpDown();
                StartCoroutine(LerpRoutine);
            }
            else
            {
                StopCoroutine(LerpRoutine);
                LerpRoutine = VineLerpDown();
                StartCoroutine(LerpRoutine);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.Player.walkSpeed = 3.5f;
    }
    IEnumerator LerpRoutine;

    IEnumerator VineLerpUp()
    {
        while(transform.position != finalVineLoc)
        {
            transform.position = Vector3.Lerp(originalVineLoc, finalVineLoc, speed);
            speed += 1f * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        LerpRoutine = null;
    }

    IEnumerator VineLerpDown()
    {
        while (transform.position != originalVineLoc)
        {
            transform.position = Vector3.Lerp(originalVineLoc, finalVineLoc, speed);
            speed -= 1f * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1);
        LerpRoutine = VineLerpUp();
        StartCoroutine(LerpRoutine);
    }
}

