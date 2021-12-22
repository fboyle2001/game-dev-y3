using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    
    [Header("Resource Gains")]
    public GameObject xpText;
    public GameObject xpPanel;
    public GameObject levelUpText;
    public GameObject levelUpPanel;
    public GameObject goldText;
    public GameObject goldPanel;
    public GameObject healthText;
    public GameObject healthPanel;
    public GameObject damageMultText;
    public GameObject damageMultPanel;
    public GameObject armourText;
    public GameObject armourPanel;
    public GameObject maxHealthText;
    public GameObject maxHealthPanel;
    public GameObject regenText;
    public GameObject regenPanel;

    [Header("Interaction")]
    public GameObject inventoryPanel;
    public GameObject shopPanel;
    public GameObject interactionText;

    [Header("Characters")]
    public GameObject primaryImage;
    public GameObject primaryHealth;

    public GameObject secondaryPanel;
    public GameObject secondaryImage;
    public GameObject secondaryHealth;

    private float xpGain;
    private int goldGain;
    private int healthGain;
    private float damageMultGain;
    private int armourGain;
    private int maxHealthGain;
    private float regenGain;

    void Start() {
        GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);
        GetComponent<PlayerResources>().RegisterResourceUpdateListener(OnResourceUpdate);
        GetComponent<PlayerResources>().RegisterLevelUpListener(OnLevelUp);

        // Automatically updates the UI with the health of each character
        GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>().RegisterHealthUpdateListener((stats, change) => {
            if(change > 0) {
                OnHealthGain(change);
            }

            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            primaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        });

        GetComponent<CharacterManager>().secondary.GetComponent<CharacterStats>().RegisterHealthUpdateListener((stats, change) => {
            if(change > 0) {
                OnHealthGain(change);
            }

            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            secondaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        });

        DisplaySecondaryPanel(false);
        HideAllGainPanels();
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

    private void OnHealthGain(float gain) {

    }

    private void OnResourceUpdate(PlayerResources resources) {

    }

    private void OnLevelUp(int newLevel) {

    }

    private void HideXPPanel() {

    }

    private void HideLevelUpPanel() {

    }

    private void HideGoldPanel() {

    }

    private void HideHealthPanel() {

    }

    private void HideDamageMultPanel() {

    }

    private void HideArmourPanel() {

    }

    private void HideMaxHealthPanel() {

    }

    private void HideRegenPanel() {

    }

    private void HideAllGainPanels() {
        xpPanel.SetActive(false); //d
        levelUpPanel.SetActive(false); //d
        goldPanel.SetActive(false); //d
        healthPanel.SetActive(false); //d
        damageMultPanel.SetActive(false); //d
        armourPanel.SetActive(false); //d
        maxHealthPanel.SetActive(false); //d
        regenPanel.SetActive(false); //d
    }

}
