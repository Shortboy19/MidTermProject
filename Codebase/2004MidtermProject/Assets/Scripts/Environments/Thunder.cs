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

        SoundManager.Instance.StartRain();
        InvokeRepeating("PlayThunderEffect", 3 , Random.Range(20, 61));
    }

    public void PlayThunderEffect()
    {
        int i = Random.Range(0, SoundManager.Instance.ThunderClap.Length);
        SoundManager.Instance.PlayThunderEffect(SoundManager.Instance.ThunderClap[i], 0.9f);
        StartCoroutine(LightFlicker()); 
    }

    IEnumerator LightFlicker()
    {
        float num = Random.Range(0.1f, 0.16f);

        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("FakeMonsterScare");
        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(num);

        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("FakeMonsterScare");
        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.05f); 

        num = Random.Range(0.1f, 0.16f);

        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("FakeMonsterScare");
        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(num);
        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("FakeMonsterScare");
        for (int j = 0; j < lights.Length; j++)
        {
            lights[j].gameObject.SetActive(false);
        }
    }
}
