using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float currSpeed;
    public float maxSpeed = 6f;
    public float acceleration = 1.0f;
    public float decceleration = 1.0f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currSpeed = 0f;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); 
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            currSpeed += maxSpeed * Time.deltaTime * acceleration;
            direction *= currSpeed;
            currSpeed = Mathf.Clamp(currSpeed, 0f, maxSpeed);
        }
        else
        {
            currSpeed -= Time.deltaTime * decceleration;
            direction *= currSpeed;
            currSpeed = Mathf.Clamp(currSpeed, 0f, maxSpeed);
        }
        
        move(direction, currSpeed);
    }

    private void move(Vector3 direction, float speed)
    {
        if (direction.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
