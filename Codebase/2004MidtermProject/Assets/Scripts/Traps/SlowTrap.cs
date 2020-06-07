using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    Enemy enemyObj;
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!GetComponentInParent<Trap>().armed)
            {
                return;
            }
            enemyObj = other.gameObject.GetComponent<Enemy>();
            enemyObj.agent.speed = 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {          
            enemyObj.agent.speed = enemyObj.oldSpeed;
        }
    }
}
