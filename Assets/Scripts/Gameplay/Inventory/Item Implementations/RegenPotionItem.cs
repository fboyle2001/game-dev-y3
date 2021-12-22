using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenPotionItem : ConsumableInventoryItem {
    
    public RegenPotionItem(Sprite itemImage) : base(itemIdentifier: "regenPotion", itemName: "Regen Potion", goldValue: 100, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats stats = gameManager.GetComponent<PlayerStats>();
        stats.AddRegenPerSecond(0.2f);
    }

}
