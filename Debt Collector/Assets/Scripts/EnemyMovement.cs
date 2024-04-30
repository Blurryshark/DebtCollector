using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    public float range = 15f;
    public string enemyTag = "PlayerMC";
    public Transform EnemyRotate;
    public float turnSpeed = 10f;
    public float moveSpeed = 5f;
    public float stopDistance = 2f; // Distance at which the enemy stops moving towards the player

    private bool hasHitPlayer = false;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        if (closestEnemy != null && closestDistance <= range)
        {
            target = closestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // If the player is within range, move towards it
        if (distance <= range)
        {
            // Rotate towards the target
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.RotateTowards(EnemyRotate.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
            EnemyRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            // If the distance to the target is greater than the stopping distance, move towards it
            if (distance > stopDistance)
            {
                transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
            }
            else if (!hasHitPlayer) // Check if the enemy has hit the player and hasn't already logged it
            {
                Debug.Log("Player is hit");
                hasHitPlayer = true; // Set hasHitPlayer to true to prevent duplicate logging
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag)) // Check if the collided object is the player capsule
        {
            Debug.Log("Player is hit");
        }
    }
}
