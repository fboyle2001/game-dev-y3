using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquippableInventoryItem : InventoryItem {

    public EquippableInventoryItem(string itemIdentifier, string itemName, int goldValue, Sprite itemImage) 
    : base(itemIdentifier, itemName, goldValue, itemImage, maxQuantity: 1, equippable: true) {}

    public abstract void ReverseEffect(GameObject gameManager);

}
