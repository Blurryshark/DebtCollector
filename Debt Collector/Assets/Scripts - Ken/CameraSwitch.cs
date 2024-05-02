using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public int transitionTo;
    public int transitionFrom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            CharacterCameraState cameraState = other.GetComponent<CharacterCameraState>();
            if (cameraState != null && CharacterCameraState.CurrentCamera == transitionFrom){
                // Call the SwitchCamera method with the specified transitionTo value
                cameraState.SwitchCamera(transitionTo);
            } else if(cameraState != null && CharacterCameraState.CurrentCamera == transitionTo){
                cameraState.SwitchCamera(transitionFrom);
            } else {
                Debug.Log("CharacterCameraState not found on the player.");
            }
        }
    }
}
