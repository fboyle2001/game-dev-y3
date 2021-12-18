using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableInventoryItem : InventoryItem {

    public ConsumableInventoryItem(string itemIdentifier, string itemName, Sprite itemImage) : base(itemIdentifier, itemName, itemImage, maxQuantity: 99, equippable: false) {}

}
