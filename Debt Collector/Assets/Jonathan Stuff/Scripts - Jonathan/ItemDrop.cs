using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private UnityEvent onDrop = null;

    public void Drop()
    {
        gameObject.SetActive(true);
        transform.parent = null;

        onDrop?.Invoke();
    }
    //this is so the enemy can drop current items.
}
