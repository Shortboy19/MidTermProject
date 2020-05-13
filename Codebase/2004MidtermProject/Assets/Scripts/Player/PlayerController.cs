﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AUTHOR: Griffin DesBles 5/6/2020

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("The current instace of the player in the scene. (Use this instead of GetComponent<PlayerController> when trying to access the player from an outside script.)")]
    public static PlayerController Player;

    #region Public Variables
    [Header("Movement Variables")]
    [Tooltip("The Component of type CharacterController attached to this object.")]
    [HideInInspector] public CharacterController cc;
    [Tooltip("The mouse sensitivity for looking around. (Defaults to 100)")]
    [Range(1, 200)]
    public float LookSenstivity = 100f;
    [Tooltip("The speed at which the player moves. (Defaults to 5)")]
    [Range(0,10)]
    [Space(10)]
    public float walkSpeed = 5f;
    [Tooltip("The multiplier to the players speed while sprinting. (Defaults to 2)")]
    [Range(0, 10)]
    public float sprintSpeedModifier = 2f;
    [Tooltip("The ammount of stamina sprinting requires (Defaults to 3)")]
    public float sprintStaminaCost = 3f;
    [Tooltip("The height that the player jumps. (Defaults to 3)")]
    [Range(0, 5)]
    [Space(10)]
    public float jumpHeight = 3f;
    [Tooltip("The ammount of stamina jumping requires (Defaults to 3)")]
    public float jumpStaminaCost = 3f;
    [Tooltip("The height that the player jumps. (Defaults to 5)")]
    [Range(0, 5)]
    [Space(10)]
    public float slideDistance = 5f;
    [Tooltip("The ammount of stamina sliding requires (Defaults to 3)")]
    public float slideStaminaCost = 3f;

    [Header("Stat Variables")]
    [Tooltip("The players maximum amount of stamina. (Defaults to 10")]
    public float maxStamina = 10f;
    [Tooltip("The players current amount of stamina.")]
    [HideInInspector] public float currStamina;
    [Tooltip("The rate at which the players stamina will regen. (Defaults to 1 per second)")]
    public float staminaRegenRate = 1f;

    [Space(10)]
    public MiniMap minimap;

    [Space(10)]
    public GameObject flashlight;
    #endregion
    #region Private Variables
    Camera cam;//the players camera
    float gravity = 9.8f;
    Vector3 moveDirection = Vector3.zero;
    int KeyCount = 0;

    bool isSprinting = false;
    bool isSliding = false;

    GameObject enemy;
    bool enemySeen = false;

    #endregion

    private void Awake()
    {
        //singleton management
        if ( Player == null) { Player = this; } else { if (Player != this) Debug.LogWarning("Multiple or 0 " + this + " in the scene. There should only be 1 player."); }
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        currStamina = maxStamina;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    
    void Update()
    {
        MouseLook();
        Movement();


        if (!isSprinting && !isSliding) { currStamina += staminaRegenRate * Time.deltaTime; }
        currStamina = Mathf.Clamp(currStamina, 0, maxStamina);

        Vector3 screenPoint = cam.WorldToViewportPoint(enemy.transform.position);
        if(screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            if (enemySeen)
            {
                enemySeen = true;
                //playSound
            }
        }
        else
        {
            enemySeen = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            flashlight.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            flashlight.SetActive(false);
        }
    }

    void Movement()
    {
        if (cc.isGrounded)
        {
            //Are we sprinting
            if (Input.GetButton("Sprint"))
            {
                if (currStamina > sprintSpeedModifier)
                {
                    isSprinting = true;
                    currStamina -= sprintStaminaCost * Time.deltaTime;
                }

            }
            else { isSprinting = false; }

            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed * (isSprinting ? sprintSpeedModifier : 1);

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetButton("Slide"))
            {
                if (isSprinting) { Slide(); }
            } else { cc.height = 1f; isSliding = false; }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        cc.Move(moveDirection * Time.deltaTime);
    }

    private void MouseLook()
    {
        float lookX = Input.GetAxis("Mouse X") * LookSenstivity * Time.deltaTime;
        float lookY = -Input.GetAxis("Mouse Y") * LookSenstivity * Time.deltaTime;

        transform.Rotate(Vector3.up * lookX);
        cam.transform.Rotate(Vector3.right * lookY);
    }

    void Jump()
    {
        if( currStamina > jumpStaminaCost)
        {
            moveDirection.y = jumpHeight;
            currStamina -= jumpStaminaCost;
        }
    }

    void Slide()
    {
        if(currStamina > slideStaminaCost)
        {
            isSliding = true;
            cc.height = 0.5f;
        } else { cc.height = 1f; isSliding = false; }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            KeyCount++;
            minimap.ShowExit();
        }
        if (collision.gameObject.tag == "Exit" && KeyCount > 0)
        {
            Destroy(collision.gameObject);
        }
    }
}