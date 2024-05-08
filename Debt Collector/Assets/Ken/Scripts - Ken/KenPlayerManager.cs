using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KenPlayerManager : MonoBehaviour
{
    public int maxHealth = 100;
    public HealthBar healthBar;

    public int currentHealth;
    private bool isSpedUp;
    private ThirdPersonMovement thirdPersonMovement;
    private UIManager uiManager;
    private ParticleSystem  speedParticle;
    private ParticleSystem  healthParticle;
    private ParticleSystem  deathParticle;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isSpedUp = false;
        thirdPersonMovement = GetComponent<ThirdPersonMovement>();
        uiManager = GetComponent<UIManager>();
        healthBar.SliderMaxHealth(maxHealth);
        speedParticle = transform.Find("Speed Particle Effect").GetComponent<ParticleSystem>();
        healthParticle = transform.Find("Health Particle Effect").GetComponent<ParticleSystem>();
        deathParticle = transform.Find("Player Death Effect").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(25);
        }

        if (currentHealth <= 0)
        {
            die();
        }
    }
    private void die()
    {
        if (thirdPersonMovement._animator.enabled == false)
        {
            return;
        }
        deathParticle.Play();
        thirdPersonMovement._animator.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!thirdPersonMovement.isDodging)
            {
                TakeDamage(25);
            }
        }
        // Check if the other is a "SpeedUp"
        if (other.CompareTag("SpeedUp")){
            Debug.Log("Player Speed Up");
            Destroy(other.gameObject);
            if(!isSpedUp){
                StartCoroutine(IncreaseSpeed());
            }

        // Check if the other is a "AttackUp"
        } else if(other.CompareTag("AttackUp")){
            Debug.Log("Player Attack Up");
            Destroy(other.gameObject);
            
        // Check if the other is a "HealthUp"
        } else if(other.CompareTag("HealthUp")){
            Debug.Log("Player Health Up");
            Destroy(other.gameObject);
            if(currentHealth < maxHealth){
                Heal(10);
            }
        }
    }

    IEnumerator IncreaseSpeed()
    {
        Debug.Log("Up the Speed Here!");

        isSpedUp = true;
        thirdPersonMovement.speed = thirdPersonMovement.speed + 5;
        thirdPersonMovement.sprintSpeed = thirdPersonMovement.sprintSpeed + 5;
        speedParticle.Play();

        // Wait for 10 seconds
        yield return new WaitForSeconds(10);

        isSpedUp = false;
        thirdPersonMovement.speed = thirdPersonMovement.speed - 5;
        thirdPersonMovement.sprintSpeed = thirdPersonMovement.sprintSpeed - 5;
        
        
    }

    void TakeDamage(int damage)
    {   
        if(currentHealth > 0){
            currentHealth-=damage;
        } else {
            currentHealth = 0;
        }
        Debug.Log("Took Damage, Health Now At: " + currentHealth);

        if(currentHealth == 0){
            //End the game
            uiManager.EndGame();
        }
        if (currentHealth <= 0)
        {
            thirdPersonMovement._animator.enabled = false;
            deathParticle.Play();
        }
        healthBar.SetHealth(currentHealth);
    }

    

    void Heal(int healAmount)
    {   
        if(currentHealth + healAmount <= maxHealth){
            currentHealth += healAmount;
        } else {
            currentHealth += maxHealth - currentHealth;
        }
        Debug.Log("Healed, Health Now At: " + currentHealth);
        healthBar.SetHealth(currentHealth);
        healthParticle.Play();
    }
}
