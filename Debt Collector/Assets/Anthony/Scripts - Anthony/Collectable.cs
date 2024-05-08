using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour {
    private int value;
    public int moneyVal;
    private Transform magnetPoint;
    private float moveSpeed = 10f;

    public AudioSource collectableSound;

    void Start() {
        value = moneyVal;
        magnetPoint = PlayerManager.playerTransform;
    }

    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            CollectionManager.totalDebt -= value;
            Debug.Log("Total Debt: " + CollectionManager.totalDebt);
            if (gameObject.CompareTag("Magnetized"))
                Destroy(gameObject, 0.5f);
            else
                Destroy(gameObject);
            collectableSound.Play();
        }
    }

    void OnTriggerStay(Collider other) {
        if (gameObject.CompareTag("Magnetized") && other.CompareTag("Player") && other != null) {
            if (PlayerManager.playerTransform != null)
                magnetPoint = PlayerManager.playerTransform;
            transform.position = Vector3.MoveTowards(transform.position, magnetPoint.position, moveSpeed * Time.deltaTime);
        }
    }
}
