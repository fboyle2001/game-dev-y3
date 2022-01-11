using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ApplicationBehaviour : MonoBehaviour {

    void Awake() {
        // If we are in the menu then set the locale to en-GB at the start
        GameObject localeOwner = GameObject.FindGameObjectWithTag("Locale");

        if(localeOwner != null) {
            localeOwner.GetComponent<LocaleManager>().ChangeLocale("en-GB");
        }
    }

    void OnEnable() {
        // Register event to disable haptics
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    void OnDisable() {
        // Register event to disable haptics
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    void Start() {
        // Handles cursor state and targets VSync for the frame rate
        Application.targetFrameRate = -1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void OnApplicationQuit() {
        // End any rumble
        InputSystem.ResetHaptics();
    }

    void OnSceneChange(Scene currentScene, Scene newScene) {
        // End any rumble
        InputSystem.ResetHaptics();
    }

}
