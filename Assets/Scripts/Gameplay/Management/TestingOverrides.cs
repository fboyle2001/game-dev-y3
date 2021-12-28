using UnityEngine;

/**
* Used to grant resources, stats, objectives and items in play testing
**/
public class TestingOverrides : MonoBehaviour {

    void Start() {
        // Invoke("Delayed", 0.2f);
    }

    void Delayed() {
        // GetComponent<CharacterManager>().UnlockSecondary();
        // GetComponent<PlayerResources>().AddGold(99999);
        // GetComponent<PlayerInventory>().AddItemToInventory("expertBow", 1);
        // GetComponent<UIManager>().inventoryPanel.SetActive(true);
        // GetComponent<ObjectiveManager>().AddObjective("findSeeds", "Find Seeds", "crossZiplineA", new ObjectiveManager.RewardEntry(0, 0));
        // GetComponent<CharacterManager>().primary.GetComponent<ShowInteractText>().RecheckInterability();
        // GetComponent<MapSectionManager>().EnableMountainPathSection();

        // GetComponent<ObjectiveManager>().AddObjective("plantSeeds", "Plant Seeds", "a", new ObjectiveManager.RewardEntry(0, 0));
        // GetComponent<PlayerStats>().AddArmour(200f);
        // GetComponent<PlayerStats>().AddDamageMultiplier(2f);
        // GetComponent<PlayerStats>().AddMaxHealthMultiplier(200f);
        // GetComponent<PlayerStats>().AddRegenPerSecond(200f);
        // GetComponent<PlayerInventory>().AddItemToInventory("expertBow", 1);
        // GetComponent<PlayerInventory>().AddItemToInventory("crystalArmour", 1);
        // GetComponent<PlayerInventory>().AddItemToInventory("godAmulet", 1);
    }

}
