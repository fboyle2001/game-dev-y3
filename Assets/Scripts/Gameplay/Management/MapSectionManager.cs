using UnityEngine;

/**
* Handles revealing of map sections as story progresses
**/
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

    // These just activate specific areas of the map

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
