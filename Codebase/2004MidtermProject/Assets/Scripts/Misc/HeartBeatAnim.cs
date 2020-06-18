using System.Collections;
using UnityEngine;

public class HeartBeatAnim : MonoBehaviour
{
    RectTransform rect = null;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Pulse()
    {
        if(routine == null)
        {
            routine = PulseRoutine();
            StartCoroutine(routine);
        }
    }

    IEnumerator routine;
    IEnumerator PulseRoutine()
    {
        rect.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        yield return new WaitForSeconds(0.4f);
        rect.localScale = new Vector3(1,1,1);
        routine = null;
    }
}
