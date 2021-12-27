using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    
    public TMP_Dropdown languageDropdown;
    public LocalisedDropdown localisedDropdownLocaliser;
    public Slider horizontalSlider;
    public Slider verticalSlider;
    
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

        horizontalSlider.value = GlobalSettings.horizontalMouseSensitivity;
        verticalSlider.value = GlobalSettings.verticalMouseSensitivity;
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

    public void OnHorizontalSensChange() {
        GlobalSettings.horizontalMouseSensitivity = horizontalSlider.value;
    }

    public void OnVerticalSensChange() {
        GlobalSettings.verticalMouseSensitivity = verticalSlider.value;
    }

    public void Quit() {
        Application.Quit();
    }

}
