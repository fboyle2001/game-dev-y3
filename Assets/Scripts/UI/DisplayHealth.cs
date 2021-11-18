using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHealth : MonoBehaviour
{

    public GameObject gameManager;
    public bool asPercentage;
    public bool withPrefix;
    public string displayType = "active";
    private TMP_Text healthText;

    void Start() {
        healthText = GetComponent<TMP_Text>();
    }

    void Update() {
        HealthManager manager;

        if(displayType == "active") {
            manager = gameManager.GetComponent<SwitchPlayer>().GetActiveCharacter().GetComponent<HealthManager>();
        } else if (displayType == "secondary") {
            manager = gameManager.GetComponent<SwitchPlayer>().secondary.GetComponent<HealthManager>();
        } else {
            manager = gameManager.GetComponent<SwitchPlayer>().player.GetComponent<HealthManager>();
        }
         
        if(asPercentage) {
            float percent = Mathf.Round(manager.health * 100 / manager.maxHealth);

            if(withPrefix) {
                healthText.text = "Health: " + percent + "%";
            } else {
                healthText.text = percent + "%";
            }
        } else {
            if(withPrefix) {
                healthText.text = "Health: " + manager.health + " / " + manager.maxHealth;
            } else {
                healthText.text = manager.health + " / " + manager.maxHealth;
            }
        }
    }
}
