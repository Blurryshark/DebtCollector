using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera Camera1;
    public CinemachineVirtualCamera Camera2;

    private bool isCamera1Active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isCamera1Active)
            {
                // Switch to the second camera
                Camera1.Priority = 0;
                Camera2.Priority = 10;
                isCamera1Active = false;
            }
            else
            {
                // Switch back to the first camera
                Camera1.Priority = 10;
                Camera2.Priority = 0;
                isCamera1Active = true;
            }
        }
    }
}
