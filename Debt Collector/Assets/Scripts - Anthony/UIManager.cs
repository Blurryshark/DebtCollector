using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject HUDCanvas;
    public GameObject pauseCanvas;
    //public GameObject gOverCanvas;
    
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            HandlePause();
    }

    private void HandlePause() {
        HUDCanvas.SetActive(pauseCanvas.activeSelf);
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
    }
}
