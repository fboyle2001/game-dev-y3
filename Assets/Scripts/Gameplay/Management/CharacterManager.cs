using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{

    public GameObject primary;
    public GameObject primaryCamera;
    public GameObject secondary;
    public GameObject secondaryCamera;

    public GameObject primaryImage;
    public GameObject primaryHealth;

    public GameObject secondaryPanel;
    public GameObject secondaryImage;
    public GameObject secondaryHealth;

    private bool secondaryUnlocked = false;
    private bool primaryActive = true;

    private Dictionary<int, System.Action<GameObject>> activeChangeListeners = new Dictionary<int, System.Action<GameObject>>();

    void OnEnable() {
        // Automatically updates the UI with the health of each character
        primary.GetComponent<CharacterStats>().RegisterHealthUpdateListener(new System.Action<CharacterStats, float>((stats, change) => {
            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            primaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        }));

        secondary.GetComponent<CharacterStats>().RegisterHealthUpdateListener(new System.Action<CharacterStats, float>((stats, change) => {
            int percentage = Mathf.RoundToInt(100 * Mathf.Clamp01(stats.GetCurrentHealth() / stats.GetMaxHealth()));
            primaryHealth.GetComponent<TMP_Text>().SetText(percentage + "%");
        }));
    }

    public void DisplaySecondaryPanel(bool display) {
        secondaryPanel.SetActive(display && secondaryUnlocked);
    }

    public void SetSecondaryUnlocked(bool unlocked) {
        secondaryUnlocked = unlocked;
    }

    public bool IsPrimaryActive() {
        return primaryActive;
    }

    public GameObject GetActiveCharacter() {
        // TODO: Implement
        return primary;
    }

    public GameObject GetActiveCamera() {
        // TODO: Implement
        return primaryCamera;
    }

    public void SetFrozen(bool frozen) {
        ICharacterActions actions = primary.GetComponent<ICharacterActions>();

        if(frozen) {
            actions.StopMovement();
            actions.StopLookAround();
            actions.StopSprinting();
            actions.StopAttack();
        }

        actions.SetFrozen(frozen);
    }

    public void RegisterActiveChangeListener(GameObject owner, System.Action<GameObject> action) {
        activeChangeListeners.Add(owner.GetInstanceID(), action);
        action(IsPrimaryActive() ? primary : secondary);
    }

    public void DeregisterActiveChangeListener(GameObject owner) {
        activeChangeListeners.Remove(owner.GetInstanceID());
    }

    private void PropagateActiveChangeEvent(GameObject newActive) {
        foreach(System.Action<GameObject> action in activeChangeListeners.Values) {
            action(newActive);
        }
    }

}
