using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    
    public TMP_Dropdown languageDropdown;
    public LocalisedDropdown localisedDropdownLocaliser;
    
    public TMP_Text horizontalValue;
    public Button horizontalDecrease;
    public Button horizontalIncrease;
    public TMP_Text verticalValue;
    public Button verticalDecrease;
    public Button verticalIncrease;
    
    private LocaleManager localeManager;

    void Awake() {
        localeManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<LocaleManager>();
    }

    void OnEnable() {
        Time.timeScale = 0;
        int languageValue = 0;
        string code = localeManager.GetCurrentTable().LocaleIdentifier.CultureInfo.IetfLanguageTag;

        switch(code) {
            case "en-GB":
                languageValue = 0;
                break;
            case "en-US":
                languageValue = 1;
                break;
            case "fr":
                languageValue = 2;
                break;
        }

        languageDropdown.value = languageValue;

        horizontalValue.text = GlobalSettings.horizontalMouseSensitivity.ToString();
        verticalValue.text = GlobalSettings.verticalMouseSensitivity.ToString();
    }

    void OnDisable() {
        Time.timeScale = 1;
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void OnLanguageChange(int value) {
        switch(value) {
            case 0:
                localeManager.ChangeLocale("en-GB");
                break;
            case 1:
                localeManager.ChangeLocale("en-US");
                break;
            case 2:
                localeManager.ChangeLocale("fr");
                break;
            default:
                return;
        }

        localisedDropdownLocaliser.overrideDefault = value;
    }

    public void Quit() {
        Application.Quit();
    }

    public void IncreaseHS() {
        ChangeHorizontalSens(1);
    }

    public void DecreaseHS() {
        ChangeHorizontalSens(-1);
    }

    public void IncreaseVS() {
        ChangeVerticalSens(1);
    }

    public void DecreaseVS() {
        ChangeVerticalSens(-1);
    }

    private void ChangeHorizontalSens(int amount) {
        int newVal = (int) Mathf.Clamp(GlobalSettings.horizontalMouseSensitivity + amount, 1, 10);
        GlobalSettings.horizontalMouseSensitivity = newVal;
        horizontalValue.text = newVal.ToString();

        if(newVal <= 1) {
            horizontalIncrease.interactable = true;
            horizontalDecrease.interactable = false;
        } else if (newVal >= 10) {
            horizontalIncrease.interactable = false;
            horizontalDecrease.interactable = true;
        } else {
            horizontalIncrease.interactable = true;
            horizontalDecrease.interactable = true;
        }
    }

    private void ChangeVerticalSens(int amount) {
        int newVal = (int) Mathf.Clamp(GlobalSettings.verticalMouseSensitivity + amount, 1, 10);
        GlobalSettings.verticalMouseSensitivity = newVal;
        verticalValue.text = newVal.ToString();

        if(newVal <= 1) {
            verticalIncrease.interactable = true;
            verticalDecrease.interactable = false;
        } else if (newVal >= 10) {
            verticalIncrease.interactable = false;
            verticalDecrease.interactable = true;
        } else {
            verticalIncrease.interactable = true;
            verticalDecrease.interactable = true;
        }
    }

}
