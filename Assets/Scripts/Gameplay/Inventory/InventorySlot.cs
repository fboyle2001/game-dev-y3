using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

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

    void OnEnable() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
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
        if(occupyingItem == null) {
            SetVisible(false);
            inventoryManager.PerformEmptyChecks();
            return;
        }

        itemNameText.GetComponent<TMP_Text>().SetText(occupyingItem.itemName);
        useButtonText.GetComponent<TMP_Text>().SetText(occupyingItem.equippable ? "Equip Item" : "Use One");
        imageDisplay.GetComponent<Image>().sprite = occupyingItem.itemImage;
        useButton.GetComponent<Button>().onClick.RemoveAllListeners();
        useButton.GetComponent<Button>().onClick.AddListener(() => {
            occupyingItem.ApplyItemEffect(gameManager);
            UpdateQuantity(-1);

            if(quantity <= 0) {
                inventoryManager.PerformEmptyChecks();
            }
        });
        quantityText.GetComponent<TMP_Text>().SetText(quantity.ToString());
    }

}
