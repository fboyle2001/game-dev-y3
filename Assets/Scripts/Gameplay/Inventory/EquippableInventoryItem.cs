using UnityEngine;

/**
* Represents any equippable item
* (i.e. weapons*, rings and armour)
* (*weapons extend the subclass WeaponInventoryItem instead)
**/
public abstract class EquippableInventoryItem : InventoryItem {

    // Limit the quantity to 1 since they don't have durability
    public EquippableInventoryItem(string itemIdentifier, string itemKey, int goldValue, Sprite itemImage) 
    : base(itemIdentifier, itemKey, goldValue, itemImage, maxQuantity: 1, equippable: true) {}

    // Called when they unequip the item to remove any stats etc that were granted
    public abstract void ReverseEffect(GameObject gameManager);

}
