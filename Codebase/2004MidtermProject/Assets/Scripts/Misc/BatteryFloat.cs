using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryFloat : MonoBehaviour
{
    float rotSpeed = 0.3f;
    [Range(0, 1)]
    [SerializeField] float floatAmount = 0.1f;
    float floatSpeed = 0;
    Vector3 floatVec;

    private void Start()
    {
        floatVec = transform.position;
    }

    private void Update()
    {

        transform.Rotate(new Vector3(0, rotSpeed, 0));

        Bounce();
    }
    void Bounce()
    {
        transform.position = Vector3.Lerp(floatVec, new Vector3(floatVec.x, floatVec.y * (1 + floatAmount), floatVec.z), floatSpeed);

        floatSpeed = Mathf.PingPong(Time.time * 0.5f, 1);
    }

    float rotSpeedMod = 1;
    IEnumerator Spin()
    {
        while (rotSpeedMod > 0)
        {
            rotSpeed = Mathf.Lerp(0.035f, 5, rotSpeedMod);
            yield return new WaitForEndOfFrame();
            rotSpeedMod -= 0.005f;
            Debug.Log(rotSpeedMod);
        };
    }
}
