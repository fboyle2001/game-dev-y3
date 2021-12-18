using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealthPotionItem  : ConsumableInventoryItem {

    public FullHealthPotionItem(Sprite itemImage) : base(itemIdentifier: "fullHealthPotion", itemName: "Total Heal", goldValue: 10000, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.Heal(100000);
    }
}
