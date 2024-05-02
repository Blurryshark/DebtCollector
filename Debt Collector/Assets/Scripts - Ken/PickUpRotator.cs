using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRotator : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation around the y-axis
    public float oscillationSpeed = 1f; // Speed of oscillation
    public float oscillationHeight = 0.5f; // Height of oscillation

    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate around the world y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Oscillate up and down
        float newY = startY + Mathf.Sin(Time.time * oscillationSpeed) * oscillationHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
