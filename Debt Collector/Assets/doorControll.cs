using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject textHint;  // Assign the UI Text element in the inspector
    public float triggerDistance = 3.0f;  // Distance within which the player can trigger the door
    private Animator anim;
    private bool doorIsOpen = false;
    private Transform playerTransform;  // To store player's transform

    void Start()
    {
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // Make sure your player has the tag "Player"
    }

    void Update()
    {
        // Check distance between the player and the door
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance < triggerDistance)
        {
            // Show hint text when player is within the trigger distance
            textHint.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (doorIsOpen)
                {
                    anim.SetTrigger("close");
                    doorIsOpen = false;
                }
                else
                {
                    anim.SetTrigger("open");
                    doorIsOpen = true;
                }
            }
        }
        else
        {
            // Hide hint text when player is not within the trigger distance
            textHint.SetActive(false);
        }
    }
}