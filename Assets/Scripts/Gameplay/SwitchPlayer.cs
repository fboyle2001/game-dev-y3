using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPlayer : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public GameObject secondary;
    public Camera secondaryCamera;
    public bool isPlayerActive;
    public Image playerImage;
    public Image secondaryImage;

    private MovementControllerWithFollow playerMovementController;
    private MovementControllerWithFollow secondaryMovementController;

    void Start() {
        playerMovementController = player.GetComponent<MovementControllerWithFollow>();
        secondaryMovementController = secondary.GetComponent<MovementControllerWithFollow>();

        playerCamera.gameObject.SetActive(isPlayerActive);
        secondaryCamera.gameObject.SetActive(!isPlayerActive);

        if(isPlayerActive) {
            playerImage.color = new Color32(255, 255, 255, 255);
            secondaryImage.color = new Color32(128, 128, 128, 128);
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.V)) {
            isPlayerActive = !isPlayerActive;
            playerMovementController.movementEnabled = isPlayerActive;
            secondaryMovementController.movementEnabled = !isPlayerActive;
            playerCamera.gameObject.SetActive(isPlayerActive);
            secondaryCamera.gameObject.SetActive(!isPlayerActive);

            if(isPlayerActive) {
                playerImage.color = new Color32(255, 255, 255, 255);
                secondaryImage.color = new Color32(128, 128, 128, 128);
            } else {
                playerImage.color = new Color32(128, 128, 128, 128);
                secondaryImage.color = new Color32(255, 255, 255, 255);
            }
        } 
    }

    public GameObject GetActiveCharacter() {
        if(isPlayerActive) {
            return player;
        }

        return secondary;
    }
}
