using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableInventoryItem : InventoryItem {

    public ConsumableInventoryItem(string itemIdentifier, string itemKey, int goldValue, Sprite itemImage)
     : base(itemIdentifier, itemKey, goldValue, itemImage, maxQuantity: 99, equippable: false) {}

}
