using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatteredArmourItem : EquippableInventoryItem {
    
    public TatteredArmourItem(Sprite itemImage) : base(itemIdentifier: "tatteredArmour", itemName: "Tattered Armour", goldValue: 50, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.AddArmour(5);

        gameManager.GetComponent<CharacterManager>().GetPlayerInventory().EquipItem(this, "armour");
    }

    public override void ReverseEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.AddArmour(-5);

        PlayerInventory inventory = gameManager.GetComponent<CharacterManager>().GetPlayerInventory();
        inventory.AddItemToInventory(itemIdentifier, 1);
    }

}
