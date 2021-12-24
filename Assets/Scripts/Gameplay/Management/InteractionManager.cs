using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour {

    public GameObject interactText;
    private Dictionary<string, System.Action> registeredInteractions = new Dictionary<string, System.Action>();
    private LocaleManager localeManager;

    void Awake() {
        localeManager = GetComponent<LocaleManager>();
    }

    public void HideText() {
        interactText.SetActive(false);
    }

    public void ShowText() {
        interactText.SetActive(true);
    }

    public void SetText(string textKey) {
        interactText.GetComponent<TMP_Text>().text = localeManager.GetString("ui_interact_text_start") + " " + localeManager.GetString(textKey);
    }

    public void RegisterInteraction(string key, System.Action action) {
        if(registeredInteractions.ContainsKey(key)) return;
        registeredInteractions.Add(key, action);
    }

    public void UnregisterInteraction(string key) {
        if(!registeredInteractions.ContainsKey(key)) return;
        registeredInteractions.Remove(key);
    }

    public void ExecuteInteractions() {
        List<System.Action> copiedActions = new List<System.Action>(registeredInteractions.Values);
        
        foreach(System.Action action in copiedActions) {
            action();
        }
    }

    public string StrListInteractions() {
        string conc = "";

        foreach(string key in new List<string>(registeredInteractions.Keys)) {
            if(conc == "") {
                conc += ", ";
            }

            conc += key;
        }

        return conc;
    }

}
