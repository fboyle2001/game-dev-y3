using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
* Represents and manages the players' inventory
**/
public class PlayerInventory : MonoBehaviour {

    // All items need to be registered against their identifier for ease of access
    public static Dictionary<string, InventoryItem> registeredItems = new Dictionary<string, InventoryItem>();

    public static void RegisterItem(InventoryItem item) {
        registeredItems.Add(item.itemIdentifier, item);
    }

    public AudioClip potionClip;
    public GameObject weaponSlot;
    public GameObject armourSlot;
    public GameObject ringSlot;
    public InventorySlot[] inventorySlots;
    public GameObject emptyText;

    private GameObject gameManager;

    private WeaponInventoryItem currentWeapon = null;
    private EquippableInventoryItem currentArmour = null;
    private EquippableInventoryItem currentRing = null;

    private List<System.Action<PlayerInventory>> equipUpdateListeners = new List<System.Action<PlayerInventory>>();
    private List<System.Action<PlayerInventory>> itemChangeListeners = new List<System.Action<PlayerInventory>>();

    private void RegisterItems() {
        // Load sprite sheet and store them so they can be accessed by their names
        Sprite[] itemSprites = Resources.LoadAll<Sprite>("Sprites/item_sprite_sheet");
        Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();

        foreach(Sprite sprite in itemSprites) {
            spriteMap.Add(sprite.name, sprite);
        }
        
        // Register all of the items along with their sprite
        RegisterItem(new ClawsItem(spriteMap["claws"]));
        RegisterItem(new CraftedBowWeapon(spriteMap["craftedBow"]));
        RegisterItem(new CrystalArmour(spriteMap["crystalArmour"]));
        RegisterItem(new DamageRing(spriteMap["damageRing"]));
        RegisterItem(new ExpertBowWeapon(spriteMap["expertBow"]));
        RegisterItem(new FullHealthPotion(spriteMap["fullHealthPotion"], potionClip));
        RegisterItem(new GodAmuletRing(spriteMap["godAmulet"]));
        RegisterItem(new HealRing(spriteMap["healRing"]));
        RegisterItem(new HealthPotionItem(spriteMap["healthPotion"], potionClip));
        RegisterItem(new PlatedArmour(spriteMap["platedArmour"]));
        RegisterItem(new RippedArmour(spriteMap["rippedArmour"]));
        RegisterItem(new SorcerersPotion(spriteMap["sorcerersPotion"]));
        RegisterItem(new SuperPotion(spriteMap["superPotion"], potionClip));
        RegisterItem(new WornBowWeapon(spriteMap["wornBow"]));
    }

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        // Register all items once
        RegisterItems();
    }

    void OnEnable() {
        foreach(InventorySlot slot in inventorySlots) {
            slot.SetVisible(false);
        }

        emptyText.SetActive(true);
    }

    // Other objects can list to equipping of items
    public void RegisterEquipUpdateListener(System.Action<PlayerInventory> action) {
        equipUpdateListeners.Add(action);
        PropagateEquipEvent();
    }

    // Other objects can list to when items are gained or lost
    public void RegisterItemChangeListener(System.Action<PlayerInventory> action) {
        itemChangeListeners.Add(action);
        PropagateItemChangeEvent();
    }

    public void AddItemToInventory(string itemIdentifier, int quantity) {
        // First check if we have an item slot occupied by this
        bool inventoryFull = true;
        InventorySlot occupiedSlot = null;
        InventorySlot firstEmptySlot = null;

        foreach(InventorySlot slot in inventorySlots) {
            if(slot.HasOccupyingItem()) {
                if(slot.GetOccupyingItem().itemIdentifier == itemIdentifier) {
                    occupiedSlot = slot;
                    break;
                }
            } else {
                inventoryFull = false;
                
                if(firstEmptySlot == null) {
                    firstEmptySlot = slot;
                    // don't break here as we want to check if they have the item in another slot
                }
            }
        }

        emptyText.SetActive(false);
        InventoryItem identifiedItem = registeredItems[itemIdentifier];

        if(occupiedSlot == null && inventoryFull) {
            // Let them know their inventory is full and pay them for the excess
            int goldPayout = quantity * identifiedItem.goldValue;
            gameManager.GetComponent<PlayerResources>().AddGold(goldPayout);
        } else if (occupiedSlot != null) {
            // Add to the current slot and pay for their excess
            int realQuantity = occupiedSlot.GetQuantity() + quantity > occupiedSlot.GetOccupyingItem().maxQuantity ?
                    occupiedSlot.GetOccupyingItem().maxQuantity - occupiedSlot.GetQuantity() : quantity;
            int goldPayout = (quantity - realQuantity) * identifiedItem.goldValue; 

            gameManager.GetComponent<PlayerResources>().AddGold(goldPayout);
            occupiedSlot.UpdateQuantity(realQuantity);
        } else {
            // Occupy a new slot
            int realQuantity = quantity > identifiedItem.maxQuantity ? identifiedItem.maxQuantity : quantity;
            int goldPayout = (quantity - realQuantity) * identifiedItem.goldValue;
            gameManager.GetComponent<PlayerResources>().AddGold(goldPayout);

            firstEmptySlot.SetOccupyingItem(identifiedItem, realQuantity);
            firstEmptySlot.SetVisible(true);
        }

        PropagateItemChangeEvent();
    }

    public void PerformEmptyChecks() {
        bool empty = true;

        foreach(InventorySlot slot in inventorySlots) {
            if(slot.HasOccupyingItem()) {
                empty = false;
            }
        }

        // Display some text if the inventory is empty
        if(empty) {
            emptyText.SetActive(true);
        }
    }

    // Find the item slot occupied by the item with itemIdentifier
    public InventorySlot GetItemSlot(string itemIdentifier) {
        foreach(InventorySlot slot in inventorySlots) {
            if(slot.HasOccupyingItem()) {
                if(slot.GetOccupyingItem().itemIdentifier == itemIdentifier) {
                    return slot;
                }
            }
        }

        return null;
    }

    // Check if the inventory has space for new item types
    public bool AcceptingNewItemTypes() {
        foreach(InventorySlot slot in inventorySlots) {
            if(!slot.HasOccupyingItem()) {
                return true;
            }
        }

        return false;
    }

    // Check if the inventory has space for quantity amount of this specific type
    public bool HasSpace(string itemIdentifier, int quantity) {
        InventoryItem item = registeredItems[itemIdentifier];

        if(item == null) {
            return false;
        }

        InventorySlot slot = GetItemSlot(itemIdentifier);

        if(slot != null) {
            if(slot.GetQuantity() + quantity > item.maxQuantity) {
                return false;
            }

            return true;
        }

        // We didn't already have this item so it will need to occupy a new slot
        // check if this is an option
        return AcceptingNewItemTypes();
    }

    public void EquipItem(EquippableInventoryItem item, string slot) {
        // Equip an item to the correct slot
        switch(slot) {
            case "weapon":
                if(currentWeapon != null) {
                    // Remove any stats and effects granted by this item
                    currentWeapon.ReverseEffect(gameManager);
                }

                weaponSlot.GetComponent<Image>().sprite = item.itemImage;
                // Cast to a weapon item, should be fine as we registered all items statically
                // rather than on the fly
                currentWeapon = item as WeaponInventoryItem;
                break;
            case "armour":
                if(currentArmour != null) {
                    // Remove any stats and effects granted by this item
                    currentArmour.ReverseEffect(gameManager);
                }

                armourSlot.GetComponent<Image>().sprite = item.itemImage;
                currentArmour = item;
                break;
            case "ring":
                if(currentRing != null) {
                    // Remove any stats and effects granted by this item
                    currentRing.ReverseEffect(gameManager);
                }

                ringSlot.GetComponent<Image>().sprite = item.itemImage;
                currentRing = item;
                break;
            default:
                return;
        }

        PropagateEquipEvent();
    }

    // Check if they have this item already equipped
    public bool IsEquipped(string itemIdentifier) {
        if(currentWeapon != null && currentWeapon.itemIdentifier == itemIdentifier) {
            return true;
        }

        if(currentArmour != null && currentArmour.itemIdentifier == itemIdentifier) {
            return true;
        }

        if(currentRing != null && currentRing.itemIdentifier == itemIdentifier) {
            return true;
        }

        return false;
    }

    private void PropagateEquipEvent() {
        equipUpdateListeners.ForEach(action => action(this));
    }

    private void PropagateItemChangeEvent() {
        itemChangeListeners.ForEach(action => action(this));
    }

    public WeaponInventoryItem GetCurrentWeapon() {
        return currentWeapon;
    }

}
