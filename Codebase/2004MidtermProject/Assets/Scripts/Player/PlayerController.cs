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
    [Tooltip("The speed at which the player moves. (Defaults to 2.5)")]
    [Range(0,10)]
    [Space(10)]
    public float walkSpeed = 2.5f;
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
    public float slideDistance = 3f;
    [Tooltip("The ammount of stamina sliding requires (Defaults to 3)")]
    public float slideStaminaCost = 3f;
    [Tooltip("The amount of time it takes to complete a slide.")]
    public float slideDuration = 0.25f;

    [Header("Stat Variables")]
    [Tooltip("The players maximum amount of stamina. (Defaults to 10")]
    public float maxStamina = 10f;
    [Tooltip("The players current amount of stamina.")]
    [HideInInspector] public float currStamina;
    [Tooltip("The rate at which the players stamina will regen. (Defaults to 1 per second)")]
    public float staminaRegenRate = 1f;
    [Tooltip("The players maximum amount of flashlight battery. (Defaults to 10")]
    [Space(10)]
    public float maxBattery = 5f;
    [Tooltip("The players current amount of battery.")]
    [HideInInspector] public float currBattery;
    [Tooltip("The ammount of battery the flashlight requires (Defaults to 1)")]
    public float flashlightBatteryCost = 1f;
    [Tooltip("The ammount of battery the battery pickup refills (Defaults to 2)")]
    public float batteryChargeAmount = 2f;

    [Header("Other/Misc")]
    [Space(10)]
    [SerializeField] MiniMap minimap;
    [SerializeField] GameObject flashlight;
    [SerializeField] ObjectiveTracker objective;
    #endregion

    #region Private Variables
    Camera cam;//the players camera
    float gravity = 9.8f;
    Vector3 moveDirection = Vector3.zero;
    Vector3 slideDirection = Vector3.zero;
    int KeyCount = 0;

    bool isSprinting = false;
    bool isSliding = false;

    [HideInInspector] public GameObject enemy;
    [HideInInspector] public Enemy enemyComp;
    [HideInInspector] public bool frozen;
    public static bool enemySeen = false;

    #endregion

    private void Awake()
    {
        //singleton management
        if ( Player == null) { Player = this; } else { if (Player != this) Debug.LogWarning("Multiple " + this + " in the scene. There should only be 1 player."); }
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        objective = FindObjectOfType<ObjectiveTracker>();
        cam = GetComponentInChildren<Camera>();
        moveSound = GetComponent<AudioSource>();

        currStamina = maxStamina;
        currBattery = maxBattery;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyComp = enemy.GetComponent<Enemy>();
        defaultPosY = cam.transform.localPosition.y;
        frozen = false;

    }

    
    void Update()
    {
        if (GameState.gamePaused)
            return;

        if(!frozen)
        {
            MouseLook();
            Movement();
            //HeadBobbing
            if (cc.isGrounded)
            {
                if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.z) > 0)
                {
                    timer += Time.deltaTime * walkingBobbingSpeed;
                    cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * (isSprinting ? bobbingAmount * 2 : bobbingAmount), cam.transform.localPosition.z);
                }
                else
                {
                    timer = 0;
                    cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, Mathf.Lerp(cam.transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), cam.transform.localPosition.z);
                }
            }
        }

        //stamina
        if (!isSprinting && !isSliding && cc.isGrounded) { currStamina += staminaRegenRate * Time.deltaTime; }
        currStamina = Mathf.Clamp(currStamina, 0, maxStamina);

        //monster detection
        Vector3 screenPoint = cam.WorldToViewportPoint(enemy.transform.position);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            if (!enemySeen)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, enemy.transform.position - transform.position, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject == enemy.gameObject)
                    {
                        enemySeen = true;
                        enemyComp.anim.SetTrigger("Scare");
                        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.effects[0], transform.position);
                        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.effects[2], transform.position);
                    }
                }
            }
        }
        else
        {
            if(enemySeen)
            {
                if(sightDelay == null)
                {
                    sightDelay = DelayedSightRefresh();
                    StartCoroutine(sightDelay);
                }
            }
            else
                enemySeen = false;
        }

        //Flashlight
        if (currBattery > 0)
        {
            if (Input.GetMouseButton(0))
            {
                currBattery -= flashlightBatteryCost * Time.deltaTime;
                FlashLight(true);
            }
            else
            {
                FlashLight(false);
            }
        }
        else
        {
            FlashLight(false);
        }

        //sliding
        if(isSliding)
        {
            if(slideTimer > 0)
                Slide();
            else
            {
                if (!Physics.Raycast(transform.position, transform.up, 1f))
                {
                    walkSpeed = oldSpeed;
                    isSliding = false;
                    cc.height = 1f;
                }
            }
        }
    }

    bool flashlightOn = false;
    void FlashLight(bool val)
    {
        if(val == true)
        {
            if (flashlightOn)
                return;

            flashlight.SetActive(true);
            flashlightOn = true;
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.FlashLightClick, transform.position);
        }
        else
        {
            if (!flashlightOn)
                return;
            flashlightOn = false;
            flashlight.SetActive(false);
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.FlashLightClick, transform.position);
        }
    }

    IEnumerator sightDelay;

    IEnumerator DelayedSightRefresh()
    {
        yield return new WaitForSeconds(10);
        enemySeen = false;
        enemyComp.anim.ResetTrigger("Scare");
        sightDelay = null;
    }

    AudioSource moveSound;

    void Movement()
    {
        if (cc.isGrounded)
        {
            if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.z) > 0)
            {
                if (Input.GetButton("Sprint"))
                {
                    if (currStamina > sprintSpeedModifier)
                    {
                        isSprinting = true;
                        currStamina -= sprintStaminaCost * Time.deltaTime;
                    }
                    else { isSprinting = false;}
                }
                else { isSprinting = false;}
            }
            else { isSprinting = false; moveSound.clip = null; }

            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed * (isSprinting ? sprintSpeedModifier : 1);

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetButtonDown("Slide"))
            {
                if(!isSliding)
                {
                    if (currStamina > slideStaminaCost)
                    {

                        isSliding = true;
                        slideTimer = slideDuration;
                        oldSpeed = walkSpeed;

                        slideDirection = new Vector3(0.0f, 0.0f, 1);
                        slideDirection = transform.TransformDirection(slideDirection);
                        slideDirection *= slideDistance / slideDuration;
                        currStamina -= slideStaminaCost;
                        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.PlayerSliding, transform.position); 
                    }
                }
            }

        }

        moveDirection.y -= gravity * Time.deltaTime;
        if (isSliding) { slideDirection.y -= gravity * 2 * Time.deltaTime; }

        cc.Move((isSliding ? slideDirection : moveDirection) * Time.deltaTime);

        if(!moveSoundPlaying)
        {
            StartCoroutine(PlayMoveSound());
        }
    }

    AudioClip RandomFootstep()
    {
        int i = Random.Range(0, SoundManager.Instance.PlayerWalking.Length);
        return SoundManager.Instance.PlayerWalking[i];
    }

    bool moveSoundPlaying = false;
    IEnumerator PlayMoveSound()
    {
        if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.z) > 0)
            moveSound.clip = RandomFootstep();
        else
            moveSound = null;

        moveSoundPlaying = true;
        float delay = isSprinting ? 0.3225f : 0.3805f;

        if(moveSound == null)
        {
            moveSound = GetComponent<AudioSource>();
        }

        moveSound.Play();
        yield return new WaitForSeconds(delay);
        moveSoundPlaying = false;
    }

    Vector3 rotClamp = Vector3.zero;
    private void MouseLook()
    {
        float lookX = Input.GetAxis("Mouse X") * LookSenstivity * Time.deltaTime;
        float lookY = -Input.GetAxis("Mouse Y") * LookSenstivity * Time.deltaTime;

        transform.Rotate(Vector3.up * lookX);

        rotClamp.x += lookY;
        rotClamp.x = Mathf.Clamp(rotClamp.x, -90, 60);
        cam.transform.localEulerAngles = rotClamp;
    }
    #region Head Bobing Variables
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.025f;
    float defaultPosY;
    float timer = 0;
    #endregion

    void Jump()
    {
        if (currStamina > jumpStaminaCost)
        {
            moveDirection.y = jumpHeight;
            currStamina -= jumpStaminaCost;
        }
    }
    
    float slideTimer;
    float oldSpeed;
    void Slide()
    {
        cc.height = 0.5f;
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            walkSpeed = slideDistance / slideDuration;
            
            if(!Physics.Raycast(transform.position, transform.up, 1f))
            {
                if (slideTimer <= 0)
                {
                    walkSpeed = oldSpeed;
                    isSliding = false;
                    cc.height = 1f;
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            SoundManager.Instance.PlayEffectAtPoint(other.GetComponent<AudioSource>().clip, transform.position, 0.25f);
            Destroy(other.gameObject);
            KeyCount++;
            objective.DisplayNewObjective("Get to the exit");
            minimap.ShowExit();
        }
        if (other.gameObject.tag == "Exit" && KeyCount > 0)
        {
            Destroy(other.gameObject);
            GameState.ShowWinMenu();
        }
        if(other.gameObject.CompareTag("Battery"))
        {
            if(currBattery < maxBattery)
            {
                Destroy(other.gameObject);
                currBattery += batteryChargeAmount;
                if (currBattery > maxBattery)
                    currBattery = maxBattery;
            }
        }
        if (other.gameObject.CompareTag("Entrance"))
        {
            GameState.Instance.timer.isCounting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance"))
        {
            other.isTrigger = false;
        }
    }
}
