using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionAction : IItemConsumeAction
{

    private float healAmount;

    public HealthPotionAction(float healAmount) {
        this.healAmount = healAmount;
    }

    public void ApplyEffect(GameObject gameManager) {
        gameManager.GetComponent<ActiveCharacterManager>().GetActiveCharacter().GetComponent<CharacterStatManager>().Heal(healAmount);
    }

}
