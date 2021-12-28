using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
* Represents a slot in the shop
**/
public class PurchaseSlotManager : MonoBehaviour {

    public string itemIdentifier;
    public float markupScalar = 1;
    public GameObject purchaseButton;
    public GameObject nameText;
    public GameObject buttonText;
    public GameObject image;

    [Header("Audio")]
    public AudioClip successClip;
    public AudioClip notAllowedClip;

    private GameObject gameManager;
    private PlayerInventory playerInventory;
    private PlayerResources playerResources;
    private InventoryItem heldItem;
    private AudioSource audioSource;
    private int goldCost;
    private bool ready = false;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        playerInventory = gameManager.GetComponent<PlayerInventory>();
        playerResources = gameManager.GetComponent<PlayerResources>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() {
        // Register handlers
        playerResources.RegisterResourceUpdateListener((resources, discard_a, discard_b) => {
            CheckIfPurchasable();
        });

        playerInventory.RegisterItemChangeListener((inventory) => {
            CheckIfPurchasable();
        });

        if(ready) {
            UpdateName();
        }
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
        UpdateName();
        image.GetComponent<Image>().sprite = heldItem.itemImage;
        CheckIfPurchasable();
    }

    private void UpdateName() {
        if(heldItem == null) return;
        nameText.GetComponent<TMP_Text>().text = heldItem.GetItemName(gameManager.GetComponent<LocaleManager>());
    }

    private bool CheckIfPurchasable() {
        if(!ready) return false;

        // Purchasable if they have enough gold and enough space in their inventory
        // and they don't have it equipped

        int playerGold = playerResources.GetGold();

        bool hasSpace = playerInventory.HasSpace(itemIdentifier, 1) && !playerInventory.IsEquipped(itemIdentifier);
        bool hasGold = playerGold >= goldCost;

        if(hasSpace && hasGold) {
            buttonText.GetComponent<TMP_Text>().text = heldItem.goldValue + "G";
            buttonText.GetComponent<TMP_Text>().color = Color.black;
            purchaseButton.GetComponent<Button>().interactable = true;
            return true;
        } else if (!hasSpace) {
            buttonText.GetComponent<TMP_Text>().text = gameManager.GetComponent<LocaleManager>().GetString("ui_ps_no_space");
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

        if(!purchasable) {
            audioSource.PlayOneShot(notAllowedClip);
            return;
        }
        
        // Buy the item and add it to their inventory

        playerResources.AddGold(-goldCost);
        playerInventory.AddItemToInventory(heldItem.itemIdentifier, 1);
        audioSource.PlayOneShot(successClip);

        CheckIfPurchasable();
    }

}
