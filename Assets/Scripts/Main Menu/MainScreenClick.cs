using UnityEngine;

/**
* Main menu screen UI actions
**/
public class MainScreenClick : MonoBehaviour {
    
    public GameObject startScreen;
    public GameObject settingsScreen;

    public void ShowStartScreen() {
        startScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ShowSettingsScreen() {
        settingsScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }

}
