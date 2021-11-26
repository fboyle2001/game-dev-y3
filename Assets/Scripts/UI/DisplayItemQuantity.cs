using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayItemQuantity : MonoBehaviour
{
    public string itemName;
    public PlayerAssets assetManager;

    private TMP_Text textComponent;

    void Start() {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update() {
        textComponent.text = "Quantity: " + assetManager.CountItem(itemName);
    }
}
