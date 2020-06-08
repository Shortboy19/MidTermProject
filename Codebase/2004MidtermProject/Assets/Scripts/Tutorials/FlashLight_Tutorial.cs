using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight_Tutorial : MonoBehaviour
{
    [SerializeField] Light[] tutorialLights;
    [SerializeField] bool trigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!trigger)
            {
                for (int i = 0; i < tutorialLights.Length; i++)
                {
                    tutorialLights[i].gameObject.SetActive(false);
                }
                SoundManager.Instance.PlayVoiceLine(4);
                PlayerController.tutorialBattery = true;
            }
            else
            {
                SoundManager.Instance.PlayVoiceLine(6);
            }
        }
    }
}
