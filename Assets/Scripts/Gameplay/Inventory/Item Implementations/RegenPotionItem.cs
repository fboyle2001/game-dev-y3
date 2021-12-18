using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenPotionItem : ConsumableInventoryItem {
    
    public RegenPotionItem(Sprite itemImage) : base(itemIdentifier: "regenPotion", itemName: "Regen Potion", itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.SetRegenPerSecond(stats.GetRegenPerSecond() + 0.2f);
    }

}
