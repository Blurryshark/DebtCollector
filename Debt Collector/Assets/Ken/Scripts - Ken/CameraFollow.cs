using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 clampAxis = new Vector2(-60, 60);  // Adjusted for intuitive up/down limits
    
    [SerializeField] float follow_smoothing = 5;
    [SerializeField] float rotate_Smoothing = 10;  // Adjusted for smoother rotation
    [SerializeField] float sensitivity = 100;  // Adjusted sensitivity

    float rotX, rotY;
    bool cursorLocked = true;  // Initialized as true for immediate locking
    Transform cam;

    public bool lockedTarget;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
    }

    void Update()
    {
        Vector3 target_P = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing * Time.deltaTime);

        if (!lockedTarget)
            CameraTargetRotation();
        else
            LookAtTarget();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = !cursorLocked;  // Toggle the cursor locked state
            Cursor.visible = !cursorLocked;
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    void CameraTargetRotation()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotX += mouseAxis.x * sensitivity * Time.deltaTime;
        rotY -= mouseAxis.y * sensitivity * Time.deltaTime;  // Changed from -= to += for intuitive control

        rotY = Mathf.Clamp(rotY, clampAxis.x, clampAxis.y);  // Ensure rotation is within bounds

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, rotate_Smoothing * Time.deltaTime);
    }

    void LookAtTarget()
    {
        transform.rotation = cam.rotation;
        Vector3 r = cam.eulerAngles;
        rotX = r.y;
        rotY = 1.8f;  // Minor adjustment may be needed based on specific use case
    }
}

