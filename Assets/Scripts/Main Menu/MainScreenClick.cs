using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenClick : MonoBehaviour {
    
    public GameObject startScreen;
    public GameObject settingsScreen;
    public GameObject creditsScreen;

    public void ShowStartScreen() {
        startScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ShowSettingsScreen() {
        settingsScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ShowCreditsScreen() {
        creditsScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }

}
