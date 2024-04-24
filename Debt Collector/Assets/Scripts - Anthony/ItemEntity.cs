using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour {
    public int damageMultiplier = 2;
    private int durability = 5; // will touch up more on later
    private bool isHeld = false;
    
    void Start() {
        
    }
    
    void Update() {
        if (isHeld) {
            transform.position = PlayerManager.playerTransform.position + new Vector3(0.5f, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        isHeld = true;
        CollectionManager.instance.itemUpdate(gameObject);
    }
}
