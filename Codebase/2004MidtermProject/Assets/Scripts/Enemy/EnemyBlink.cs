using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlink : MonoBehaviour
{
    [SerializeField] Light[] eyes = null;
    // Start is called before the first frame update
    void Start()
    {
        //eyes = GetComponentsInChildren<Light>();
        InvokeRepeating("EyesBlinking", 2, Random.Range(1, 6));
    }
    void EyesBlinking()
    {
        StartCoroutine(Blinking());
    }

    IEnumerator Blinking()
    {
        int num = Random.Range(1, 3);
        for (int i = 0; i < num; i++)
        {
            yield return new WaitForSeconds(0.06f);

            for (int j = 0; j < eyes.Length; j++)
            {
                eyes[j].gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(0.08f);

            for (int j = 0; j < eyes.Length; j++)
            {
                eyes[j].gameObject.SetActive(true);
            }
        }

    }
}
