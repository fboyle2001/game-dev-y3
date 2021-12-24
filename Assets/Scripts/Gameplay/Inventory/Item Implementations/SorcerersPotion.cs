using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcerersPotion : ConsumableInventoryItem {

    public SorcerersPotion(Sprite itemImage) : base(itemIdentifier: "sorcerersPotion", itemKey: "item_sorcerers_potion_name", goldValue: 1000, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddRegenPerSecond(0.2f);
        playerStats.AddDamageMultiplier(0.1f);
        playerStats.AddArmour(2f);
    }
}
