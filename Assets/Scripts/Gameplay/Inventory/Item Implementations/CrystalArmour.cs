using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalArmour : EquippableInventoryItem {
    
    public CrystalArmour(Sprite itemImage) : base(itemIdentifier: "crystalArmour", itemName: "Crystal Armour", goldValue: 2000, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddArmour(50f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "armour");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddArmour(-50f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
