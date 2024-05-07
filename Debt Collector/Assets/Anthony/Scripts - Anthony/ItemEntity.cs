using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEntity : MonoBehaviour {
    public int damageMultiplier = 2;
    private int durability = 5; // will touch up more on later
    private bool isEquipped = false;
    private float angle = 25f;
    public GameObject pivotPoint;
    
    void Start() {
        transform.rotation = Quaternion.Euler(angle, 0f, 0f);
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("Q key is pressed down");
            DropItem();
            Unequipped();
        }
        
        if (!isEquipped)
            pivotPoint.transform.rotation *= Quaternion.AngleAxis(45f * Time.deltaTime, Vector3.up);
    }

    void DropItem() {
        PlayerManager.itemHolder.transform.DetachChildren();
        PlayerManager.currentItem = null;
        isEquipped = false;
        GetComponent<Collider>().enabled = true;
        
        if (CollectionManager.instance != null)
            CollectionManager.instance.itemText.text = "Item:\n";
        else {
            Debug.Log("CollectionManager.instance is null");
        }
    }

    void Unequipped() {
        transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && PlayerManager.currentItem == null) {
            isEquipped = true;
            GameObject holder = GameObject.Find("itemHolder");
            
            transform.localRotation = Quaternion.identity;
            
            pivotPoint.transform.SetParent(holder.transform);
            pivotPoint.transform.localPosition = Vector3.zero;
            pivotPoint.transform.localRotation = Quaternion.identity;
            
            GetComponent<Collider>().enabled = false;
            
            CollectionManager.instance.itemUpdate(gameObject);
            PlayerManager.currentItem = gameObject;
        }
        
    }
}
