using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : MonoBehaviour 
{
    public static Dictionary<string, IItemConsumeAction> itemActions;

    static ConsumeItem() {
        itemActions = new Dictionary<string, IItemConsumeAction>();
        itemActions.Add("healthPotion", new HealthPotionAction(25));
    }

    public string itemName;
    public GameObject gameManager;
    public GameObject player;
    private PlayerAssets assetManager;

    void Start() {
        assetManager = player.GetComponent<PlayerAssets>();
    }

    public void Consume() {
        if(assetManager.CountItem(itemName) <= 0) {
            Debug.Log("No items to consume! Item: " + itemName);
            return;
        }

        assetManager.DepleteItem(itemName, 1);

        if(itemActions.ContainsKey(itemName)) {
            itemActions[itemName].ApplyEffect(gameManager);
        }
    }
}
