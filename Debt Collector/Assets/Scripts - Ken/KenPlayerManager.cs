using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KenPlayerManager : MonoBehaviour
{
    private bool isSpedUp;
    private ThirdPersonMovement thirdPersonMovement;
    // Start is called before the first frame update
    void Start()
    {
        isSpedUp = false;
        thirdPersonMovement = GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
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
        }
    }

    IEnumerator IncreaseSpeed()
    {
        Debug.Log("Up the Speed Here!");

        isSpedUp = true;
        thirdPersonMovement.speed = thirdPersonMovement.speed + 5;
        thirdPersonMovement.speed = thirdPersonMovement.speed + 5;

        // Wait for 10 seconds
        yield return new WaitForSeconds(10);

        isSpedUp = false;
        thirdPersonMovement.speed = thirdPersonMovement.speed - 5;
        thirdPersonMovement.speed = thirdPersonMovement.speed - 5;
    }
}
