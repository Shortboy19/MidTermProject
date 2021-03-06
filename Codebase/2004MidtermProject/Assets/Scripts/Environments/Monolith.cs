using System.Collections;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    public static int shardCharge = 0;
    float rotSpeed = 0.035f;
    [Range(0, 1)]
    [SerializeField] float floatAmount = 0.05f;
    [SerializeField] AudioSource Whispers = null;

    [SerializeField] GameObject textObj = null;
    [SerializeField] GameObject trailMaker = null;
    Light[] lights = null;

    bool playerCanActivate = false;
    float floatSpeed = 0;
    Vector3 floatVec = Vector3.zero;

    void Start()
    {
        textObj.SetActive(false);
        lights = GetComponentsInChildren<Light>();
        floatVec = transform.position;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (playerCanActivate)
                Activate();
        }

        transform.Rotate(new Vector3(0, rotSpeed, 0));

        Bounce();

        if (GameState.gamePaused)
        {
            Whispers.volume = 0;
        }
        else
        {
            Whispers.volume = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.Player.hasShard)
            {
                textObj.SetActive(true);
                playerCanActivate = true;
            }
            //Whispers.volume = 1;
            //Whispers.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObj.SetActive(false);
            playerCanActivate = false;
            //Whispers.volume = 0;
            //Whispers.Stop();
        }
    }

    void Activate()
    {
        StartCoroutine(Spin());
        textObj.SetActive(false);

        //activate shard here
        switch (shardCharge)
        {
            case 1:
                YellowShard();
                break;
            case 2:
                GreenShard();
                break;
            case 3:
                PurpleShard();
                break;
            case 4:
                BlueShard();
                break;
            case 5:
                CapsuleMode();
                break;
            default:
                //Do nothing
                break;
        }
    }

    void Bounce()
    {
        transform.position = Vector3.Lerp(floatVec, new Vector3(0, floatVec.y * (1 + floatAmount), 0), floatSpeed);

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
        };
    }

    void YellowShard()
    {
        if (!PlayerController.Player.hasKey)
        {
            DialogBox.ShowWindow("Unavailable", "You need the <color=yellow>key</color> to activate this shard. \n\nHeads towards the <color=yellow>indicator</color> on you minimap to find it.");
            return;
        }

        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);
        string message = "You just activated the <color=yellow>Yellow Monolith Shard</color>. Your <color=yellow>minimap</color> will now display the quickest possible path to the exit. \n\nBut be warned, the monster has now become <color=red>aggrivated</color> and will chase after you at accelerated speeds.";
        DialogBox.ShowWindow("Shard Activated", message);
        //put trail activation here
        trailMaker.SetActive(true);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.yellow;
        }
        Enemy enemy = FindObjectOfType<Enemy>();
        StartCoroutine(YellowShardDelayedWarp(enemy));
        enemy.agent.speed = 6.75f;
        enemy.oldSpeed = 6.75f;
        enemy.agent.angularSpeed = 180;
        playerCanActivate = false;
        PlayerController.Player.hasShard = false;
    }

    IEnumerator YellowShardDelayedWarp(Enemy enemy)
    {
        yield return new WaitForSeconds(1.5f);
        enemy.agent.Warp(Vector3.zero);
    }

    void GreenShard()
    {
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);
        string message = "You just activated the <color=green>Green Monolith Shard</color>. Your stamina and battery life have just been <color=green>doubled</color> and <color=green>refilled</color>. \n\nBut be warned, your stamina will now recover at <color=red>half</color> the rate it used to and batteries have become <color=red>less</color> effective.";
        DialogBox.ShowWindow("Shard Activated", message);
        //put trail activation here
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.green;
        }

        PlayerController.Player.maxStamina *= 2;
        PlayerController.Player.maxBattery *= 2;

        PlayerController.Player.currStamina = PlayerController.Player.maxStamina;
        PlayerController.Player.currBattery = PlayerController.Player.maxBattery;

        PlayerController.Player.staminaRegenRate *= 0.5f;
        playerCanActivate = false;
        PlayerController.Player.hasShard = false;
    }

    void PurpleShard()
    {
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);
        string message = "You just activated the <color=#9700FF>Purple Monolith Shard</color>. Your flashlight now emits <color=#9700FF>UV</color> light that will scare the monster instead of stunning it. \n\nBut be warned, your flashlight will now drain battery at <color=red>twice</color> the rate it used to.";
        DialogBox.ShowWindow("Shard Activated", message);
        //put trail activation here
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = new Color(1, 0, 1);
        }

        PlayerController.Player.flashlight.GetComponentInChildren<Light>().color = new Color(0.9f, 0, 0.9f);

        PlayerController.Player.UVFlashlight = true;
        PlayerController.tutorialBattery = false;
        playerCanActivate = false;
        PlayerController.Player.hasShard = false;
    }

    void BlueShard()
    {
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);
        string message = "You just activated the <color=#00D6FF>Blue Monolith Shard</color>. You now have a <color=#00D6FF>savior</color> watching over you that will <color=#00D6FF>protect</color> you from the monster's gaze. \n\nBut be warned, it can only protect you <color=red>once</color> and doing so will cause you to <color=red>drop</color> the key and have to go back for it.";
        DialogBox.ShowWindow("Shard Activated", message);
        //put trail activation here
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.cyan;
        }

        PlayerController.Player.hasBlueLife = true;
        playerCanActivate = false;
        PlayerController.Player.hasShard = false;
    }

    [SerializeField] GameObject capsule = null;
    void CapsuleMode()
    {
        capsule.SetActive(true);
    }
}
