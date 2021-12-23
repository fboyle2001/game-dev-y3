using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingOverrides : MonoBehaviour {

    void Start() {
        Invoke("Delayed", 1);
    }

    void Delayed() {
        // GetComponent<CharacterManager>().UnlockSecondary();
        GetComponent<PlayerResources>().AddGold(1000000);
        GetComponent<PlayerInventory>().AddItemToInventory("craftedBow", 1);
        GetComponent<ObjectiveManager>().AddObjective("crossZiplineA", "Cross Zipline", "crossZiplineA", new ObjectiveManager.RewardEntry(0, 0));
        // GetComponent<MapSectionManager>().EnableMountainPathSection();
    }

}
