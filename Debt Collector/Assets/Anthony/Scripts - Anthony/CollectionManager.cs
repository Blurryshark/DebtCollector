using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionManager : MonoBehaviour {
    public static CollectionManager instance;
    public static int totalDebt;
    public TextMeshProUGUI debtText;
    [SerializeField]
    public TextMeshProUGUI itemText;
    public int startDebt = 10000;
    
    void Awake() {
        // if (instance == null) {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else {
        //     Destroy(gameObject);
        // }
    }

    void Start()
    {
        totalDebt = startDebt;
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
