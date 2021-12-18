using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public static Dictionary<string, InventoryItem> registeredItems = new Dictionary<string, InventoryItem>();

    public static void RegisterItem(InventoryItem item) {
        registeredItems.Add(item.itemIdentifier, item);
    }

    public InventorySlot[] inventorySlots;
    public GameObject emptyText;

    private void RegisterItems() {
        RegisterItem(new RegenPotionItem(Resources.Load<Sprite>("Images/UI/Health")));
    }

    void OnEnable() {
        RegisterItems();

        foreach(InventorySlot slot in inventorySlots) {
            slot.SetVisible(false);
        }

        emptyText.SetActive(true);
    }

    public void AddItemToInventory(string itemIdentifier, int quantity, int value) {
        // First check if we have an item slot occupied by this
        bool inventoryFull = true;
        InventorySlot occupiedSlot = null;
        InventorySlot firstEmptySlot = null;

        foreach(InventorySlot slot in inventorySlots) {
            if(slot.HasOccupyingItem()) {
                if(slot.GetOccupyingItem().itemIdentifier == itemIdentifier) {
                    occupiedSlot = slot;
                    break;
                }
            } else {
                inventoryFull = false;
                
                if(firstEmptySlot == null) {
                    firstEmptySlot = slot;
                }
            }
        }

        emptyText.SetActive(false);

        if(occupiedSlot == null && inventoryFull) {
            // TODO: Let them know their inventory is full and pay them for the excess
            int goldPayout = quantity * value;
        } else if (occupiedSlot != null) {
            // Add to the current slot and pay for their excess
            int realQuantity = occupiedSlot.GetQuantity() + quantity > occupiedSlot.GetOccupyingItem().maxQuantity ?
                    occupiedSlot.GetOccupyingItem().maxQuantity - occupiedSlot.GetQuantity() : quantity;
            int goldPayout = (quantity - realQuantity) * value; 
            
            occupiedSlot.UpdateQuantity(realQuantity);
        } else {
            // Occupy a new slot
            firstEmptySlot.SetOccupyingItem(registeredItems[itemIdentifier], quantity);
            firstEmptySlot.SetVisible(true);
        }

    }

    public void PerformEmptyChecks() {
        bool empty = true;

        foreach(InventorySlot slot in inventorySlots) {
            if(slot.HasOccupyingItem()) {
                empty = false;
            }
        }

        if(empty) {
            emptyText.SetActive(true);
        }
    }

}
