using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustedBowItem : WeaponInventoryItem {
    
    public RustedBowItem(Sprite itemImage) : base(itemIdentifier: "rustedBow", itemName: "Rusted Bow", goldValue: 100, itemImage, roundsPerMinute: 120, damagePerRound: 5) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats stats = gameManager.GetComponent<PlayerStats>();
        stats.AddDamageMultiplier(0.02f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "weapon");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats stats = gameManager.GetComponent<PlayerStats>();
        stats.AddDamageMultiplier(-0.02f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
