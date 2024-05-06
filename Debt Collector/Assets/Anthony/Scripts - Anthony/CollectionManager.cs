using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionManager : MonoBehaviour {
    public static CollectionManager instance;
    public static int totalDebt = 1000;
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

    void LateUpdate() {
        moneyUpdate();
    }

    private void moneyUpdate() {
        debtText.text = $"{totalDebt:000000}";
    }

    public void itemUpdate(GameObject item) {
        if (item != null)
            itemText.text = $"ITEM:\n{item.transform.parent.name}";
    }
}
