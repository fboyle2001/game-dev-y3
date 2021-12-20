using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInteractText : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if(interactable != null) {
            interactable.OnInteractPossible();
        }
    }

    void OnTriggerExit(Collider other) {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if(interactable != null) {
            interactable.OnInteractImpossible();
        }
    }

}
