using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject[] CollectableDrops;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            CollectableDrop();
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
        Destroy(gameObject);
    }
    private void CollectableDrop()
    {
        for(int i = 0; i < CollectableDrops.Length; i++)
        {
            int amount = Random.Range(1, 11);
            for (int j = 0; j< amount; j++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(Random.insideUnitCircle.x, 0f, Random.insideUnitCircle.y) * 0.8f;
                Instantiate(CollectableDrops[i], spawnPosition, Quaternion.identity);
            }
            
        }
    }
}
