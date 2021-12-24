using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawsItem : WeaponInventoryItem {
    
    public ClawsItem(Sprite itemImage) : base(itemIdentifier: "claws", itemKey: "item_claws_name", goldValue: 0, itemImage, roundsPerMinute: 120, damagePerRound: 30) {}

    public override void ApplyItemEffect(GameObject gameManager) {}

    public override void ReverseEffect(GameObject gameManager) {}

}
