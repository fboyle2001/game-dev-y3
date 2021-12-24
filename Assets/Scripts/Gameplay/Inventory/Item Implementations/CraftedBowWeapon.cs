using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedBowWeapon : WeaponInventoryItem {
    
    public CraftedBowWeapon(Sprite itemImage) : base(itemIdentifier: "craftedBow", itemKey: "item_crafted_bow_name", goldValue: 500, itemImage, roundsPerMinute: 120, damagePerRound: 6) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "weapon");
    }

    public override void ReverseEffect(GameObject gameManager) {
        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
