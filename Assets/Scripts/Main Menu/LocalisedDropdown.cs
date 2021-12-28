using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
* Implements dynamic localised UI dropdown
**/
public class LocalisedDropdown : MonoBehaviour {
    
    [System.Serializable]
    // Represents a dropdown entry
    public struct DropdownEntry {

        public string localisationKey;
        public Sprite sprite;
        public bool def;

        public DropdownEntry(string localisationKey, Sprite sprite, bool def) {
            this.localisationKey = localisationKey;
            this.sprite = sprite;
            this.def = def;
        }

    }

    public DropdownEntry[] entries;
    public int overrideDefault = -1;

    private LocaleManager localeManager;
    private TMP_Dropdown dropdown;

    void Awake() {
        GameObject localeOwner = GameObject.FindGameObjectWithTag("Locale");

        if(localeOwner == null) {
            localeOwner = GameObject.FindGameObjectWithTag("Game Manager");
        }

        localeManager = localeOwner.GetComponent<LocaleManager>();
        dropdown = GetComponent<TMP_Dropdown>();
    }

    void OnEnable() {
        FillDropdownOptions();
        // Want to automatically update if the locale changes
        localeManager.SubscribeToLocaleChange(gameObject, OnLocaleChange);
    }

    void OnDisable() {
        localeManager.UnsubscribeFromLocaleChange(gameObject);
    }

    private void OnLocaleChange(LocaleManager locale) {
        FillDropdownOptions();
    }

    private void FillDropdownOptions() {
        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();
        int index = 0;
        int defIndex = 0;

        foreach(DropdownEntry entry in entries) {
            // Localise the dropdown options
            data.Add(new TMP_Dropdown.OptionData(localeManager.GetString(entry.localisationKey), entry.sprite));

            if(entry.def) {
                defIndex = index;
            }

            index++;
        }

        dropdown.AddOptions(data);

        // Set the item that is selected
        dropdown.value = overrideDefault == -1 ? defIndex : overrideDefault;
    }

}
