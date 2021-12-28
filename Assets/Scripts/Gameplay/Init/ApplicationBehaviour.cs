using UnityEngine;

public class ApplicationBehaviour : MonoBehaviour {

    void Awake() {
        // If we are in the menu then set the locale to en-GB at the start
        GameObject localeOwner = GameObject.FindGameObjectWithTag("Locale");

        if(localeOwner != null) {
            localeOwner.GetComponent<LocaleManager>().ChangeLocale("en-GB");
        }
    }

    void Start() {
        // Handles cursor state and targets VSync for the frame rate
        Application.targetFrameRate = -1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

}
