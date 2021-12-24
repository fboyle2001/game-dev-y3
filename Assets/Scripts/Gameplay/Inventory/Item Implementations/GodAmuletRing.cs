using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodAmuletRing : EquippableInventoryItem {
    
    public GodAmuletRing(Sprite itemImage) : base(itemIdentifier: "godAmulet", itemKey: "item_god_amulet_name", goldValue: 15000, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddDamageMultiplier(0.75f);
        playerStats.AddArmour(30f);
        playerStats.AddRegenPerSecond(1.8f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "ring");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddDamageMultiplier(-0.75f);
        playerStats.AddArmour(-30f);
        playerStats.AddRegenPerSecond(-1.8f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
