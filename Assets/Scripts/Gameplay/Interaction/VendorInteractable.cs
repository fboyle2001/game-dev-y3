using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorInteractable : MonoBehaviour, IInteractable {
    
    public GameObject shopCanvas;

    private InteractionManager interactionManager;

    void Awake() {
        interactionManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<InteractionManager>();
    }

    public void OnInteractPossible() {
        interactionManager.SetText("to open shop");
        interactionManager.ShowText();
        interactionManager.RegisterInteraction("vendor", () => {
            if(shopCanvas.activeSelf) {
                shopCanvas.SetActive(false);
                interactionManager.ShowText();
            } else {
                shopCanvas.SetActive(true);
                interactionManager.HideText();
            }
        });
    }

    public void OnInteractImpossible() {
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("vendor");
        shopCanvas.SetActive(false);
    }

}
