using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{

    public GameObject primary;
    public GameObject secondary;

    public GameObject primaryImage;
    public GameObject primaryHealth;

    public GameObject secondaryPanel;
    public GameObject secondaryImage;
    public GameObject secondaryHealth;

    private bool secondaryUnlocked = false;
    private bool primaryActive = true;

    private PlayerInventory playerInventory;

    void OnEnable() {
        playerInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<PlayerInventory>();
        // Automatically updates the UI with the health of each character
        primary.GetComponent<CharacterStats>().RegisterHealthUpdateListener(new System.Action<CharacterStats, float>((stats, change) => {
            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            primaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        }));

        secondary.GetComponent<CharacterStats>().RegisterHealthUpdateListener(new System.Action<CharacterStats, float>((stats, change) => {
            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            primaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        }));
    }

    public PlayerInventory GetPlayerInventory() {
        return playerInventory;
    }

    public void DisplaySecondaryPanel(bool display) {
        secondaryPanel.SetActive(display && secondaryUnlocked);
    }

    public void SetSecondaryUnlocked(bool unlocked) {
        secondaryUnlocked = unlocked;
    }

    public bool IsPrimaryActive() {
        return primaryActive;
    }

}
