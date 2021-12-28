using UnityEngine;

/**
* Represents any item in the inventory that can be consumed
* e.g. potions
**/
public abstract class ConsumableInventoryItem : InventoryItem {

    public ConsumableInventoryItem(string itemIdentifier, string itemKey, int goldValue, Sprite itemImage)
     : base(itemIdentifier, itemKey, goldValue, itemImage, maxQuantity: 99, equippable: false) {}

}
