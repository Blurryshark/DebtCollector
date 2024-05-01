using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement2 : MonoBehaviour
{
    private Transform target;
    public float range = 15f;

    public string enemyTag = "PlayerMC";

    public Transform EnemyRotate;

    public float turnSpeed = 10f;
    public float moveSpeed = 5f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float furthestDistance = 0f;
        GameObject furthestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > furthestDistance)
            {
                furthestDistance = distanceToEnemy;
                furthestEnemy = enemy;
            }
        }
        if (furthestEnemy != null && furthestDistance <= range)
        {
            target = furthestEnemy.transform;
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

        Vector3 dir = transform.position - target.position; // Reverse direction to move away
        float distance = dir.magnitude;

        dir.y = 0f;

        // If the player is within range, move away from them
        if (distance <= range)
        {
            // Rotate away from the target
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(EnemyRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            EnemyRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            // Move away from the target
            transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
