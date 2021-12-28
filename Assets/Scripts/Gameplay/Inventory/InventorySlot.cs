using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        this.occupyingItem = item;
        this.quantity = quantity;
        RefreshUI();
    }

    public InventoryItem GetOccupyingItem() {
        return occupyingItem;
    }

    public void UpdateQuantity(int change) {
        this.quantity += change;

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
        };

        if(occupyingItem == null) {
            SetVisible(false);
            inventoryManager.PerformEmptyChecks();
            return;
        }

        itemNameText.GetComponent<TMP_Text>().SetText(occupyingItem.GetItemName(gameManager.GetComponent<LocaleManager>()));
        useButton.GetComponent<Button>().onClick.RemoveAllListeners();

        if(occupyingItem is WeaponInventoryItem && !gameManager.GetComponent<CharacterManager>().IsPrimaryActive()) {
            useButtonText.GetComponent<TMP_Text>().SetText(gameManager.GetComponent<LocaleManager>().GetString("ui_inventory_player_only"));
            useButton.GetComponent<Button>().interactable = false;
        } else {
            string useKey = occupyingItem.equippable ? "ui_inventory_equip_item" : "ui_inventory_use_one";
            useButtonText.GetComponent<TMP_Text>().SetText(gameManager.GetComponent<LocaleManager>().GetString(useKey));
            useButton.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("clicked!");
                uiAudioSource.PlayOneShot(usedClip);
                occupyingItem.ApplyItemEffect(gameManager);
                UpdateQuantity(-1);

                if(quantity <= 0) {
                    inventoryManager.PerformEmptyChecks();
                }
            });
            useButton.GetComponent<Button>().interactable = true;
        }

        imageDisplay.GetComponent<Image>().sprite = occupyingItem.itemImage;
        quantityText.GetComponent<TMP_Text>().SetText(quantity.ToString());
    }

}
