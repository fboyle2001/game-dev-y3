using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSecondaryUI : MonoBehaviour
{

    public GameObject gameManager;
    private ActiveCharacterManager gameCharacterManager;

    void Start() {
        gameCharacterManager = gameManager.GetComponent<ActiveCharacterManager>();
    }

    void Update() {
        if(gameObject.activeSelf && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))) {
            this.CloseUI();
        }
    }

    public void OpenUI() {
        if(gameObject.activeSelf) {
            return;
        }

        // Start sometimes does not get called correctly
        // Maybe a bug in Unity??
        if(gameCharacterManager == null) {
            gameCharacterManager = gameManager.GetComponent<ActiveCharacterManager>();
        }

        gameCharacterManager.SetCharacterRotationAvailability(false);
        gameObject.SetActive(true);
    }

    public void CloseUI() {
        if(!gameObject.activeSelf) {
            return;
        }
        
        // Start sometimes does not get called correctly
        // Maybe a bug in Unity??
        if(gameCharacterManager == null) {
            gameCharacterManager = gameManager.GetComponent<ActiveCharacterManager>();
        }

        gameCharacterManager.SetCharacterRotationAvailability(true);
        gameObject.SetActive(false);
    }

}
