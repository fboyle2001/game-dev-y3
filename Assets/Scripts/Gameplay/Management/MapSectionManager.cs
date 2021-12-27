using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSectionManager : MonoBehaviour {
    
    public GameObject nonPlayableLayer;
    public GameObject mountainPathParent;
    public GameObject zipLineAttack;
    public GameObject originalOrcParent;
    public GameObject finalOrcParent;
    public GameObject seedParent;

    void Start() {
        nonPlayableLayer.SetActive(true);
    }

    public void EnableMountainPathSection() {
        mountainPathParent.SetActive(true);
    }

    public void EnableZipLineAttack() {
        zipLineAttack.SetActive(true);
    }

    public void DisableOriginalOrcCave() {
        originalOrcParent.SetActive(false);
    }

    public void EnableFinalOrcCave() {
        finalOrcParent.SetActive(true);
        seedParent.SetActive(false);
    }

    public void EnableSeedParent() {
        seedParent.SetActive(true);
    }

}
