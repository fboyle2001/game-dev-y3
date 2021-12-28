using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationBehaviour : MonoBehaviour
{

    public LocaleManager localeManager;

    void Awake() {
        GameObject localeOwner = GameObject.FindGameObjectWithTag("Locale");

        if(localeOwner == null) {
            localeOwner = GameObject.FindGameObjectWithTag("Game Manager");
        } else {
            localeOwner.GetComponent<LocaleManager>().ChangeLocale("en-GB");
        }
    }

    void Start() {
        Application.targetFrameRate = -1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

}
