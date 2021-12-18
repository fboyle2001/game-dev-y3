using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronArmourItem : EquippableInventoryItem {

    public IronArmourItem(Sprite itemImage) : base(itemIdentifier: "ironArmour", itemName: "Iron Armour", goldValue: 300, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.AddArmour(10);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "armour");
    }

    public override void ReverseEffect(GameObject gameManager) {
        CharacterStats stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        stats.AddArmour(-10);
        
        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
