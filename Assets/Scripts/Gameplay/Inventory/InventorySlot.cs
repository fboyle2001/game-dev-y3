using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
* Represents a slot in the inventory and its UI
**/
public class InventorySlot : MonoBehaviour {

    public AudioSource uiAudioSource;
    public AudioClip usedClip;

    public PlayerInventory inventoryManager;
    public GameObject quantityText;
    public GameObject useButton;
    public GameObject useButtonText;
    public GameObject imageDisplay;
    public GameObject itemNameText;
    public GameObject slotParent;

    private InventoryItem occupyingItem = null;
    private int quantity = 0;
    private GameObject gameManager;

    private GameObject active;

    void OnEnable() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        RefreshUI();
    }

    public void SetVisible(bool visible) {
        // Set visible iff they have an item regardless
        slotParent.SetActive(visible && occupyingItem != null);

        if(visible && occupyingItem != null) {
            RefreshUI();
        }
    }

    public bool IsVisible() {
        return slotParent.activeSelf;
    }

    public bool HasOccupyingItem() {
        return occupyingItem != null && quantity > 0;
    }

    public void SetOccupyingItem(InventoryItem item, int quantity) {
        // Occupying item determines what is displayed in the UI
        this.occupyingItem = item;
        this.quantity = quantity;
        RefreshUI();
    }

    public InventoryItem GetOccupyingItem() {
        return occupyingItem;
    }

    public void UpdateQuantity(int change) {
        this.quantity += change;

        // If we go to 0 or below then we no longer have an item
        if(quantity <= 0) {
            this.occupyingItem = null;
        }

        RefreshUI();
    }

    public int GetQuantity() {
        return quantity;
    }

    private void RefreshUI() {
        if(gameManager == null) {
            gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        }

        // No item then hide this
        if(occupyingItem == null) {
            SetVisible(false);
            inventoryManager.PerformEmptyChecks();
            return;
        }

        // Get the localised item name
        itemNameText.GetComponent<TMP_Text>().SetText(occupyingItem.GetItemName(gameManager.GetComponent<LocaleManager>()));
        // Remove the on click listeners as we will re-add them
        useButton.GetComponent<Button>().onClick.RemoveAllListeners();
        
        if(occupyingItem is WeaponInventoryItem && !gameManager.GetComponent<CharacterManager>().IsPrimaryActive()) {
            // Can't equip weapons from the secondary
            useButtonText.GetComponent<TMP_Text>().SetText(gameManager.GetComponent<LocaleManager>().GetString("ui_inventory_player_only"));
            useButton.GetComponent<Button>().interactable = false;
        } else {
            // Text depends on if it's consumable or equippable
            string useKey = occupyingItem.equippable ? "ui_inventory_equip_item" : "ui_inventory_use_one";
            // Localise
            useButtonText.GetComponent<TMP_Text>().SetText(gameManager.GetComponent<LocaleManager>().GetString(useKey));
            useButton.GetComponent<Button>().onClick.AddListener(() => {
                // Use the item and play a UI sound
                uiAudioSource.PlayOneShot(usedClip);
                occupyingItem.ApplyItemEffect(gameManager);
                UpdateQuantity(-1);

                if(quantity <= 0) {
                    inventoryManager.PerformEmptyChecks();
                }
            });

            // Enable the button
            useButton.GetComponent<Button>().interactable = true;
        }

        // Update the image and quantity
        imageDisplay.GetComponent<Image>().sprite = occupyingItem.itemImage;
        quantityText.GetComponent<TMP_Text>().SetText(quantity.ToString());
    }

}
