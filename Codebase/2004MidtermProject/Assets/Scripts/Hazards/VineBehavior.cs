using System;
using System.Collections;
using UnityEngine;

public class VineBehavior : MonoBehaviour
{
   
    Vector3 originalVineLoc = Vector3.zero;
    Vector3 finalVineLoc = Vector3.zero;
    public float speed = 1.0f;
    public float durationOfEffect = 5;
    bool active = true;

    void Start()
    {
        originalVineLoc = transform.position;
        originalVineLoc.y = -2.7f;

        finalVineLoc = transform.position;
        finalVineLoc.y = 1.25f;

        transform.position = finalVineLoc;
    }

    private void Update()
    {
        speed = Mathf.Clamp01(speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
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
        active = true;
        while (transform.position != finalVineLoc)
        {
            transform.position = Vector3.Lerp(originalVineLoc, finalVineLoc, speed);
            speed += 1f * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        LerpRoutine = null;
    }

    IEnumerator VineLerpDown()
    {
        active = false;
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

