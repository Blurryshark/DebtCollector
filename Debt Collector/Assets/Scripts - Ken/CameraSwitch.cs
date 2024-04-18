using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera topDownCamera;

    private bool isThirdPersonActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isThirdPersonActive)
            {
                // Switch to the top-down camera
                thirdPersonCamera.Priority = 0;
                topDownCamera.Priority = 10;
                isThirdPersonActive = false;
            }
            else
            {
                // Switch back to the third-person camera
                thirdPersonCamera.Priority = 10;
                topDownCamera.Priority = 0;
                isThirdPersonActive = true;
            }
        }
    }
}
