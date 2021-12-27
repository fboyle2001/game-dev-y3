using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;

public class LocaleManager : MonoBehaviour {
    
    private Dictionary<int, System.Action<LocaleManager>> callbacks = new Dictionary<int, System.Action<LocaleManager>>();

    public LocalizedStringTable localizedStringTable;
    private StringTable currentStringTable;

    public void SubscribeToLocaleChange(GameObject owner, System.Action<LocaleManager> action) {
        if(callbacks.ContainsKey(owner.GetInstanceID())) return;
        callbacks.Add(owner.GetInstanceID(), action);
    }

    public void UnsubscribeFromLocaleChange(GameObject owner) {
        if(!callbacks.ContainsKey(owner.GetInstanceID())) return;
        callbacks.Remove(owner.GetInstanceID());
    }

    void Awake() {
        ChangeLocale("en-GB");
        LoadStringTable();
        localizedStringTable.TableChanged += (a) => {
            LoadStringTable();
        };
    }

    public void ChangeLocale(string ietfCode) {
        Locale locale = null;

        foreach(Locale l in LocalizationSettings.AvailableLocales.Locales) {
            if(l.Identifier.Code == ietfCode) {
                locale = l;
                break;
            }
        }
        
        LocalizationSettings.SelectedLocale = locale;
    }

    private void LoadStringTable() {
        currentStringTable = localizedStringTable.GetTable();
        List<System.Action<LocaleManager>> callbackActions = new List<System.Action<LocaleManager>>(callbacks.Values);
        callbackActions.ForEach(action => action(this));
    }

    public string GetString(string key) {
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

