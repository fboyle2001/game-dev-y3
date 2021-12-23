using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingOverrides : MonoBehaviour {

    void Start() {
        Invoke("Delayed", 5);
    }

    void Delayed() {
        // GetComponent<CharacterManager>().UnlockSecondary();
        GetComponent<PlayerResources>().AddGold(1000000);
        GetComponent<PlayerInventory>().AddItemToInventory("craftedBow", 1);
        GetComponent<MapSectionManager>().EnableMountainPathSection();
    }

}
