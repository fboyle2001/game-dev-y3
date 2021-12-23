using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSectionManager : MonoBehaviour {
    
    public GameObject mountainPathParent;

    public void EnableMountainPathSection() {
        mountainPathParent.SetActive(true);
    }

}
