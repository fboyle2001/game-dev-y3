using UnityEngine;

/**
* Represents any inventory item
* Use ConsumableInventoryItem, EquippableInventoryItem and WeaponInventoryItem in general instead
**/
public abstract class InventoryItem {

    public readonly string itemIdentifier;
    public readonly string itemKey;
    public readonly int goldValue;
    public readonly Sprite itemImage;
    public readonly bool equippable;
    public readonly int maxQuantity;
    
    // Stores all necessary information about the item
    public InventoryItem(string itemIdentifier, string itemKey, int goldValue, Sprite itemImage, int maxQuantity, bool equippable) {
        this.itemIdentifier = itemIdentifier;
        this.itemKey = itemKey;
        this.goldValue = goldValue;
        this.equippable = equippable;
        this.itemImage = itemImage;
        this.maxQuantity = maxQuantity;
    }

    // Called when the item is used or equipped
    public abstract void ApplyItemEffect(GameObject gameManager);

    // Get the localised name of this item
    // Can't get a reference to LocaleManager easily since this
    // isn't a MonoBehaviour
    public string GetItemName(LocaleManager localeManager) {
        return localeManager.GetString(itemKey);
    }

}
