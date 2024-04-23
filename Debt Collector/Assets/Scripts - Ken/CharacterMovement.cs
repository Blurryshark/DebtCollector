using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float acceleration = 125f;
    public float maxSpeed = 8f;
    public float jumpImpulse = 25f;
    public float jumpBoost = 6f;
    public bool isGrounded;
    public float moveSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Rigidbody rbody = GetComponent<Rigidbody>();
        rbody.velocity += Vector3.right * horizontalMovement * Time.deltaTime * acceleration;

        Collider col = GetComponent<Collider>();
        float halfHeight = col.bounds.extents.y + 0.03f;

        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + Vector3.down * halfHeight;

        isGrounded = Physics.Raycast(startPoint, Vector3.down, halfHeight);
        Color lineColor = (isGrounded) ? Color.red : Color.blue;
        Debug.DrawLine(startPoint, endPoint, lineColor, 0f, false);

        if(isGrounded && Input.GetKeyDown(KeyCode.Space)){
            rbody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
        } else if(!isGrounded && Input.GetKeyDown(KeyCode.Space)){
            if(rbody.velocity.y > 0){
                rbody.AddForce(Vector3.up * jumpBoost, ForceMode.Force);
            }
        }

        if(Math.Abs(rbody.velocity.x) > maxSpeed){
            Vector3 newVel = rbody.velocity;
            newVel.x = Math.Clamp(newVel.x, -maxSpeed, maxSpeed);
            rbody.velocity = newVel;
        }

        if(isGrounded && Math.Abs(horizontalMovement) < 0.5f){
            Vector3 newVel = rbody.velocity;
            newVel.x *= 0.9f;
            rbody.velocity = newVel;
        }

        float yaw = (rbody.velocity.x > 0) ? 90 : -90;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        float speed = Math.Abs(rbody.velocity.x);
    }

}
