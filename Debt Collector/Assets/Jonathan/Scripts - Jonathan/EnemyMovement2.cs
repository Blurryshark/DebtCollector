using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement2 : MonoBehaviour
{
    [Header("Detection")] 
    public float detectionRange = 15f;

    public float distanceToPlayer;
    public Transform player;
    
    [Header("Animation")] 
    public Animator _animator;
    public String animatorSpeed = "Speed";

    [Header("Movement")] 
    public NavMeshAgent enemy;
    public float speed = 15f;
    public float currSpeed;

    [Header("Sounds")] public AudioSource deathSound;

    void Start()
    {
        enemy.speed = speed;
    }

    void Update()
    {
        if (_animator.enabled == false)
        {
            return;
        }
        updateDistance();
        locomotion();
    }
    private void OnTriggerStay(Collider other)
    {
        deathSound.Play();
        _animator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        deathSound.Play();
        _animator.enabled = false;
    }
    void locomotion()
    {
        currSpeed = enemy.velocity.magnitude;
        if (distanceToPlayer <= detectionRange)
        { 
            Vector3 directionToPlayer = transform.position - player.transform.position; 
            Vector3 directionToRun = transform.position + directionToPlayer;
            enemy.SetDestination(directionToRun);
        }

        _animator.SetFloat(animatorSpeed, currSpeed);
    }
    public void updateDistance()
    {
        Vector3 enemyXZ = getXZVector(transform);
        Vector3 playerXZ = getXZVector(player.transform);
                
        distanceToPlayer = Vector3.Distance(enemyXZ, playerXZ);
    }
    private Vector3 getXZVector(Transform input)
    {
        Vector3 pos = input.position;
        return new Vector3(pos.x, 0, pos.z);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
