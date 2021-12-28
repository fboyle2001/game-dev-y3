using UnityEngine;

public class DamageRing : EquippableInventoryItem {
    
    public DamageRing(Sprite itemImage) : base(itemIdentifier: "damageRing", itemKey: "item_damage_ring", goldValue: 800, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddDamageMultiplier(0.5f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "ring");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddDamageMultiplier(-0.5f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
