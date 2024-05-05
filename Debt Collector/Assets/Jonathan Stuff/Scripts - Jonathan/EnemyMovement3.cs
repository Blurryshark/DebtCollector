using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement3 : MonoBehaviour
{
    private Transform target;
    public float range = 15f;
    public string enemyTag = "PlayerMC";
    public Transform EnemyRotate;
    public float turnSpeed = 10f;
    public float moveSpeed = 5f;
    public float stopDistance = 2f;

    private bool hasHitPlayer = false;
    private bool hasCharged = false;

    Animator animator;
    BoxCollider boxCollider;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
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
        dir.y = 0;

        if (distance <= range)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.RotateTowards(EnemyRotate.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
            EnemyRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (distance > stopDistance)
            {
                transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
                hasCharged = false;
            }
            else if (!hasHitPlayer && !hasCharged)
            {
                Debug.Log("Sphere Attacked");
                hasHitPlayer = true;
                attack();
                hasCharged = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void attack()
    {
        animator.SetTrigger("Attack");
    }
    void EnableAttack()
    {
        Debug.Log("Collider enabled.");
        boxCollider.enabled = true;
    }
    void DisableAttack()
    {
        Debug.Log("Collider disabled.");
        boxCollider.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMC"))
        {
            Debug.Log("Player is hit");
        }
    }


}
