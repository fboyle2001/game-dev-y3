using UnityEngine;

/**
* Interactable that allows the player to use the shop
**/
public class VendorInteractable : MonoBehaviour, IInteractable {
    
    public GameObject shopCanvas;

    private InteractionManager interactionManager;

    void Awake() {
        interactionManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<InteractionManager>();
    }

    public void OnInteractPossible() {
        interactionManager.SetText("int_open_shop");
        interactionManager.ShowText();
        interactionManager.RegisterInteraction("vendor", () => {
            // Open and close the shop
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
