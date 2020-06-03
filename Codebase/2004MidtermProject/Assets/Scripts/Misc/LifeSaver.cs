using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LifeSaver : MonoBehaviour
{
    [SerializeField] GameObject savior;
    GameObject saviorOBJ;
    [SerializeField]Enemy enemy;
    [SerializeField]KeySpawner keySpawner;
    [SerializeField]MiniMap map;
    Camera playerCam;

    bool saved = false;
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
        enemy.agent.Warp(enemy.PickSpawnPoint().position);

        Vector3 spawn = PlayerController.Player.transform.position + playerCam.transform.forward * 2.25f;
        spawn.y = -0.15f;
        saviorOBJ = Instantiate(savior, spawn, Quaternion.LookRotation(-PlayerController.Player.transform.forward));
        saved = true;

        yield return new WaitForSeconds(2.5f);

        PlayerController.Player.hasKey = false;
        keySpawner.Start();
        map.ShowKey();
        PlayerController.Player.frozen = false;
    }
}
