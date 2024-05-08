using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject HUDCanvas;
    public GameObject healthBarCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject gameWonCanvas;
    private bool isPaused;
    [HideInInspector]public bool gameLost;
    
    void Start() {
        isPaused = false;
        gameLost = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused)
                Resume();
            else
                Pause();
        }
        
        if (CollectionManager.totalDebt <= 0)
            WonGame();
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
        gameLost = true;
        
        HUDCanvas.SetActive(false);
        healthBarCanvas.SetActive(false);
        gameWonCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        StartCoroutine(WaitForAnimation());
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void WonGame() {
        gameLost = true;
        
        HUDCanvas.SetActive(false);
        healthBarCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        gameWonCanvas.SetActive(true);
        StartCoroutine(WaitForAnimation());
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0f;
    }
}
