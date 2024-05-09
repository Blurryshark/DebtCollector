using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDayNightCycle : MonoBehaviour
{
    public float speed = 4.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * speed,0,0);
    }
}
