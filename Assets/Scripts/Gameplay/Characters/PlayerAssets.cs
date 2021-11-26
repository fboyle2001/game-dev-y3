using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssets : MonoBehaviour
{

    public int initialGold = 0;
    public float initialExperience = 0;

    private Dictionary<string, int> inventoryItems;

    private int gold;
    private float experience;
    private float spentExperience;

    void Start() {
        inventoryItems = new Dictionary<string, int>();
        gold = initialGold;
        experience = initialExperience;
        spentExperience = 0;
    }

    public int CountItem(string itemName) {
        return inventoryItems.ContainsKey(itemName) ? inventoryItems[itemName] : 0;
    }

    public void DepleteItem(string itemName, int amount) {
        if(inventoryItems.ContainsKey(itemName)) {
            inventoryItems[itemName] = inventoryItems[itemName] - amount < 0 ? 0 : inventoryItems[itemName] - amount;
        }
    }

    public void GiveItem(string itemName, int amount) {
        if(inventoryItems.ContainsKey(itemName)) {
            inventoryItems[itemName] += 1;
        } else {
            inventoryItems.Add(itemName, 1);
        }
    }

    public int GetGold() {
        return gold;
    }

    public bool HasEnoughGold(int requiredAmount) {
        return this.gold >= requiredAmount;
    }

    public void RemoveGold(int amount) {
        this.gold = gold - amount > 0 ? gold - amount : 0;
    }

    public void AddGold(int amount) {
        this.gold += amount;
    }

}
