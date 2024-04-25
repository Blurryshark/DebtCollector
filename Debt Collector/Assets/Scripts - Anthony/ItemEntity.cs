using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEntity : MonoBehaviour {
    public int damageMultiplier = 2;
    private int durability = 5; // will touch up more on later
    public GameObject pivotPoint;
    
    void Start() {

    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("Q key is pressed down");
            DropItem();
        }
    }

    void DropItem() {
        PlayerManager.itemHolder.transform.DetachChildren();
        PlayerManager.currentItem = null;
        GetComponent<Collider>().enabled = true;
        
        if (CollectionManager.instance != null)
            CollectionManager.instance.itemText.text = "Item:\n";
        else {
            Debug.Log("CollectionManager.instance is null");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && PlayerManager.currentItem == null) {
            GameObject holder = GameObject.Find("itemHolder");
            pivotPoint.transform.SetParent(holder.transform);

            pivotPoint.transform.localPosition = Vector3.zero;
            pivotPoint.transform.localRotation = Quaternion.identity;
            GetComponent<Collider>().enabled = false;
            
            CollectionManager.instance.itemUpdate(gameObject);
            PlayerManager.currentItem = gameObject;
        }
        
    }
}
