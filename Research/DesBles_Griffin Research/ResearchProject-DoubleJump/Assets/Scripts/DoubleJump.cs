using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    [Tooltip("The hight of a single jump.")]
    [Range(5, 10)] [SerializeField] float jumpHeight = 6.5f;

    [Tooltip("The amount of jumps that can be made before touching the ground")]
    [Range(0, 5)] [SerializeField] int numberOfJumps = 2;

    Rigidbody rb;
    int jumpCount;//the current amount of jumps

    void Start()
    {
        rb = GetComponent<Rigidbody>();//find the rigidbody
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        if (jumpCount < numberOfJumps)
        {
            Vector2 vel = rb.velocity;
            vel.y = 0;
            vel += Vector2.up * jumpHeight;
            rb.velocity = vel;
            jumpCount++;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
