using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelName;

    public void ShowStartOptions() {
        SceneManager.LoadScene(levelName);
    }

    public void ExitGame() {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
