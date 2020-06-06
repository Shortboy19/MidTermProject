using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class VineBehavior : MonoBehaviour
{
    public GameObject vineObj;
    public Vector3 originalVineLoc;
    public Vector3 finalVineLoc;
    public float slowPlayerAmount = 1.3F;
    public float speed = 1.0F;
    private bool flashLightOnIt = false;

    void Start()
    {
        //currVineLoc = transform.position;
        originalVineLoc = transform.position;
        originalVineLoc.y = -0.5f;
        finalVineLoc = transform.position;
        finalVineLoc.y = 10;
        transform.position = originalVineLoc;
    }

    private void FixedUpdate()
    {
        flashLightOnIt = false;
    }
    private void Update()
    {
        if (flashLightOnIt == true)
        {
            transform.position = Vector3.Lerp(originalVineLoc, finalVineLoc, Time.deltaTime * speed);
            speed += 0.05f;
        }
        else if (flashLightOnIt == false)
        {
            transform.position = Vector3.Lerp(originalVineLoc, finalVineLoc, Time.deltaTime * speed);
            speed -= 0.05f;
        }
        speed = Mathf.Clamp01(speed);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("FlashLight"))
        {
            flashLightOnIt = true;
        }

    }
}

