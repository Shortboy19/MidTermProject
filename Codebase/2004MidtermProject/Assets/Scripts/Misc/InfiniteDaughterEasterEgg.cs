using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteDaughterEasterEgg : MonoBehaviour
{
    [SerializeField] GameObject egg;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FlashLight") && PlayerController.Player.UVFlashlight)
        {
            egg.SetActive(true);
        }
    }
}
