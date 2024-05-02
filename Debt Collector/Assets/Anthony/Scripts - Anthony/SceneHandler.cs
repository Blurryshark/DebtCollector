using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ChangeScene(string _scene) {
        SceneManager.LoadScene(_scene);
    }
    
    public void QuitApplication() {
        Application.Quit();
    }
}
