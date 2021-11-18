using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssets : MonoBehaviour
{

    public float gold = 0;
    public float experience = 0;
    public float experienceLevel = 0;

    public Dictionary<string, int> inventoryItems;

    void Start() {
        inventoryItems = new Dictionary<string, int>();
    }

    void Update() {
        
    }

}
