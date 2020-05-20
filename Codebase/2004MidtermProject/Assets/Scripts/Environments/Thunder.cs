using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    Light[] lights;
    // Start is called before the first frame update
    void Start()
    {
        lights = GetComponentsInChildren<Light>(); 

        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(false);
        }

        InvokeRepeating("PlayThunderEffect", 8 , Random.Range(10, 31));

        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Rain, transform.position); 
    }

    void PlayThunderEffect()
    {
        int i = Random.Range(0, SoundManager.Instance.ThunderClap.Length);
        SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.ThunderClap[i]);
        StartCoroutine(LightFlicker()); 
    }

    IEnumerator LightFlicker()
    {
        float num = Random.Range(0.1f, 0.16f); 

        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(num);

        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.05f); 

         num = Random.Range(0.1f, 0.16f);

        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(num);

        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(false);
        }
    }
}
