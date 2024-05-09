using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorControl : MonoBehaviour
{
    public GameObject textHint;  
    public float triggerDistance = 3.0f;  
    private Animator anim;
    private bool doorIsOpen = false;
    private Transform playerTransform;  

    void Start()
    {
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Check distance between the player and the door
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance < triggerDistance)
        {
            // Show hint text when player is within the distance
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
            // Hide hint text
            textHint.SetActive(false);
        }
    }
}
