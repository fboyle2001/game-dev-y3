using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealthPotion : ConsumableInventoryItem {

    public FullHealthPotion(Sprite itemImage) : base(itemIdentifier: "fullHealthPotion", itemKey: "item_full_health_potion_name", goldValue: 450, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterManager characterManager = gameManager.GetComponent<CharacterManager>();
        characterManager.primary.GetComponent<CharacterStats>().HealAsPercent(1);

        if(characterManager.IsSecondaryUnlocked()) {
            characterManager.secondary.GetComponent<CharacterStats>().HealAsPercent(1);
        }
    }
}
