using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        ShowKey();
    }

    public void ShowKey()
    {
        cam.cullingMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Key") | (0 << LayerMask.NameToLayer("Exit")));
    }

    public void ShowExit()
    {
        cam.cullingMask = (1 << LayerMask.NameToLayer("Player")) | (0 << LayerMask.NameToLayer("Key") | (1 << LayerMask.NameToLayer("Exit")));
    }
}
