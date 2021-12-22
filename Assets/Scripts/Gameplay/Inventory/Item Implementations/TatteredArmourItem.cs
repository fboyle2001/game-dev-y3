using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatteredArmourItem : EquippableInventoryItem {
    
    public TatteredArmourItem(Sprite itemImage) : base(itemIdentifier: "tatteredArmour", itemName: "Tattered Armour", goldValue: 50, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats stats = gameManager.GetComponent<PlayerStats>();
        stats.AddArmour(5);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "armour");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats stats = gameManager.GetComponent<PlayerStats>();
        stats.AddArmour(-5);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
