using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Scene Items")]
    public CharacterController controller;
    public Transform cam;
    
    [Header("Speed Items")]
    public float currSpeed;
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float acceleration = 1.0f;
    public float decceleration = 1.0f;
    [SerializeField] public float jumpPower;
    [SerializeField] private Movement movement;
    public Vector3 moveDir = new Vector3();

    [Header("Dodging")] 
    public float dodgeSpeed = 12f;
    public float backstepSpeed = 3f;
    public bool backstep;
    public Vector3 dodgeDirection;
    public float dodgeMaxCooldown = 1f;
    public float actCooldown;
    public bool isDodging;
    
    [Header("Turning")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public Vector3 direction;
    
    [Header("Gravity Things")]
    public float _gravity = -9.81f;
    public float _gravityMultiplyer = 3.0f;
    public float _velocity;
    public bool isGrounded;
    
    [Header("----------")]
    private Vector3 _input;
    public float raycastLength = 1.1f;
    public Transform transform;
   

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isGrounded = controller.isGrounded;
        transform = GetComponent<Transform>();
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastLength);
    }

    private void Update()
    {
        Motivate();
        dodgeManager();
    }

    void Motivate()
    {
        applyGravity();
        getRotation();
    }

    private void getRotation()
    {
        if (direction.magnitude >= 0.1)
        { 
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
        }
        moveDir.y = _velocity; 
        applyMovement(moveDir,getSpeed());
    }
    private void applyMovement(Vector3 moveDir, float movingSpeed)
    { 
        controller.Move(moveDir * movingSpeed * Time.deltaTime);
    }

    private float getSpeed()
    {
        float maxSpeed;
        if (movement.isSprinting)
            maxSpeed = sprintSpeed;
        else
            maxSpeed = speed;
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            if (Input.GetButton("Sprint"))
                maxSpeed = sprintSpeed;
            
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

        return currSpeed;
    }
    private void dodgeManager()
    {
        
        
        if (Input.GetButton("Dodge"))
        {
            setDodgeType(); 
            setDodgeDirection();
            actCooldown = dodgeMaxCooldown;
        }
            
        if (actCooldown > 0)
        {
            isDodging = true;
            if (backstep)
            {
                controller.Move(dodgeDirection * backstepSpeed * Time.deltaTime);
            }
            else
            {
                controller.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);
            }
            
            actCooldown -= Time.deltaTime;
        }
        else
        {
            isDodging = false;
        }
    }
    private void setDodgeType()
    {
        if (direction.magnitude < 0.1f) backstep = true;
        else backstep = false;
    }
    private void setDodgeDirection()
    {
        if (backstep) dodgeDirection = transform.forward * -1.0f;
        else dodgeDirection = moveDir;
    }
    
    void applyGravity()
    {
        if (isGrounded && _velocity < 0.0f) 
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * _gravityMultiplyer * Time.deltaTime;
        }
    }
    public void move(InputAction.CallbackContext context) //called everytime there is an input from the InputManager
    {
        _input = context.ReadValue<Vector3>();
        direction = new Vector3(_input.x, 0f, _input.y).normalized;
    }
    public void jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isGrounded) return;

        _velocity += jumpPower;
    }

    public void sprint(InputAction.CallbackContext context)
    {
        movement.isSprinting = context.started || context.performed;
    }

    public void dodge(InputAction.CallbackContext context)
    {
        if (!isGrounded) return; //no dodging while falling
        if (isDodging) return; //no dodging is already dodging

        //isDodging = context.started || context.performed;
    }
}

[Serializable]
public struct Movement
{
    [HideInInspector] public bool isSprinting;
     
}
