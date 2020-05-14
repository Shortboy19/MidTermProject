using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    float delay;
    public float cooldown = 3;

    [SerializeField] GameObject area;
    public bool playerInArea = false;
    bool refreshing = false;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        delay = cooldown;
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInArea && !refreshing)
        {
            delay -= Time.deltaTime;
            if(delay <= 0 && !refreshing)
            {
                refreshing = true;
                delay = 0;
            }
        }


        if (refreshing)
        {
            area.SetActive(false);
            enemy.agent.speed = enemy.oldSpeed;
            enemy.inMonolith = false;
            delay += Time.deltaTime;
            if ( delay >= cooldown)
            {
                refreshing = false;
                delay = cooldown;
                area.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInArea = false;
        }
    }
}
