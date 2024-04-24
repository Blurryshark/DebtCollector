using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static Transform playerTransform;
    public static int playerHealth = 100; // static to be referenced by other enemy scripts.
    public int baseDamage = 10;
    public static GameObject currentItem = null;
    
    void Start() {
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        playerTransform = gameObject.transform;
    }
}
