using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JonMovementController : MonoBehaviour
{

    public float acceleration = 10f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 50f;
    public float jumpBoost = 3f;
    public bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Rigidbody rbody = GetComponent<Rigidbody>();

        // Check if the Shift key is held down
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Set the desired max speed and acceleration based on sprinting or not
        float sprintMaxSpeed = isSprinting ? 10f : 5f;
        float sprintAcceleration = isSprinting ? acceleration * 2f : acceleration;

        // Apply movement
        rbody.velocity += Vector3.right * horizontalMovement * Time.deltaTime * sprintAcceleration;

        Collider col = GetComponent<Collider>();
        float halfHeight = col.bounds.extents.y + 0.03f;

        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + Vector3.down * halfHeight;

        isGrounded = Physics.Raycast(startPoint, Vector3.down, halfHeight);
        Color lineColor = (isGrounded) ? Color.red : Color.blue;
        Debug.DrawLine(startPoint, endPoint, lineColor, 0f, false);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rbody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
        }
        else if (!isGrounded && Input.GetKey(KeyCode.Space))
        {
            rbody.AddForce(Vector3.up * jumpBoost, ForceMode.Force);
        }

        // Clamp velocity to max speed
        if (Mathf.Abs(rbody.velocity.x) > sprintMaxSpeed)
        {
            Vector3 newVel = rbody.velocity;
            newVel.x = Mathf.Clamp(newVel.x, -sprintMaxSpeed, sprintMaxSpeed);
            rbody.velocity = newVel;
        }

        // Slow down gradually if grounded and no input
        if (isGrounded && Mathf.Abs(horizontalMovement) < .5f)
        {
            Vector3 newVel = rbody.velocity;
            newVel.x *= 1f - Time.deltaTime;
            rbody.velocity = newVel;
        }

        float yaw = (rbody.velocity.x > 0) ? 90 : -90;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        //float speed = rbody.velocity.x;
        //Animator anim = GetComponent<Animator>();
        //anim.SetFloat("Speed", Mathf.Abs(speed));
        //anim.SetBool("In Air", !isGrounded);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Flag"))
    //    {
    //        Debug.Log("You win!");
    //    }
    //}

}