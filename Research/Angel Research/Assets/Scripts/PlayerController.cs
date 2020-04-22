using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody player;
    private Rigidbody rb;
    public float playerSpeed = 1f;
    public float turnSpeed;
    public Vector3 playerPosition;
    public float timeScale = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.forward * playerSpeed);

        }
        if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow))
        {
        
            player.transform.Rotate(playerPosition.x, playerPosition.y - 10 * Time.deltaTime * timeScale - turnSpeed, playerPosition.z);
        }
        if (Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Rotate(playerPosition.x, playerPosition.y + 10 * Time.deltaTime * timeScale + turnSpeed, playerPosition.z);
        }
    }
}
