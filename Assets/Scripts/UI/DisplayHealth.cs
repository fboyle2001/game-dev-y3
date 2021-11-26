using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHealth : MonoBehaviour
{

    public GameObject characterManager;
    public bool asPercentage;
    public bool withPrefix;
    public string displayType = "active";
    private TMP_Text healthText;
    private ActiveCharacterManager activeCharacterManager;

    void Start() {
        healthText = GetComponent<TMP_Text>();
        activeCharacterManager = characterManager.GetComponent<ActiveCharacterManager>();
    }

    void Update() {
        CharacterStatManager manager;

        if(displayType == "active") {
            manager = activeCharacterManager.GetActiveCharacter().GetComponent<CharacterStatManager>();
        } else if (displayType == "secondary") {
            manager = activeCharacterManager.secondary.GetComponent<CharacterStatManager>();
        } else {
            manager = activeCharacterManager.primary.GetComponent<CharacterStatManager>();
        }
         
        if(asPercentage) {
            float percent = Mathf.Round(manager.GetCurrentHealth() * 100 / manager.maxHealth);

            if(withPrefix) {
                healthText.text = "Health: " + percent + "%";
            } else {
                healthText.text = percent + "%";
            }
        } else {
            if(withPrefix) {
                healthText.text = "Health: " + manager.GetCurrentHealth() + " / " + manager.maxHealth;
            } else {
                healthText.text = manager.GetCurrentHealth() + " / " + manager.maxHealth;
            }
        }
    }
}
