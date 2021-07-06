using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void Start() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void loadGame() {
        SceneManager.LoadScene("firstscene");
    }   

    public void quitGame() {
        Debug.Log("quit");
        Application.Quit();
    }
}
