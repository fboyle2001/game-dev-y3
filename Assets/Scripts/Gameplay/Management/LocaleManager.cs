using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class LocaleManager : MonoBehaviour {
    
    public LocalizedStringTable localizedStringTable;
    private StringTable currentStringTable;

    void Awake() {
        LoadStringTable();
        localizedStringTable.TableChanged += (a) => {
            LoadStringTable();
        };
    }

    private void LoadStringTable() {
        currentStringTable = localizedStringTable.GetTable();
    }

    public string GetString(string key) {
        return currentStringTable[key].GetLocalizedString();
    }

}
