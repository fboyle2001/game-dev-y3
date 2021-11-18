using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionICA : IItemConsumeAction
{

    private float healAmount;

    public HealthPotionICA(float healAmount) {
        this.healAmount = healAmount;
    }

    public void ApplyEffect(GameObject gameManager) {
        gameManager.GetComponent<SwitchPlayer>().GetActiveCharacter().GetComponent<HealthManager>().AddHealth(healAmount);
    }

}
