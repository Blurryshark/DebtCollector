using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterCameraState : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCamera;
    public CinemachineVirtualCamera topDownCamera;
    public CinemachineVirtualCamera firstPersonCamera;

    public static int CurrentCamera;
    // 1 is third person
    // 2 is top down
    // 3 is first person

    private void Start()
    {
        CurrentCamera = 1;
        thirdPersonCamera.Priority = 10;
        topDownCamera.Priority = 0;
        firstPersonCamera.Priority = 0;
    }

    public void SwitchCamera(int cameraNum)
    {
        if(cameraNum == 2){
            CurrentCamera = 2;
            thirdPersonCamera.Priority = 0;
            topDownCamera.Priority = 10;
            firstPersonCamera.Priority = 0;
        } else if(cameraNum == 3){
            CurrentCamera = 3;
            thirdPersonCamera.Priority = 0;
            topDownCamera.Priority = 0;
            firstPersonCamera.Priority = 10;
        } else {
            CurrentCamera = 1;
            thirdPersonCamera.Priority = 10;
            topDownCamera.Priority = 0;
            firstPersonCamera.Priority = 0;
        }
    }
}
