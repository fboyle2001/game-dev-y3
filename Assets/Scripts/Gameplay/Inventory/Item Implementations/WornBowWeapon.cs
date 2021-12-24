using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WornBowWeapon : WeaponInventoryItem {
    
    public WornBowWeapon(Sprite itemImage) : base(itemIdentifier: "wornBow", itemKey: "item_worn_bow_name", goldValue: 0, itemImage, roundsPerMinute: 90, damagePerRound: 4) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats stats = gameManager.GetComponent<PlayerStats>();
        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "weapon");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats stats = gameManager.GetComponent<PlayerStats>();
        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
