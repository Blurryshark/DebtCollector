using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public NavMeshAgent enemy;            // declare navMesh For enemy Ai
    public Transform Player;              // declare player transform
    public float range = 25f;             // set a specified range
    public string playerTag = "player";   // set player tag as a vaiable

    void Start()
    {
        InvokeRepeating("UpdateTarget",0f,.5f);   // call UpdateTarget function at start of script every half second
    }

    void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);  // find all potential players within game
        
        float shortestDiatance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players) // using for loop for possible multiplayer functionalty, finding all players in game
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); // return distance from enemies to player

            if(distanceToPlayer <shortestDiatance)
            {
                shortestDiatance = distanceToPlayer;
                closestPlayer = player;
            }
        }

        if(closestPlayer != null && shortestDiatance <= range)
        {
            Player = closestPlayer.transform;
        }
    }

    void Update()
    {
        if (Player != null && Vector3.Distance(transform.position, Player.position) <= range)
        {
            enemy.SetDestination(Player.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
