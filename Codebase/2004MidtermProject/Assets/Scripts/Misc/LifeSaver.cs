using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class LifeSaver : MonoBehaviour
{
    [SerializeField] GameObject savior;
    GameObject saviorOBJ;
    [SerializeField]Enemy enemy;
    [SerializeField]KeySpawner keySpawner;
    [SerializeField]MiniMap map;
    Camera playerCam;

    Light saviorLight;

    float speed = 1;
    private void Start()
    {
        playerCam = Camera.main;
    }
    public void SaveFromMonster()
    {
        StartCoroutine(PlaySaveAnim());
        PlayerController.Player.hasBlueLife = false;
    }
    
    IEnumerator PlaySaveAnim()
    {
        PlayerController.Player.frozen = true;
        enemy.agent.speed = 0;
        enemy.GetComponent<Collider>().enabled = false;
        Vector3 runPoint = enemy.PickSpawnPoint().position;
        enemy.anim.SetBool("Stunned", true);
        //enemy.agent.Warp(enemy.PickSpawnPoint().position);

        Vector3 spawn = PlayerController.Player.transform.position + playerCam.transform.forward * 1.5f;
        spawn.y = -0.15f;
        saviorOBJ = Instantiate(savior, spawn, Quaternion.LookRotation(-PlayerController.Player.transform.forward));
        saviorLight = saviorOBJ.GetComponentInChildren<Light>();
        spawn.y = 1.5f;

        while (speed < 1.15f)
        {
            Quaternion targetRot = Quaternion.LookRotation(spawn - playerCam.transform.position);
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed);
            speed += 0.05f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        saviorLight.range = 10;
        speed = 1;
        while(speed < 1.2f)
        {
            Quaternion targetRot = Quaternion.LookRotation(enemy.eyes.transform.position - playerCam.transform.position);
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed);

            Quaternion targetRot2 = Quaternion.LookRotation(enemy.transform.position - saviorOBJ.transform.position);
            targetRot2.x = targetRot2.z = 0;
            saviorOBJ.transform.rotation = Quaternion.Lerp(saviorOBJ.transform.rotation, targetRot2, Time.deltaTime * speed);

            speed += 0.05f * Time.deltaTime;

            if(speed > 1.05f)
            {
                enemy.agent.speed = enemy.oldSpeed;
                StartCoroutine(enemy.SaviorScare(runPoint));
            }

            yield return new WaitForEndOfFrame();
        }

        saviorLight.range = 6;
        speed = 1;
        while(speed < 1.15f)
        {
            Quaternion targetRot = Quaternion.LookRotation(spawn - playerCam.transform.position);
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, targetRot, Time.deltaTime * speed);

            Quaternion targetRot2 = Quaternion.LookRotation(PlayerController.Player.transform.position - saviorOBJ.transform.position);
            targetRot2.x = targetRot2.z = 0;
            saviorOBJ.transform.rotation = Quaternion.Lerp(saviorOBJ.transform.rotation, targetRot2, Time.deltaTime * speed);

            speed += 0.05f * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        enemy.agent.Warp(runPoint);
        Destroy(saviorOBJ);
        yield return new WaitForEndOfFrame();

        PlayerController.Player.hasKey = false;
        keySpawner.Start();
        map.ShowKey();

        PlayerController.Player.frozen = false;
        DialogBox.ShowWindow("You've Been Saved", "A savior has <color=#00D6FF>protected</color> you from death. In doing so you have <color=red>dropped</color> the key. You'll have to go back for it. \n\nBe carful, as you will not be saved again.");

        PlayerController.Player.objective.DisplayOldObjective("Find the key");
        enemy.scared = false;
    }
}
