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
            
            gameObject.GetComponent<Light>().enabled = true;
            
            enemyObj = other.gameObject.GetComponent<Enemy>();
            enemyObj.agent.speed = 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!GetComponentInParent<Trap>().armed)
            {
                return;
            }
            gameObject.GetComponent<Light>().enabled = false;
            enemyObj.agent.speed = enemyObj.oldSpeed;
        }
    }
}
