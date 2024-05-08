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
    [Header("Animations")] 
    public Animator _animator;
    [HideInInspector]public String animatorSpeed = "Speed";
    [HideInInspector]public String animatorIsAttacking = "isAttacking";
    [HideInInspector]public String animatorAttackType = "AttackType";
    [HideInInspector]public String animatorRolling = "Rolling";
    
    [Header("Scene Items")]
    public CharacterController controller;
    public Transform cam;
    
    [Header("Speed Items")]
    public float currSpeed;
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float acceleration = 1.0f;
    public float decceleration = 1.0f;
    public float sprintDecceleration = 2.0f;
    [SerializeField] public float jumpPower;
    [SerializeField] private Movement movement;
    public Vector3 moveDir = new Vector3();

    [Header("Dodging")] 
    public float dodgeSpeed = 12f;
    public float backstepSpeed = 3f;
    public bool backstep;
    public Vector3 dodgeDirection;
    public float backstepMaxCooldown = 0.5f;
    public float dodgeMaxCooldown = 1f;
    public float actCooldown;
    public bool isDodging;

    [Header("Attacking")] 
    public bool isAttacking;
    public int attackType; //0 = light attack, 1 = heavy attack
    public float maxAttackCooldown;
    public float maxHeavyAttackCooldown;
    public float attackCooldown;
    public GameObject punchHitbox;
    public GameObject kickHitbox;
    
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
    //public Transform transform;
   

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isGrounded = controller.isGrounded;
        //transform = GetComponent<Transform>();
        isAttacking = false;
        isDodging = false;
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
        if (_animator.enabled == true)
        {
            Motivate();
            dodgeManager(); 
            attackManager();
        }
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
        
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            if (Input.GetButton("Sprint"))
                maxSpeed = sprintSpeed;
            else
                maxSpeed = speed;

            if (currSpeed > maxSpeed)
            {
                currSpeed -= Time.deltaTime * sprintDecceleration;
                direction *= currSpeed;
                currSpeed = Mathf.Clamp(currSpeed, 0f, sprintSpeed);
            }
            else
            {
                currSpeed += maxSpeed * Time.deltaTime * acceleration; 
                direction *= currSpeed;
                currSpeed = Mathf.Clamp(currSpeed, 0f, maxSpeed);
            }
        }
        else
        {
            currSpeed -= Time.deltaTime * decceleration;
            direction *= currSpeed;
            currSpeed = Mathf.Clamp(currSpeed, 0f, sprintSpeed);
        }

        _animator.SetFloat(animatorSpeed, currSpeed);
        return currSpeed;
    }

    private void attackManager()
    {
        if (Input.GetButton("Attack") && !isAttacking)
        {
            attackType = 0;
            attackCooldown = maxAttackCooldown;
        } else if (Input.GetButton("Heavy Attack") && !isAttacking)
        {
            attackType = 1;
            attackCooldown = maxHeavyAttackCooldown;
        }

        if (attackCooldown > 0)
        {
            isAttacking = true;
            attackCooldown -= Time.deltaTime;
        }
        else
        {
            attackType = -1;
            isAttacking = false;
        }
        
        _animator.SetBool(animatorIsAttacking, isAttacking);
        _animator.SetInteger(animatorAttackType, attackType);

        if (isAttacking)
        {
            if (attackType == 0)
            {
                punchHitbox.SetActive(true);
            } else if (attackType == 1)
            {
                kickHitbox.SetActive(true);
            }
        }
        else
        {
            punchHitbox.SetActive(false);
            kickHitbox.SetActive(false);
        }
    }
    private void dodgeManager()
    {
        if (Input.GetButton("Dodge") && !isDodging)
        {
            setDodgeType(); 
            setDodgeDirection();
            if (backstep)
                actCooldown = backstepMaxCooldown;
            else 
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
        _animator.SetBool(animatorRolling, isDodging);
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
