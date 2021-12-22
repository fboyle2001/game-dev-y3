using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PurchaseSlotManager : MonoBehaviour
{

    public string itemIdentifier;
    public float markupScalar = 1;
    public GameObject purchaseButton;
    public GameObject nameText;
    public GameObject buttonText;
    public GameObject image;

    private GameObject gameManager;
    private PlayerInventory playerInventory;
    private PlayerResources playerResources;
    private InventoryItem heldItem;
    private int goldCost;
    private bool ready = false;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        playerInventory = gameManager.GetComponent<PlayerInventory>();
        playerResources = gameManager.GetComponent<PlayerResources>();
    }

    void OnEnable() {
        playerResources.RegisterResourceUpdateListener((resources) => {
            CheckIfPurchasable();
        });

        playerInventory.RegisterItemChangeListener((inventory) => {
            CheckIfPurchasable();
        });
    }

    void Start() {
        InventoryItem item;

        if(!PlayerInventory.registeredItems.TryGetValue(itemIdentifier, out item)) {
            gameObject.SetActive(false);
            return;
        }

        heldItem = item;
        goldCost = Mathf.RoundToInt(heldItem.goldValue * markupScalar);
        ready = true;
        nameText.GetComponent<TMP_Text>().text = heldItem.itemName;
        image.GetComponent<Image>().sprite = heldItem.itemImage;
        playerResources.UpdateGold(100000);
        CheckIfPurchasable();
    }

    private bool CheckIfPurchasable() {
        if(!ready) return false;
        // Purchasable if they have enough gold and enough space in their inventory

        int playerGold = playerResources.GetGold();

        bool hasSpace = playerInventory.HasSpace(itemIdentifier, 1);
        bool hasGold = playerGold >= goldCost;

        if(hasSpace && hasGold) {
            buttonText.GetComponent<TMP_Text>().text = "Buy (" + heldItem.goldValue + "G)";
            buttonText.GetComponent<TMP_Text>().color = Color.black;
            purchaseButton.GetComponent<Button>().interactable = true;
            return true;
        } else if (!hasSpace) {
            buttonText.GetComponent<TMP_Text>().text = "No Space!";
            buttonText.GetComponent<TMP_Text>().color = Color.red;
            purchaseButton.GetComponent<Button>().interactable = false;
        } else {
            buttonText.GetComponent<TMP_Text>().text = heldItem.goldValue + "G";
            buttonText.GetComponent<TMP_Text>().color = Color.red;
            purchaseButton.GetComponent<Button>().interactable = false;
        }

        return false;
    }

    public void BuyItem() {
        bool purchasable = CheckIfPurchasable();
        if(!purchasable) return;

        playerResources.UpdateGold(-goldCost);
        playerInventory.AddItemToInventory(heldItem.itemIdentifier, 1);

        CheckIfPurchasable();
    }

}
