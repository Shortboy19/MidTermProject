using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public GameObject Key;
    [SerializeField] GameObject shard;
    [SerializeField] ObjectiveTracker objective;
    #region Spawn Points
    private int SpawnPoint;
    public Transform[] SpawnPoints;
    #endregion
    public void Start()
    {
        if (!GameObject.FindWithTag("Key"))
        {
            KeySpawn(Key);
        }
        objective.DisplayNewObjective("Find the key");
    }

    void KeySpawn(GameObject Key)
    {
        SpawnPoint = Random.Range(1, SpawnPoints.Length);
        Instantiate(Key, SpawnPoints[SpawnPoint].position, Quaternion.identity);
    }

    private void Update()
    {
        if (shard == null)
            return;

        if(PlayerController.Player.hasKey)
        {
            Destroy(shard);
        }
    }
}
