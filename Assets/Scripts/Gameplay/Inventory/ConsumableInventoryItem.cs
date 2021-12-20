using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableInventoryItem : InventoryItem {

    public ConsumableInventoryItem(string itemIdentifier, string itemName, int goldValue, Sprite itemImage)
     : base(itemIdentifier, itemName, goldValue, itemImage, maxQuantity: 2, equippable: false) {}

}
