using UnityEngine;

public class HealRing : EquippableInventoryItem {
    
    public HealRing(Sprite itemImage) : base(itemIdentifier: "healRing", itemKey: "item_heal_ring_name", goldValue: 800, itemImage) {}

    public override void ApplyItemEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddRegenPerSecond(0.8f);

        gameManager.GetComponent<PlayerInventory>().EquipItem(this, "ring");
    }

    public override void ReverseEffect(GameObject gameManager) {
        PlayerStats playerStats = gameManager.GetComponent<PlayerStats>();
        playerStats.AddRegenPerSecond(-0.8f);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory(itemIdentifier, 1);
    }

}
