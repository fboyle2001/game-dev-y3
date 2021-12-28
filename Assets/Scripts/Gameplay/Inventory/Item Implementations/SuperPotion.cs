using UnityEngine;

public class SuperPotion : ConsumableInventoryItem {

    private AudioClip potionClip;

    public SuperPotion(Sprite itemImage, AudioClip potionClip) : base(itemIdentifier: "superPotion", itemKey: "item_super_potion_name", goldValue: 300, itemImage) {
        this.potionClip = potionClip;
    }

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterManager characterManager = gameManager.GetComponent<CharacterManager>();
        characterManager.primary.GetComponent<CharacterStats>().HealAsPercent(0.7f);
        AudioSource.PlayClipAtPoint(potionClip, characterManager.GetActiveCharacter().transform.position, 0.5f);

        if(characterManager.IsSecondaryUnlocked()) {
            characterManager.secondary.GetComponent<CharacterStats>().HealAsPercent(0.7f);
        }
    }
}
