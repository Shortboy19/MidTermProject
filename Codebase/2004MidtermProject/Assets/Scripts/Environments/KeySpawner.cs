using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public GameObject Key = null;
    [SerializeField] GameObject shard = null;
    [SerializeField] ObjectiveTracker objective = null;
    #region Spawn Points
    private int SpawnPoint = -1;
    public Transform[] SpawnPoints = null;
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
        SpawnPoint = Random.Range(0, SpawnPoints.Length);
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
