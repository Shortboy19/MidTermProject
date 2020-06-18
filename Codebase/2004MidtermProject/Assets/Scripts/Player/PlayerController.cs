using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Tooltip("The speed at which the player moves. (Defaults to 3.5)")]
    [Range(0, 10)]
    [Space(10)]
    public float walkSpeed = 3.5f;
    [Tooltip("The multiplier to the players speed while sprinting. (Defaults to 2)")]
    [Range(0, 10)]
    public float sprintSpeedModifier = 2f;
    [Tooltip("The ammount of stamina sprinting requires (Defaults to 3)")]
    public float sprintStaminaCost = 3f;
    [Tooltip("The height that the player jumps. (Defaults to 3)")]
    [Range(0, 5)]
    [Space(10)]
    public float jumpHeight = 3f;
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
    public float maxBattery = 8f;
    [Tooltip("The players current amount of battery.")]
    [HideInInspector] public float currBattery;
    [Tooltip("The ammount of battery the flashlight requires (Defaults to 1)")]
    public float flashlightBatteryCost = 1f;
    [Tooltip("The ammount of battery the battery pickup refills (Defaults to 2)")]
    public float batteryChargeAmount = 2f;

    [Header("Other/Misc")]
    [HideInInspector] public bool inDeathAnim = false;
    [Space(10)]
    public GameObject flashlight = null;
    [SerializeField] MiniMap minimap = null;
    public ObjectiveTracker objective = null;
    [HideInInspector]public Animator anim = null;
    #endregion

    #region Private Variables
    Camera cam = null;//the players camera
    float gravity = 20;
    Vector3 moveDirection = Vector3.zero;
    Vector3 slideDirection = Vector3.zero;
    public bool hasKey = false;

    bool isSprinting = false;
    bool isSliding = false;

    [HideInInspector] public GameObject enemy = null;
    [HideInInspector] public Enemy enemyComp = null;
    [HideInInspector] public bool frozen = false;
    [HideInInspector] public bool hasShard = false;
    [HideInInspector] public bool UVFlashlight = false;
    [HideInInspector] public bool hasBlueLife = false;
    [HideInInspector] public bool firstTimeSeen = false;

    public static bool tutorialBattery = false;
    public static bool firstTimeIntro = false;
    [SerializeField] GameObject shards = null;
    public static bool enemySeen = false;

    #endregion

    private void Awake()
    {
        //singleton management
        if (Player == null) { Player = this; } else { if (Player != this) Debug.LogWarning("Multiple " + this + " in the scene. There should only be 1 player."); }
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
        anim = GetComponent<Animator>(); 
        frozen = false;
        inDeathAnim = false;

        if (!firstTimeIntro && SceneManager.GetActiveScene().name == "ProtoypeMilestoneScene")
        {
            SoundManager.Instance.PlayVoiceLine(12);
            firstTimeIntro = true;
        }
    }

    IEnumerator AlertMonster()
    {
        yield return new WaitForSeconds(3);
        SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.GhostBreath);
    }

    void Update()
    {
        if (GameState.gamePaused)
            return;

        if (!frozen)
        {
            MouseLook();
            Movement();
            //HeadBobbing
            if (cc.isGrounded)
            {
                if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.z) > 0 && isSprinting && !isSliding)
                {
                    timer += Time.deltaTime * walkingBobbingSpeed;
                    cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, cam.transform.localPosition.z);
                }
                else
                {
                    timer = 0;
                    cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, Mathf.Lerp(cam.transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), cam.transform.localPosition.z);

                    if (moveSound != null) { moveSound.Stop(); }
                }
            }
        }

        //stamina regen
        if (!isSprinting && !isSliding)
        {
            currStamina += staminaRegenRate * Time.deltaTime;
        }
        if (!canSprint && currStamina > 0.5f)
        {
            canSprint = true;
        }
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
                        if (!firstTimeSeen && SceneManager.GetActiveScene().name == "ProtoypeMilestoneScene")
                        {
                            if (hasKey)
                            {
                                SoundManager.Instance.PlayVoiceLine(15);
                            }
                            else
                            {
                                SoundManager.Instance.PlayVoiceLine(13);
                            }
                            firstTimeSeen = true;
                        }
                        else
                        {
                            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.effects[0], transform.position, 0.6f);
                            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.effects[2], transform.position);
                        }
                    }
                }
            }
        }
        else
        {
            if (enemySeen)
            {
                if (sightDelay == null)
                {
                    sightDelay = DelayedSightRefresh();
                    StartCoroutine(sightDelay);
                }
            }
            else
                enemySeen = false;
        }

        //Flashlight

        if (currBattery > 0 && inDeathAnim == false)
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
        if (isSliding)
        {
            if (slideTimer > 0)
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

        if (Vector3.Distance(enemy.transform.position, transform.position) < 15)
        {
            float distancemod = Vector3.Distance(enemy.transform.position, transform.position) * 0.25f;
            if (HeartbeatRoutine == null)
            {
                HeartbeatRoutine = Heartbeat(distancemod);
                StartCoroutine(HeartbeatRoutine);
            }
        }

        if (currStamina <= 0)
        {
            if (!SoundManager.Instance.BreathSound.isPlaying)
            {
                SoundManager.Instance.PlayBreath(SoundManager.Instance.OutOfBreath); 
            }
        }

        if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.z) > 0)
        {
            anim.SetBool("IsMoving", true);
        }
        else
            anim.SetBool("IsMoving", false);

    }
    IEnumerator HeartbeatRoutine;

    bool flashlightOn = false;
    void FlashLight(bool val)
    {
        if (inDeathAnim == false)
        {
            if (val == true)
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
        else if (inDeathAnim == true)
        {
            flashlight.SetActive(false);

        }
    }

    IEnumerator sightDelay;

    IEnumerator DelayedSightRefresh()
    {
        yield return new WaitForSeconds(10);
        enemySeen = false;
        //enemyComp.anim.ResetTrigger("Scare");
        sightDelay = null;
    }

    AudioSource moveSound;

    bool canSprint; //used for recovery time after emptying stamina
    void Movement()
    {
        if (cc.isGrounded)
        {
            if (Input.GetButton("Sprint") && canSprint)
            {
                if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.z) > 0)
                {
                    if (currStamina > 0)
                    {
                        isSprinting = true;
                        currStamina -= sprintStaminaCost * Time.deltaTime;
                        anim.SetFloat("Speed", 1); 
                    }
                    else { isSprinting = false; canSprint = false; anim.SetFloat("Speed", 0); }
                }
                else { isSprinting = false; moveSound.clip = null; anim.SetFloat("Speed", 0); }
            }
            else { isSprinting = false; anim.SetFloat("Speed", 0); }

            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed * (isSprinting ? sprintSpeedModifier : 1);

            if (Input.GetButtonDown("Jump"))
            {
                Jump(); 
            }

            if (Input.GetButtonDown("Slide"))
            {
                if (!isSliding)
                {
                    if (currStamina > 0)
                    {

                        isSliding = true;
                        slideTimer = slideDuration;
                        oldSpeed = walkSpeed;

                        slideDirection = new Vector3(0.0f, 0.0f, 1);
                        slideDirection = transform.TransformDirection(slideDirection);
                        slideDirection *= slideDistance / slideDuration;
                        currStamina -= slideStaminaCost;
                        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.PlayerSliding, transform.position, 2);
                    }
                }
            }

        }

        moveDirection.y -= gravity * Time.deltaTime;
        if (isSliding) { slideDirection.y -= gravity * 2 * Time.deltaTime; }

        cc.Move((isSliding ? slideDirection : moveDirection) * Time.deltaTime);

        if (!moveSoundPlaying)
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
        {
            moveSound.clip = RandomFootstep();
        }
        else
        {
            moveSound.clip = null;
        }

        moveSoundPlaying = true;
        float delay = isSprinting ? 0.3225f : 0.3805f;

        if (moveSound == null)
        {
            moveSound = GetComponent<AudioSource>();
        }
        moveSound.volume = 0.125f * SoundManager.EffectsVolume;

        if (cc.isGrounded && !isSliding)
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
        if (Mathf.Abs(cc.velocity.x) > 0 || Mathf.Abs(cc.velocity.z) > 0)
        {
            moveDirection += 0.75f * transform.forward;
        }
        moveDirection.y = jumpHeight * 2;
        anim.SetTrigger("Jump"); 
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

            if (!Physics.Raycast(transform.position, transform.up, 1f))
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

    [HideInInspector] public bool hitExit = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            SoundManager.Instance.PlayEffectAtPoint(other.GetComponent<AudioSource>().clip, transform.position, 0.25f);
            SoundManager.Instance.PlayVoiceLine(14);
            Destroy(other.gameObject);
            hasKey = true;
            objective.DisplayNewObjective("Get to the exit");
            minimap.ShowExit();
        }
        if (other.gameObject.CompareTag("Exit") && hasKey)
        {
            SoundManager.Instance.PlayEffectAtPoint(other.GetComponent<AudioSource>().clip, transform.position, 0.25f);
            other.GetComponent<Animation>().Play();
            hitExit = true;
            GameState.ShowWinMenu();
        }
        if (other.gameObject.CompareTag("TExit"))
        {
            SoundManager.Instance.PlayEffectAtPoint(other.GetComponent<AudioSource>().clip, transform.position, 0.25f);
            other.GetComponent<Animation>().Play();
            hitExit = true;
            GameState.ShowTutorialExit();
        }
        if (other.gameObject.CompareTag("Battery"))
        {
            if(tutorialBattery)
            {
                Destroy(other.gameObject);
                SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Battery, transform.position);
                currBattery = maxBattery;
                TutorialManager.PlayVoiceLine(5);
                tutorialBattery = false;
                StartCoroutine(FlashLightPrompt());
                objective.DisplayNewObjective(string.Empty);
            }
            if (currBattery < maxBattery)
            {
                Destroy(other.gameObject);
                currBattery += batteryChargeAmount;
                if (currBattery > maxBattery)
                    currBattery = maxBattery;
                SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Battery, transform.position);
            }
        }
        if (other.gameObject.CompareTag("Entrance"))
        {
            GameTimer.isCounting = true;
            StartCoroutine(AlertMonster());
        }

        if (other.gameObject.CompareTag("YellowShard"))
        {
            Destroy(shards);
            hasShard = true;
            Monolith.shardCharge = 1;
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Shard, transform.position);
        }
        if (other.gameObject.CompareTag("GreenShard"))
        {
            Destroy(shards);
            hasShard = true;
            Monolith.shardCharge = 2;
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Shard, transform.position);
        }
        if (other.gameObject.CompareTag("PurpleShard"))
        {
            Destroy(shards);
            hasShard = true;
            Monolith.shardCharge = 3;
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Shard, transform.position);
        }
        if (other.gameObject.CompareTag("BlueShard"))
        {
            Destroy(shards);
            hasShard = true;
            Monolith.shardCharge = 4;
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Shard, transform.position);
        }
        if (other.gameObject.CompareTag("Capsule"))
        {
            Destroy(other.gameObject);
            hasShard = true;
            Monolith.shardCharge = 5;
            SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.Shard, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance") && transform.position.z >= -29)
        {
            other.isTrigger = false;
        }
    }



    IEnumerator Heartbeat(float waitTime)
    {
        SoundManager.Instance.PlayHeartBeatEffect(SoundManager.Instance.Heartbeat);
        yield return new WaitForSeconds(waitTime);
        HeartbeatRoutine = null;
    }

    IEnumerator FlashLightPrompt()
    {
        while(SoundManager.Instance.VoiceLineSound.isPlaying)
        {
            yield return null;
        }
        DialogBox.ShowWindow("Flashlight", "Activate the flashlight by holding <color=yellow>Left Mouse Button</color>.", false);
    }
}
