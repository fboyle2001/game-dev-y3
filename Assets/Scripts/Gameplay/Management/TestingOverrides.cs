using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingOverrides : MonoBehaviour {

    void Start() {
        Invoke("Delayed", 2);
    }

    void Delayed() {
        // GetComponent<CharacterManager>().UnlockSecondary();
        GetComponent<PlayerResources>().AddGold(1000000);
    }

}
