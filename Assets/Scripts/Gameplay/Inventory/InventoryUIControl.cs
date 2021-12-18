using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIControl : MonoBehaviour {

    public void ToggleInventory(InputAction.CallbackContext context) {
        gameObject.SetActive(!gameObject.activeSelf);
        Cursor.visible = gameObject.activeSelf;
    }

}
