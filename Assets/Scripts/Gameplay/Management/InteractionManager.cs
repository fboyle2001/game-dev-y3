using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
* Handles interactable objects and the effects of interacting with them
**/
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
        // Localise the text
        interactText.GetComponent<TMP_Text>().text = localeManager.GetString("ui_interact_text_start") + " " + localeManager.GetString(textKey);
    }

    // Register interactions so they can be called when the user clicks the interact button

    public void RegisterInteraction(string key, System.Action action) {
        if(registeredInteractions.ContainsKey(key)) return;
        registeredInteractions.Add(key, action);
    }

    public void UnregisterInteraction(string key) {
        if(!registeredInteractions.ContainsKey(key)) return;
        registeredInteractions.Remove(key);
    }

    public void ExecuteInteractions() {
        // Execute any callbacks, take a copy so that IInteractable can alter the list
        // while it is enumerating
        List<System.Action> copiedActions = new List<System.Action>(registeredInteractions.Values);
        
        foreach(System.Action action in copiedActions) {
            action();
        }
    }

    public string StrListInteractions() {
        // Debug function that shows all active interaction callbacks
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
