using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputActions : MonoBehaviour
{
    
    public GameObject inventoryCanvas; 

    void Start() {
        inventoryCanvas.SetActive(false);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.I)) {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }
}
