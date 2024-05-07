using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 10;
    public int currHealth;
    public Animator _animator;
    public bool isInvincible;

    private void Start()
    {
        currHealth = maxHealth;
    }

    private void Update()
    {
        isInvincible = GetComponent<ThirdPersonMovement>().isDodging;
        CheckHealth();
    }
    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.CompareTag("Player"))
        {
            if (!isInvincible)
            {
                TakeDamage();
            }
        }
    }

    private void TakeDamage()
    {
        currHealth -= 1;
        //Debug.Log("Taaking damage!");
    }

    private void CheckHealth()
    {
        if (currHealth <= 0)
        {
            _animator.enabled = false;
        }
    }
}
