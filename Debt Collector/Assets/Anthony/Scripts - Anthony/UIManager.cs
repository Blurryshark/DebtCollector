using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject HUDCanvas;
    public GameObject healthBarCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    private bool isPaused = false;
    
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused)
                Resume();
            else
                Pause();
        }
        
        if (CollectionManager.totalDebt <= 0 || Input.GetKeyDown(KeyCode.O))
            EndGame();
    }

    public void Pause() {
        isPaused = true;
        Time.timeScale = 0f;
        HUDCanvas.SetActive(false);
        healthBarCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }

    public void Resume() {
        isPaused = false;
        Time.timeScale = 1f;
        HUDCanvas.SetActive(true);
        healthBarCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    public void EndGame() {
        Time.timeScale = 1f;
        HUDCanvas.SetActive(false);
        healthBarCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }
}
