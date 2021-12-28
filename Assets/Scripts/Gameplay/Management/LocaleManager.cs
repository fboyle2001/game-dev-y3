using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

/**
* Handles all dynamic localisation
* (static localisation such as menu options is handled by Localize String Event)
**/
public class LocaleManager : MonoBehaviour {
    
    private Dictionary<int, System.Action<LocaleManager>> callbacks = new Dictionary<int, System.Action<LocaleManager>>();

    public LocalizedStringTable localizedStringTable;
    private StringTable currentStringTable;

    // Use to propagate a locale change
    public void SubscribeToLocaleChange(GameObject owner, System.Action<LocaleManager> action) {
        if(callbacks.ContainsKey(owner.GetInstanceID())) return;
        callbacks.Add(owner.GetInstanceID(), action);
    }

    public void UnsubscribeFromLocaleChange(GameObject owner) {
        if(!callbacks.ContainsKey(owner.GetInstanceID())) return;
        callbacks.Remove(owner.GetInstanceID());
    }

    void Awake() {
        LoadStringTable();
        localizedStringTable.TableChanged += (a) => {
            LoadStringTable();
        };
    }

    public void UpdateGlobalVariable(string key, string value) {
        // Updates the global string database allowing localisation smart formatting to be dynamic
        PersistentVariablesSource source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        (source["global"][key] as StringVariable).Value = value;
    }

    public void ChangeLocale(string ietfCode) {
        // Switches to the selected locale if it is available
        Locale locale = null;

        foreach(Locale l in LocalizationSettings.AvailableLocales.Locales) {
            if(l.Identifier.Code == ietfCode) {
                locale = l;
                break;
            }
        }
        
        if(locale != null) {
            LocalizationSettings.SelectedLocale = locale;
        }
    }

    private void LoadStringTable() {
        // Loads the localised string table
        currentStringTable = localizedStringTable.GetTable();
        List<System.Action<LocaleManager>> callbackActions = new List<System.Action<LocaleManager>>(callbacks.Values);
        callbackActions.ForEach(action => action(this));
    }

    public string GetString(string key) {
        // Gets the localised version of a string
        if(currentStringTable == null) {
            LoadStringTable();
        }

        return currentStringTable[key].GetLocalizedString();
    }

    public StringTable GetCurrentTable() {
        if(currentStringTable == null) {
            LoadStringTable();
        }

        return currentStringTable;
    }

}

