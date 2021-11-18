using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSecondaryUI : MonoBehaviour
{
    void Update() {
        if(gameObject.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            this.CloseUI();
        }
    }

    public void OpenUI() {
        if(gameObject.activeSelf) {
            return;
        }

        gameObject.SetActive(true);
    }

    public void CloseUI() {
        if(!gameObject.activeSelf) {
            return;
        }

        gameObject.SetActive(false);
    }

}
