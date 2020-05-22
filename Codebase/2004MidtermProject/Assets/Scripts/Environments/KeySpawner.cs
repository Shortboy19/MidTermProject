using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public GameObject Key;
    #region Spawn Points
    private int SpawnPoint;
    public Transform[] SpawnPoints;
    #endregion
    private void Start()
    {
        if (!GameObject.FindWithTag("Key"))
        {
            KeySpawn(Key);
        }
    }

    void KeySpawn(GameObject Key)
    {
        SpawnPoint = Random.Range(1, SpawnPoints.Length);
        Instantiate(Key, SpawnPoints[SpawnPoint].position, Quaternion.identity);
    }
}
