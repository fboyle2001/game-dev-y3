using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSectionManager : MonoBehaviour {
    
    public GameObject mountainPathParent;
    public GameObject zipLineAttack;

    public void EnableMountainPathSection() {
        mountainPathParent.SetActive(true);
    }

    public void EnableZipLineAttack() {
        zipLineAttack.SetActive(true);
    }

}
