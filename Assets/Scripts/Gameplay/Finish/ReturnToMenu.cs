using UnityEngine;
using UnityEngine.SceneManagement;

/**
* Very simple script just used to return to the menu scene
**/
public class ReturnToMenu : MonoBehaviour {

    public void OnClick() {
        SceneManager.LoadScene("MainMenu");
    }

}