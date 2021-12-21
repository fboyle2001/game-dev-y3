using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    
    public GameObject inventoryPanel;
    public GameObject shopPanel;
    public GameObject interactionText;

    public GameObject primaryImage;
    public GameObject primaryHealth;

    public GameObject secondaryPanel;
    public GameObject secondaryImage;
    public GameObject secondaryHealth;

    void OnEnable() {
        GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);

        // Automatically updates the UI with the health of each character
        GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>().RegisterHealthUpdateListener((stats, change) => {
            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            primaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        });

        GetComponent<CharacterManager>().secondary.GetComponent<CharacterStats>().RegisterHealthUpdateListener((stats, change) => {
            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            secondaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        });

        DisplaySecondaryPanel(false);
    }

    public void DisplaySecondaryPanel(bool display) {
        secondaryPanel.SetActive(display);
    }

    public void CloseAllOpenUI() {
        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
        interactionText.SetActive(false);
        Cursor.visible = false;
    }

    private void OnActiveCharacterChange(GameObject newActive) {
        if(newActive.name == "Primary") {
            GetComponent<WeaponManager>().SwitchCharacterMode(true);
            newActive.GetComponent<ShowInteractText>().RecheckInterability();
        } else {
            GetComponent<WeaponManager>().SwitchCharacterMode(false);
        }

        CloseAllOpenUI();
    }

}
