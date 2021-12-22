using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippedArmour : EquippableInventoryItem {
    
    public RippedArmour(Sprite itemImage) : base(itemIdentifier: "rippedArmour", itemName: "Ripped Armour", goldValue: 250, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddArmour(5f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "armour");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddArmour(-5f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
