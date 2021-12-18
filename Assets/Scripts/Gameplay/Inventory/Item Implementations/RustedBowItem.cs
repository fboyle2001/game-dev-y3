using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustedBowItem : EquippableInventoryItem {
    
    public RustedBowItem(Sprite itemImage) : base(itemIdentifier: "rustedBow", itemName: "Rusted Bow", goldValue: 100, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.AddDamageMultiplier(0.02f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "weapon");
    }

    public override void ReverseEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.AddDamageMultiplier(-0.02f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
