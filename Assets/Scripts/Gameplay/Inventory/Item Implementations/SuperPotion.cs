using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPotion : ConsumableInventoryItem {

    public SuperPotion(Sprite itemImage) : base(itemIdentifier: "superPotion", itemName: "Super Potion", goldValue: 300, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterManager characterManager = gameManager.GetComponent<CharacterManager>();
        characterManager.primary.GetComponent<CharacterStats>().HealAsPercent(0.7f);

        if(characterManager.IsSecondaryUnlocked()) {
            characterManager.secondary.GetComponent<CharacterStats>().HealAsPercent(0.7f);
        }
    }
}
