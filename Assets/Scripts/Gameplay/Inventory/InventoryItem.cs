using UnityEngine;

public abstract class InventoryItem {

    public readonly string itemIdentifier;
    public readonly string itemName;
    public readonly Sprite itemImage;
    public readonly bool equippable;
    public readonly int maxQuantity;
    
    public InventoryItem(string itemIdentifier, string itemName, Sprite itemImage, int maxQuantity, bool equippable) {
        this.itemIdentifier = itemIdentifier;
        this.itemName = itemName;
        this.equippable = equippable;
        this.itemImage = itemImage;
        this.maxQuantity = maxQuantity;
    }

    public abstract void ApplyItemEffect(GameObject gameManager);

}
