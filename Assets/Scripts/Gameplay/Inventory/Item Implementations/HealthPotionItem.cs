using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionItem  : ConsumableInventoryItem {

    public HealthPotionItem(Sprite itemImage) : base(itemIdentifier: "healthPotion", itemName: "Health Potion", goldValue: 100, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterManager characterManager = gameManager.GetComponent<CharacterManager>();
        characterManager.primary.GetComponent<CharacterStats>().HealAsPercent(0.1f);

        if(characterManager.IsSecondaryUnlocked()) {
            characterManager.secondary.GetComponent<CharacterStats>().HealAsPercent(0.1f);
        }
    }
}
