using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOnOff : MonoBehaviour
{
    Light[] tutorailLights;
    [SerializeField] Light RedLight;
    bool RedLightActive = false;

    // Start is called before the first frame update
    void Start()
    {
        tutorailLights = GetComponentsInChildren<Light>();
        for (int i = 0; i < tutorailLights.Length; i++)
        {
            tutorailLights[i].gameObject.SetActive(true); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FlashLight"))
        {
            if (RedLightActive)
            {
                for (int i = 0; i < tutorailLights.Length; i++)
                {
                    tutorailLights[i].gameObject.SetActive(true);
                    //PlayerController.Player.frozen = true;
                    RedLight.gameObject.SetActive(false);
                    RedLightActive = false; 
                }
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < tutorailLights.Length; i++)
            {
                tutorailLights[i].gameObject.SetActive(false);
                //PlayerController.Player.frozen = true;
                RedLight.gameObject.SetActive(true);
                RedLightActive = true;
            }
        }
    }
}
