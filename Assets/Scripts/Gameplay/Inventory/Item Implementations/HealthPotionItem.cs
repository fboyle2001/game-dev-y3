using UnityEngine;

public class HealthPotionItem  : ConsumableInventoryItem {

    private AudioClip potionClip;

    public HealthPotionItem(Sprite itemImage, AudioClip potionClip) : base(itemIdentifier: "healthPotion", itemKey: "item_health_potion_name", goldValue: 100, itemImage) {
        this.potionClip = potionClip;
    }

    public override void ApplyItemEffect(GameObject gameManager) {
        CharacterManager characterManager = gameManager.GetComponent<CharacterManager>();
        characterManager.primary.GetComponent<CharacterStats>().HealAsPercent(0.1f);
        AudioSource.PlayClipAtPoint(potionClip, characterManager.GetActiveCharacter().transform.position, 0.5f);

        if(characterManager.IsSecondaryUnlocked()) {
            characterManager.secondary.GetComponent<CharacterStats>().HealAsPercent(0.1f);
        }
    }
}
