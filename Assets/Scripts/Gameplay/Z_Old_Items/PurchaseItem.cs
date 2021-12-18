using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    public string itemName;
    public int goldRequired;
    public PlayerAssets assetManager;

    public void Purchase() {
        if(assetManager.HasEnoughGold(goldRequired)) {
            assetManager.RemoveGold(goldRequired);
            assetManager.GiveItem(itemName, 1);
        } else {
            Debug.Log("Not enough gold");
        }
    }
}
