using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    public string itemName;
    public int goldRequired;
    public PlayerAssets assetManager;

    public void Purchase() {
        if(assetManager.gold >= goldRequired) {
            assetManager.gold -= goldRequired;

            if(assetManager.inventoryItems.ContainsKey(itemName)) {
                assetManager.inventoryItems[itemName] += 1;
            } else {
                assetManager.inventoryItems.Add(itemName, 1);
            }
        } else {
            Debug.Log("Not enough gold");
        }
    }
}
