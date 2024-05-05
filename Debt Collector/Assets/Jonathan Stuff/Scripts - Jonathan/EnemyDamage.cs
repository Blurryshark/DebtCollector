using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMC"))
        {
            PlayerMCStats playerStats = other.GetComponent<PlayerMCStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
                Debug.Log("Player took " + damageAmount + " damage.");
            }
        }
    }
}
