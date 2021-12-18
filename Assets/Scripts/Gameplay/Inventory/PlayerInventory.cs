using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    public static Dictionary<string, InventoryItem> registeredItems = new Dictionary<string, InventoryItem>();

    public static void RegisterItem(InventoryItem item) {
        registeredItems.Add(item.itemIdentifier, item);
    }

    public GameObject weaponSlot;
    public GameObject armourSlot;
    public GameObject ringSlot;
    public InventorySlot[] inventorySlots;
    public GameObject emptyText;

    private GameObject gameManager;

    private EquippableInventoryItem currentWeapon = null;
    private EquippableInventoryItem currentArmour = null;
    private EquippableInventoryItem currentRing = null;

    private void RegisterItems() {
        RegisterItem(new RegenPotionItem(Resources.Load<Sprite>("Images/UI/Health")));
        RegisterItem(new TatteredArmourItem(Resources.Load<Sprite>("Images/Characters/Fox")));
        RegisterItem(new IronArmourItem(Resources.Load<Sprite>("Images/Characters/Player")));
    }

    void OnEnable() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        RegisterItems();

        foreach(InventorySlot slot in inventorySlots) {
            slot.SetVisible(false);
        }

        emptyText.SetActive(true);
        AddItemToInventory("tatteredArmour", 1);
        AddItemToInventory("tatteredArmour", 1);
        AddItemToInventory("ironArmour", 1);
    }

    public void AddItemToInventory(string itemIdentifier, int quantity) {
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
            int goldPayout = quantity * registeredItems[itemIdentifier].goldValue;
            gameManager.GetComponent<PlayerResources>().UpdateGold(goldPayout);
        } else if (occupiedSlot != null) {
            // Add to the current slot and pay for their excess
            int realQuantity = occupiedSlot.GetQuantity() + quantity > occupiedSlot.GetOccupyingItem().maxQuantity ?
                    occupiedSlot.GetOccupyingItem().maxQuantity - occupiedSlot.GetQuantity() : quantity;
            int goldPayout = (quantity - realQuantity) * registeredItems[itemIdentifier].goldValue; 
            gameManager.GetComponent<PlayerResources>().UpdateGold(goldPayout);
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

    public void EquipItem(EquippableInventoryItem item, string slot) {
        switch(slot) {
            case "weapon":
                if(currentWeapon != null) {
                    currentWeapon.ReverseEffect(gameManager);
                }

                weaponSlot.GetComponent<Image>().sprite = item.itemImage;
                currentWeapon = item;
                break;
            case "armour":
                if(currentArmour != null) {
                    currentArmour.ReverseEffect(gameManager);
                    Debug.Log("applied reverse");
                }

                armourSlot.GetComponent<Image>().sprite = item.itemImage;
                currentArmour = item;
                break;
            case "ring":
                if(currentRing != null) {
                    currentRing.ReverseEffect(gameManager);
                }

                ringSlot.GetComponent<Image>().sprite = item.itemImage;
                currentRing = item;
                break;
            default:
                break;
        }
    }

}
