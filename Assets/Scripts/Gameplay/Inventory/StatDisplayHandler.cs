using UnityEngine;
using TMPro;

/**
* Updates the player's stats that are displayed in the inventory panel
* this also provides real-time updates as they equip and use items in the
* inventory panel 
**/
public class StatDisplayHandler : MonoBehaviour {

    public GameObject armourText;
    public GameObject regenText;
    public GameObject maxHealthText;
    public GameObject damageMultiplierText;

    private GameObject gameManager;
    private CharacterManager characterManager;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        characterManager = gameManager.GetComponent<CharacterManager>();
    }

    void OnEnable() {
        // Everytime the inventory is opened update the stat display
        UpdateStatDisplayGM();
    }

    void Start() {
        gameManager.GetComponent<PlayerStats>().RegisterStatChangeListener(UpdateStatDisplay);
    }

    public void UpdateStatDisplayGM() {
        UpdateStatDisplay(gameManager.GetComponent<PlayerStats>(), 0, 0, 0, 0);
    }

    private void UpdateStatDisplay(PlayerStats stats, float maxHealthChange, float dmgChange, float regenChange, float armourChange) {
        // Display the stats 
        armourText.GetComponent<TMP_Text>().SetText(stats.GetArmour().ToString("0.0"));
        regenText.GetComponent<TMP_Text>().SetText(stats.GetRegenPerSecond().ToString("0.0"));
        damageMultiplierText.GetComponent<TMP_Text>().SetText(Mathf.RoundToInt(stats.GetDamageMultiplier() * 100) + " %");

        float maxHealthMult = stats.GetMaxHealthMultiplier();
        string maxHealthTextString = Mathf.RoundToInt(maxHealthMult * characterManager.primary.GetComponent<CharacterStats>().GetOriginalMaxHealth()).ToString("0");

        // Have to take care to make sure the secondary is available
        if(characterManager.IsSecondaryUnlocked()) {
            string secondaryMax = Mathf.RoundToInt(maxHealthMult * characterManager.secondary.GetComponent<CharacterStats>().GetOriginalMaxHealth()).ToString("0");
            maxHealthTextString += " | " + secondaryMax;
        }

        maxHealthText.GetComponent<TMP_Text>().SetText(maxHealthTextString);
    }

}

