using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTestScript : MonoBehaviour
{
    public int maxHealth = 10;
    public int currHealth;
    public Animator _animator;
    
    private void Start()
    {
        currHealth = maxHealth;
    }

    private void Update()
    {
        CheckHealth();
    }
    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            TakeDamage();
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
