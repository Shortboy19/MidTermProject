using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMountBehavior : MonoBehaviour
{
    //Variable declaration, these will be passed in in practice
    public GameObject player;
    GameObject CubeMount;
    Vector3 PlayerLocation;
    Vector3 OriginalPlayerPosition;
    Vector3 MountLocation;
    Vector3 eAngle;
    Quaternion MountOrientation;
    bool DidiGetaMount;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        CubeMount = gameObject;
        MountLocation = CubeMount.transform.position;
        MountOrientation = CubeMount.transform.rotation;
        eAngle = MountOrientation.eulerAngles;
        PlayerLocation = player.transform.position;
        OriginalPlayerPosition = PlayerLocation;
        DidiGetaMount = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            DidiGetaMount = !DidiGetaMount;
            GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
        }
        if (DidiGetaMount == true)
        {
            PlayerLocation.y = MountLocation.y + 0.5f;
            PlayerLocation.x = MountLocation.x;
            PlayerLocation.z = MountLocation.z;

            player.transform.position = PlayerLocation;

            if (Input.GetKeyDown("space"))
            {
                MountLocation.y += 2f;
                CubeMount.transform.position = MountLocation;
            }
            if (Input.GetKeyDown("left ctrl"))
            {
                MountLocation.y -= 0.5f;
                CubeMount.transform.position = MountLocation;
            }
            if (Input.GetKeyDown("left"))
            {
                eAngle.y -= 90f;
                MountOrientation.eulerAngles = eAngle;
                CubeMount.transform.rotation = MountOrientation;
            }
            if (Input.GetKeyDown("right"))
            {
                eAngle.y += 90f;
                MountOrientation.eulerAngles = eAngle;
                CubeMount.transform.rotation = MountOrientation;
            }
        }
        if (DidiGetaMount == false)
        {

            PlayerLocation.y = OriginalPlayerPosition.y;
            PlayerLocation.x = OriginalPlayerPosition.x;
            PlayerLocation.z = OriginalPlayerPosition.z;
            player.transform.position = PlayerLocation;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
