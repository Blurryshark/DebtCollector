using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemyMovement2 : MonoBehaviour
{
    [Header("Detection")] 
    public float detectionRange = 15f;

    public float distanceToPlayer;
    public Transform player;
    
    [Header("Animation")] 
    public Animator _animator;
    public String animatorSpeed = "Speed";
    private CharacterController controller;

    [Header("Movement")] 
    public NavMeshAgent enemy;
    public float speed = 15f;
    public float currSpeed;

    [Header("Drops")]
    public GameObject[] CollectableDrops;
    public int upperBoundOfDrops = 11;
    public int lowerBoundOfDrops = 1;

    void Start()
    {
        enemy.speed = speed;
        controller = GetComponent<CharacterController>();
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
        if(other.gameObject.tag == "Enemy")
        _animator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy"){
            _animator.enabled = false;
            CollectableDrop();
            controller.enabled = false;
        }
    }

    void CollectableDrop()
    {
        foreach(GameObject collectable in CollectableDrops)
        {
            int amount = Random.Range(lowerBoundOfDrops, upperBoundOfDrops);
            for (int j = 0; j< amount; j++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(Random.insideUnitCircle.x, 1f, Random.insideUnitCircle.y) * 0.8f;
                Instantiate(collectable, spawnPosition, Quaternion.identity);
            }
            
        }
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
