using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealthPotion : ConsumableInventoryItem {

    private AudioClip potionClip;

    public FullHealthPotion(Sprite itemImage, AudioClip potionClip) : base(itemIdentifier: "fullHealthPotion", itemKey: "item_full_health_potion_name", goldValue: 450, itemImage) {
        this.potionClip = potionClip;
    }

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterManager characterManager = gameManager.GetComponent<CharacterManager>();
        characterManager.primary.GetComponent<CharacterStats>().HealAsPercent(1);
        AudioSource.PlayClipAtPoint(potionClip, characterManager.GetActiveCharacter().transform.position, 0.5f);

        if(characterManager.IsSecondaryUnlocked()) {
            characterManager.secondary.GetComponent<CharacterStats>().HealAsPercent(1);
        }
    }
}
