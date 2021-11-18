using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : MonoBehaviour
{
    public static Dictionary<string, IItemConsumeAction> itemConsumers;

    static ConsumeItem() {
        itemConsumers = new Dictionary<string, IItemConsumeAction>();
        itemConsumers.Add("healthPotion", new HealthPotionICA(2));
    }

    public string itemName;
    public GameObject gameManager;
    public GameObject player;
    private PlayerAssets assetManager;

    void Start() {
        assetManager = player.GetComponent<PlayerAssets>();
    }

    public void Consume() {
        if(!assetManager.inventoryItems.ContainsKey(itemName) || assetManager.inventoryItems[itemName] <= 0) {
            Debug.Log("No items to consume! Item: " + itemName);
            return;
        }

        assetManager.inventoryItems[itemName] -= 1;

        if(itemConsumers.ContainsKey(itemName)) {
            itemConsumers[itemName].ApplyEffect(gameManager);
        }
    }
}
