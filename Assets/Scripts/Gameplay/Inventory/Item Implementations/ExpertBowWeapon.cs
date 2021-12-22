using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpertBowWeapon : WeaponInventoryItem {
    
    public ExpertBowWeapon(Sprite itemImage) : base(itemIdentifier: "expertBow", itemName: "Expert Bow", goldValue: 2000, itemImage, roundsPerMinute: 180, damagePerRound: 10) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "weapon");
    }

    public override void ReverseEffect(GameObject gameManager) {
        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
