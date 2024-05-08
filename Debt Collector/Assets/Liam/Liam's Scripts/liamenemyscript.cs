using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class liamenemyscript : MonoBehaviour
{
    [Header("Detection")] 
    public float detectionRange = 25f; // set a specified range
    public NavMeshAgent enemy; // declare navMesh For enemy Ai
    public Transform Player; // declare player transform
    public float distanceToPlayer;

    [Header("Animation")]
    public Animator _animator;
    public String animatorString = "Speed";
    public String animatorIsAttacking = "isAttacking";
    public String animatorAttackType = "AttackType";
    public float velocity;
    public ParticleSystem moneyExplosion;

    [Header("Locomotion")] 
    public float moveSpeed = 13f;
    /*
       This will be how 'short' the enemy stops from thec player transform.
       Basically to stop them from being in the EXACT SAME SPOT as the player.
    */
    public float stopDistance;

    [Header("Attacking")] 
    public float maxPunchCooldown = 1f;
    public float maxKickCooldown = 1f;
    public float currAttackCooldown;
    public float maxPause = 1f;
    public float currPause;
    public bool isAttacking;
    public int attackType = -1;
    public GameObject punchHitbox;
    public GameObject kickHitbox;

    [Header("Health")] 
    public int currHealth;
    public int maxHealth = 1;

    [Header("Sounds")] public AudioSource deathSound;
    
    void Start()
    {
        stopDistance = enemy.stoppingDistance;
        enemy.speed = moveSpeed;
        isAttacking = false;
        currPause = maxPause;
        currHealth = maxHealth; 
    }

    void Update()
    {
        if (_animator.enabled == false)
        {
            return;
        }
        updateDistance();
        locomotion();
        attackManager();
        if (currHealth <= 0)
            death();
        
    }

    void death()
    {
        if (_animator.enabled = true)
        {
            _animator.enabled = false;
            deathSound.Play();
            moneyExplosion.Play();
        }
    }
    void updateDistance()
    {
        Vector3 enemyXZ = getXZVector(transform);
        Vector3 playerXZ = getXZVector(Player.transform);
                
        distanceToPlayer = Vector3.Distance(enemyXZ, playerXZ);
    }

    void attackManager()
    {
        if (!inRange())
        {
            return;
        }

        if (currPause > 0)
        {
            currPause -= Time.deltaTime;
        }
        else
        {
            if (currAttackCooldown > 0)
            {
                isAttacking = true;
                currAttackCooldown -= Time.deltaTime;
                if (currAttackCooldown <= 0)
                {
                    isAttacking = false;
                    attackType = -1;
                    currPause = maxPause;
                    return;
                }
            }
            else
            {
                isAttacking = true;
                attackType = getAttackType();
                if (attackType == 1)
                {
                    currAttackCooldown = maxPunchCooldown;
                }
                else
                {
                    currAttackCooldown = maxKickCooldown;
                }
            }
        }
        _animator.SetBool(animatorIsAttacking, isAttacking);
        _animator.SetInteger(animatorAttackType, attackType);
        hitboxManager();
        
    }

   

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            currHealth -= 1;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            currHealth -= 1;
        }
        
    }

    void hitboxManager()
    {
        if (!isAttacking)
        {
            punchHitbox.SetActive(false);
            kickHitbox.SetActive(false);
        } else if (attackType == 1)
        {
            punchHitbox.SetActive(true);
            kickHitbox.SetActive(false);
        } else if (attackType == 2)
        {
            punchHitbox.SetActive(false);
            kickHitbox.SetActive(true);
        }
    }
    int getAttackType()
    {
        return Random.Range(1, 3); //1-punch, 2-kick
    }
    void locomotion()
    {
        velocity = enemy.velocity.magnitude;
        _animator.SetFloat(animatorString, velocity);
        if (Player != null && detected())
        { 
            if (inRange())
            {
                enemy.ResetPath();
                //Debug.Log("attack!");
                //attack
                return;
            }
            enemy.SetDestination(Player.position);
        }        
    }
    private bool inRange()
    {
        if (distanceToPlayer <= stopDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool detected()
    {
        if (distanceToPlayer <= detectionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /*
     * Helper method to take transforms and get their horizontal representation. 
     */
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
