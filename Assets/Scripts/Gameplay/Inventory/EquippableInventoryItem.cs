using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquippableInventoryItem : InventoryItem {

    public EquippableInventoryItem(string itemIdentifier, string itemName, Sprite itemImage) : base(itemIdentifier, itemName, itemImage, maxQuantity: 1, equippable: true) {}

}
