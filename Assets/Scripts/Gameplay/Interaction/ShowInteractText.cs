using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInteractText : MonoBehaviour {

    private CharacterManager characterManager;

    private IInteractable currentInteractable;

    void Awake() {
        characterManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<CharacterManager>();
    }

    public void RecheckInterability() {
        if(currentInteractable != null) {
            currentInteractable.OnInteractPossible();
        }
    }

    void OnTriggerEnter(Collider other) {
        if(!characterManager.IsPrimaryActive() || other.tag != "Interactable") return;

        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if(interactable != null) {
            interactable.OnInteractPossible();
            currentInteractable = interactable;
        }
    }

    void OnTriggerExit(Collider other) {
        if(!characterManager.IsPrimaryActive() || other.tag != "Interactable") return;
        
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if(interactable != null) {
            interactable.OnInteractImpossible();
            currentInteractable = interactable;
        }
    }

}
