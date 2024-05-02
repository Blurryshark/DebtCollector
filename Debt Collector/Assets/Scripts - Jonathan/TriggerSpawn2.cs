using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawn2 : MonoBehaviour
{
    public GameObject enemy;
    public Transform enemyPos;
    private float repeatRate = 5.0f;

    void Start()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerMC")
        {
            Debug.Log("checking for player");
            InvokeRepeating("EnemySpawner", 0.5f, repeatRate);
            Destroy(gameObject, 11);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    void EnemySpawner()
    {
        Debug.Log("Enemy spawning...");
        Instantiate(enemy, enemyPos.position, enemyPos.rotation);
    }
}
