using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingOverrides : MonoBehaviour {

    void Start() {
        Invoke("Delayed", 2);
    }

    void Delayed() {
        // GetComponent<ObjectiveManager>().AddObjective("freeCat", "a", "a", new ObjectiveManager.RewardEntry(0, 0));
        // Debug.Log("added");
    }

}
