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

    void Start() {
        value = moneyVal;
        magnetPoint = PlayerManager.playerTransform;
    }

    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log($"Collision");

        if (other.CompareTag("Player")) {
            CollectionManager.totalDebt -= value;
            if (gameObject.CompareTag("Magnetized"))
                Destroy(gameObject, 0.5f);
            else
                Destroy(gameObject);
            
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
