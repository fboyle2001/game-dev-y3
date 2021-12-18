using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* The CharacterManager is responsible for managing which character is currently active
* this can be either the primary character or the secondary (i.e. the fox/cat).
* This is also responsible for changing the active character and updating the active
* camera and image as necessary.
*/
public class ActiveCharacterManager : MonoBehaviour
{

    public GameObject primary;
    public GameObject secondary;
    public Camera primaryCamera;
    public Camera secondaryCamera;
    public Image primaryImage;
    public Image secondaryImage;

    private bool isPrimaryActive;
    private bool isSecondaryUnlocked;
    private MovementControllerWithFollow playerMovementController;
    private MovementControllerWithFollow secondaryMovementController;
    private bool primaryAlive;
    private bool secondaryAlive;

    void Start() {
        this.isPrimaryActive = true;
        this.playerMovementController = primary.GetComponent<MovementControllerWithFollow>();
        this.primaryAlive = true;
        this.secondaryAlive = secondary != null;

        this.UpdateSecondaryStatus();
        this.UpdateActiveCharacter();
    }

    void Update() {
        // If the user presses V and they have the secondary character then 
        // we switch to the other character
        if(Input.GetKeyDown(KeyCode.V) && isSecondaryUnlocked && primaryAlive && secondaryAlive) {
            isPrimaryActive = !isPrimaryActive;
            this.UpdateActiveCharacter();
        }
    }

    // Update cameras and images to reflect which character is active
    private void UpdateActiveCharacter() {
        primaryCamera.gameObject.SetActive(isPrimaryActive);
        
        playerMovementController.movementEnabled = isPrimaryActive;

        if(isSecondaryUnlocked) {
            secondaryMovementController.movementEnabled = !isPrimaryActive;
            secondaryCamera.gameObject.SetActive(!isPrimaryActive);
        }

        if(isPrimaryActive) {
            primaryImage.color = new Color32(255, 255, 255, 255);
            secondaryImage.color = isSecondaryUnlocked && secondaryAlive ? new Color32(128, 128, 128, 128) : new Color32(128, 0, 0, 50);
        } else {
            primaryImage.color = primaryAlive ? new Color32(128, 128, 128, 128) : new Color32(128, 0, 0, 50);
            secondaryImage.color = new Color32(255, 255, 255, 255);
        }
    }

    // Returns the GameObject representing the active character
    public GameObject GetActiveCharacter() {
        return isPrimaryActive ? primary : secondary;
    }

    public bool IsPrimaryActive() {
        return isPrimaryActive;
    }
    
    public void UpdateSecondaryStatus() {
        this.isSecondaryUnlocked = secondary != null;

        if(isSecondaryUnlocked) {
            this.secondaryMovementController = secondary.GetComponent<MovementControllerWithFollow>();
        }
    }

    public void OnCharacterDeath(bool isPrimary) {
        GameObject deadCharacter = isPrimary ? this.primary : this.secondary;

        if(isPrimary) {
            primaryAlive = false;
        } else {
            secondaryAlive = false;
        }

        if(!primaryAlive && (!isSecondaryUnlocked || !secondaryAlive)) {
            this.OnGameOver();
            return;
        }

        isPrimaryActive = isPrimary ? false : true;
        this.UpdateActiveCharacter();
        deadCharacter.SetActive(false);
    }

    private void OnGameOver() {
        Debug.Log("Both players are dead");
    }

    public void SetCharacterRotationAvailability(bool canRotate) {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("Player Camera");

        foreach(GameObject camera in cameras) {
            camera.GetComponent<CameraMouseRotate>().rotationEnabled = canRotate;
        }
    }

}
