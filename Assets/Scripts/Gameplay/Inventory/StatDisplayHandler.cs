using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayHandler : MonoBehaviour
{

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
        UpdateStatDisplayGM();
    }

    void Start() {
        gameManager.GetComponent<PlayerStats>().RegisterStatChangeListener(UpdateStatDisplay);
    }

    public void UpdateStatDisplayGM() {
        UpdateStatDisplay(gameManager.GetComponent<PlayerStats>());
    }

    private void UpdateStatDisplay(PlayerStats stats) {
        armourText.GetComponent<TMP_Text>().SetText(stats.GetArmour().ToString("0.0"));
        regenText.GetComponent<TMP_Text>().SetText(stats.GetRegenPerSecond().ToString("0.0"));
        damageMultiplierText.GetComponent<TMP_Text>().SetText(stats.GetDamageMultiplier().ToString("0.00"));

        float maxHealthMult = stats.GetMaxHealthMultiplier();
        string maxHealthTextString = Mathf.RoundToInt(maxHealthMult * characterManager.primary.GetComponent<CharacterStats>().GetOriginalMaxHealth()).ToString("0");

        if(characterManager.IsSecondaryUnlocked()) {
            string secondaryMax = Mathf.RoundToInt(maxHealthMult * characterManager.secondary.GetComponent<CharacterStats>().GetOriginalMaxHealth()).ToString("0");
            maxHealthTextString += " | " + secondaryMax;
        }

        maxHealthText.GetComponent<TMP_Text>().SetText(maxHealthTextString);
    }

}

