using UnityEngine;

public abstract class InventoryItem {

    public readonly string itemIdentifier;
    public readonly string itemKey;
    public readonly int goldValue;
    public readonly Sprite itemImage;
    public readonly bool equippable;
    public readonly int maxQuantity;
    
    public InventoryItem(string itemIdentifier, string itemKey, int goldValue, Sprite itemImage, int maxQuantity, bool equippable) {
        this.itemIdentifier = itemIdentifier;
        this.itemKey = itemKey;
        this.goldValue = goldValue;
        this.equippable = equippable;
        this.itemImage = itemImage;
        this.maxQuantity = maxQuantity;
    }

    public abstract void ApplyItemEffect(GameObject gameManager);

    public string GetItemName(LocaleManager localeManager) {
        return localeManager.GetString(itemKey);
    }

}
