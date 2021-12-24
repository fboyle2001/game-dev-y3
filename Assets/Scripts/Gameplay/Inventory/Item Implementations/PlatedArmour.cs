using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatedArmour : EquippableInventoryItem {
    
    public PlatedArmour(Sprite itemImage) : base(itemIdentifier: "platedArmour", itemKey: "item_plated_armour_name", goldValue: 750, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddArmour(25f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "armour");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddArmour(-25f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
