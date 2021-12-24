using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponInventoryItem : EquippableInventoryItem {

    public readonly float roundsPerMinute;
    public readonly float damagePerRound;

    public WeaponInventoryItem(string itemIdentifier, string itemKey, int goldValue, Sprite itemImage, float roundsPerMinute, float damagePerRound) 
    : base(itemIdentifier, itemKey, goldValue, itemImage) {
        this.roundsPerMinute = roundsPerMinute;
        this.damagePerRound = damagePerRound;
    }

}
