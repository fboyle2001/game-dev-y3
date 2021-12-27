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
    public GameObject damageMultText;
    public GameObject damageMultPanel;
    public GameObject armourText;
    public GameObject armourPanel;
    public GameObject maxHealthText;
    public GameObject maxHealthPanel;
    public GameObject regenText;
    public GameObject regenPanel;
    public float expireTime = 3f;

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

    [Header("Other")]
    public GameObject crosshair;
    public GameObject pausePanel;

    private float xpGain = 0;
    private int goldGain = 0;
    private float damageMultGain = 0;
    private float armourGain = 0;
    private float maxHealthGain = 0;
    private float regenGain = 0;

    private LocaleManager localeManager;

    void Awake() {
        localeManager = GetComponent<LocaleManager>();
    }

    void Start() {
        GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);
        GetComponent<PlayerResources>().RegisterResourceUpdateListener(OnResourceUpdate);
        GetComponent<PlayerResources>().RegisterLevelUpListener(OnLevelUp);
        GetComponent<PlayerStats>().RegisterStatChangeListener(OnGlobalStatsUpdate);

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

    public void SetCrosshairActive(bool active) {
        crosshair.SetActive(active);
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

    private void OnGlobalStatsUpdate(PlayerStats globalStats, float maxHealthChange, float dmgChange, float regenChange, float armourChange) {
        if(maxHealthChange != 0) {
            CancelInvoke("HideMaxHealthPanel");
            maxHealthGain += maxHealthChange;
            string symbol = maxHealthGain >= 0 ? "+" : "-";
            maxHealthText.GetComponent<TMP_Text>().text = symbol + " " + (Mathf.Abs(maxHealthGain) * 100).ToString("0") + "% " + localeManager.GetString("ui_gain_max_health");
            maxHealthPanel.SetActive(true);
            Invoke("HideMaxHealthPanel", expireTime);
        }

        if(dmgChange != 0) {
            CancelInvoke("HideDamageMultPanel");
            damageMultGain += dmgChange;
            string symbol = damageMultGain >= 0 ? "+" : "-";
            damageMultText.GetComponent<TMP_Text>().text = symbol + " " + (Mathf.Abs(damageMultGain) * 100) + "% " + localeManager.GetString("ui_gain_dmg_mult");
            damageMultPanel.SetActive(true);
            Invoke("HideDamageMultPanel", expireTime);
        }

        if(regenChange != 0) {
            CancelInvoke("HideRegenPanel");
            regenGain += regenChange;
            string symbol = regenGain >= 0 ? "+" : "-";
            regenText.GetComponent<TMP_Text>().text = symbol + " " + Mathf.Abs(regenGain).ToString("0.0") + " " + localeManager.GetString("ui_gain_regen");
            regenPanel.SetActive(true);
            Invoke("HideRegenPanel", expireTime);
        }

        if(armourChange != 0) {
            CancelInvoke("HideArmourPanel");
            armourGain += armourChange;
            string symbol = armourGain >= 0 ? "+" : "-";
            armourText.GetComponent<TMP_Text>().text = symbol + " " + Mathf.Abs(armourGain) + " " + localeManager.GetString("ui_gain_armour");
            armourPanel.SetActive(true);
            Invoke("HideArmourPanel", expireTime);
        }
    }

    private void OnResourceUpdate(PlayerResources resources, float xpChange, int goldChange) {
        if(goldChange != 0) {
            CancelInvoke("HideGoldPanel");
            goldGain += goldChange;
            string symbol = goldGain >= 0 ? "+" : "-";
            goldText.GetComponent<TMP_Text>().text = symbol + " " + Mathf.Abs(goldGain) + " G";
            goldPanel.SetActive(true);
            Invoke("HideGoldPanel", expireTime);
        }

        if(xpChange != 0) {
            CancelInvoke("HideXPPanel");
            xpGain += xpChange;
            string symbol = goldGain >= 0 ? "+" : "-";
            xpText.GetComponent<TMP_Text>().text = symbol + " " + Mathf.Abs(xpGain) + " XP";
            xpPanel.SetActive(true);
            Invoke("HideXPPanel", expireTime);
        }
    }

    private void OnLevelUp(int newLevel) {
        CancelInvoke("HideLevelUpPanel");
        levelUpText.GetComponent<TMP_Text>().text = localeManager.GetString("ui_level_up") + " (" + newLevel + ")";
        levelUpPanel.SetActive(true);
        Invoke("HideLevelUpPanel", expireTime);
    }

    private void HideXPPanel() {
        xpGain = 0;
        xpPanel.SetActive(false);
    }

    private void HideLevelUpPanel() {
        levelUpPanel.SetActive(false);
    }

    private void HideGoldPanel() {
        goldGain = 0;
        goldPanel.SetActive(false);
    }

    private void HideDamageMultPanel() {
        damageMultGain = 0;
        damageMultPanel.SetActive(false);
    }

    private void HideArmourPanel() {
        armourGain = 0;
        armourPanel.SetActive(false);
    }

    private void HideMaxHealthPanel() {
        maxHealthGain = 0;
        maxHealthPanel.SetActive(false);
    }

    private void HideRegenPanel() {
        regenGain = 0;
        regenPanel.SetActive(false);
    }

    private void HideAllGainPanels() {
        xpPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        goldPanel.SetActive(false);
        damageMultPanel.SetActive(false);
        armourPanel.SetActive(false);
        maxHealthPanel.SetActive(false);
        regenPanel.SetActive(false);
    }

}
