using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public GameObject Key;
    #region Spawn Points
    private int SpawnPoint;
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;
    public Transform SpawnPoint4;
    #endregion
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!GameObject.FindWithTag("Key"))
            {
                SpawnPoint = Random.Range(1, 5);
                switch (SpawnPoint)
                {
                    case 1:
                        Instantiate(Key, SpawnPoint1.position, SpawnPoint1.rotation);
                        break;
                    case 2:
                        Instantiate(Key, SpawnPoint2.position, SpawnPoint2.rotation);
                        break;
                    case 3:
                        Instantiate(Key, SpawnPoint3.position, SpawnPoint3.rotation);
                        break;
                    case 4:
                        Instantiate(Key, SpawnPoint4.position, SpawnPoint4.rotation);
                        break;
                }
            }
        }
    }
}
