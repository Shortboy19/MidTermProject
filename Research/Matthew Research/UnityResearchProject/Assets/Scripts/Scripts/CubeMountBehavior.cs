using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMountBehavior : MonoBehaviour
{
    //Variable declaration, these will be passed in in practice
    public GameObject player;
    public Vector3 SlideSpeed;
    public float smoothtime;
    public float JumpSpeed;
    public float gravitymultiplier = 2.5f;
    public float NormalJumpGravityMultiplier = 2f;
    public Rigidbody gameobjectrigidbody;
    GameObject CubeMount;
    Vector3 PlayerLocation;
    Vector3 OriginalPlayerPosition;
    Vector3 OriginalMountSize;
    Transform MountLocation;
    Vector3 eAngle;
    Quaternion MountOrientation;
    bool DidiGetaMount;
    
    // Start is called before the first frame update
    void Start()
    {
    
        GetComponent<MeshRenderer>().enabled = false;
        CubeMount = gameObject;
        gameobjectrigidbody = gameObject.GetComponent<Rigidbody>();
        OriginalMountSize = gameObject.transform.localScale;
        MountLocation = CubeMount.transform;
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
            PlayerLocation.y = MountLocation.position.y + 0.5f;
            PlayerLocation.x = MountLocation.position.x;
            PlayerLocation.z = MountLocation.position.z;

            player.transform.position = PlayerLocation;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameobjectrigidbody.velocity = Vector3.up * JumpSpeed;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (gameobjectrigidbody.velocity.y < 0)
                {
                     gameobjectrigidbody.velocity += Vector3.up * Physics.gravity.y * (NormalJumpGravityMultiplier - 1) * Time.deltaTime;
                }
            }
            else if (!Input.GetKey(KeyCode.Space))
            {
                if (gameobjectrigidbody.velocity.y > 0)
                {
                    gameobjectrigidbody.velocity += Vector3.up * Physics.gravity.y * (gravitymultiplier - 1) * Time.deltaTime;
                }     
            }
            if (Input.GetKeyDown("left ctrl"))
            {
                Vector3 idealcrouchsize;
                idealcrouchsize.y = .5f;
                idealcrouchsize.x = gameObject.transform.localScale.x;
                idealcrouchsize.z = gameObject.transform.localScale.z;
                gameObject.transform.localScale = idealcrouchsize;
            }
            if (Input.GetKeyUp("left ctrl"))
            {
                gameObject.transform.localScale = OriginalMountSize;
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
