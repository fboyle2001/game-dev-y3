using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
* UI actions for the settings screen
**/
public class SettingsScreen : MonoBehaviour {

    public GameObject mainPanel;
    public TMP_Dropdown languageDropdown;
    public LocalisedDropdown localisedDropdownLocaliser;

    public TMP_Text horizontalValue;
    public Button horizontalDecrease;
    public Button horizontalIncrease;
    public TMP_Text verticalValue;
    public Button verticalDecrease;
    public Button verticalIncrease;
    
    private LocaleManager localeManager;

    private int horzChanges = 0;
    private int vertChanges = 0;

    void Awake() {
        localeManager = GameObject.FindGameObjectWithTag("Locale").GetComponent<LocaleManager>();
    }

    void OnEnable() {
        // Get the current selected language
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

        // Display current sensitivity
        horizontalValue.text = GlobalSettings.horizontalMouseSensitivity.ToString();
        verticalValue.text = GlobalSettings.verticalMouseSensitivity.ToString();
    }

    public void Back() {
        mainPanel.SetActive(true);
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
        // Handles a weird issue where the UI wants to double click on the main menu
        if(horzChanges % 2 != 0) {
            horzChanges++;
            return;
        }

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

        horzChanges++;
    }

    private void ChangeVerticalSens(int amount) {
        if(vertChanges % 2 != 0) {
            vertChanges++;
            return;
        }

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

        vertChanges++;
    }

}
