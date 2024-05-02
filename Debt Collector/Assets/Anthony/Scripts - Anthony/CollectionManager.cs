using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionManager : MonoBehaviour {
    public static CollectionManager instance;
    public static int totalDebt = 100000;
    public TextMeshProUGUI debtText;
    [SerializeField]
    public TextMeshProUGUI itemText;
    
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        moneyUpdate();
    }

    private void moneyUpdate() {
        debtText.text = $"{totalDebt:0000}";
    }

    public void itemUpdate(GameObject item) {
        if (item != null)
            itemText.text = $"ITEM:\n{item.transform.parent.name}";
    }
}
