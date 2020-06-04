using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    public static int shardCharge = 0;
    float rotSpeed = 0.035f;
    [Range(0, 1)]
    [SerializeField] float floatAmount = 0.05f;

    [SerializeField] GameObject textObj;
    [SerializeField] GameObject trailMaker;
    Light[] lights;

    bool playerCanActivate = false;
    float floatSpeed = 0;
    Vector3 floatVec;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(PlayerController.Player.hasShard && PlayerController.Player.hasKey)
            {
                textObj.SetActive(true);
                playerCanActivate = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObj.SetActive(false);
            playerCanActivate = false;
        }
    }

    void Activate()
    {
        StartCoroutine(Spin());
        textObj.SetActive(false);
        playerCanActivate = false;
        PlayerController.Player.hasShard = false;

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
            default:
                //Do nothing
                break;
        }
    }

    void Bounce()
    {
        transform.position = Vector3.Lerp(floatVec, new Vector3(0, floatVec.y * (1+floatAmount), 0), floatSpeed);

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

    void YellowShard()
    {
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
        enemy.agent.speed = 6.25f;
        enemy.oldSpeed = 6.25f;
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

        PlayerController.Player.currStamina *= PlayerController.Player.maxStamina;
        PlayerController.Player.currBattery *= PlayerController.Player.maxBattery;

        PlayerController.Player.staminaRegenRate *= 0.5f;
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
    }

    void BlueShard()
    {
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.MonolithActivate, PlayerController.Player.transform.position);
        string message = "You just activated the <color=#00D6FF>Blue Monolith Shard</color>. You now have a <color=#00D6FF>savior</color> watching over you that will <color=#00D6FF>protect</color> you from death. \n\nBut be warned, it can only protect you <color=red>once</color> and doing so will cause you to <color=red>lose</color> the key and have to go back for it.";
        DialogBox.ShowWindow("Shard Activated", message);
        //put trail activation here
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.cyan;
        }

        PlayerController.Player.hasBlueLife = true;
    }
}
