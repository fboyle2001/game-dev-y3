using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class CharacterManager : MonoBehaviour
{

    public GameObject primary;
    public GameObject primaryCamera;
    public GameObject secondary;
    public GameObject secondaryCamera;

    private bool secondaryUnlocked = false;
    private bool primaryActive = true;
    private bool swappingAvailable = true;

    private Dictionary<int, System.Action<GameObject>> activeChangeListeners = new Dictionary<int, System.Action<GameObject>>();

    void OnEnable() {
        // Spawn in, give them their stats
        GlobalSettings.GiveBonusStats(GetComponent<PlayerStats>());
    }

    public void UnlockSecondary() {
        secondaryUnlocked = true;

        secondary.GetComponent<NavMeshObstacle>().enabled = false;
        secondary.GetComponent<NavMeshAgent>().enabled = true;
        secondary.GetComponent<FollowerAgent>().enabled = true;
        secondary.GetComponent<ICharacterActions>().StopMovement();
        secondary.GetComponent<ICharacterActions>().SetFrozen(true);

        GetComponent<UIManager>().DisplaySecondaryPanel(true);
        
        GameObject statDisplay = GameObject.FindGameObjectWithTag("Stat Display");
        if(statDisplay != null && statDisplay.activeSelf) {
            statDisplay.GetComponent<StatDisplayHandler>().UpdateStatDisplayGM();
        }
    }

    public bool IsSecondaryUnlocked() {
        return secondaryUnlocked;
    }

    public bool IsPrimaryActive() {
        return primaryActive;
    }

    public GameObject GetActiveCharacter() {
        return IsPrimaryActive() ? primary : secondary;
    }

    public GameObject GetActiveCamera() {
        return IsPrimaryActive() ? primaryCamera : secondaryCamera;
    }

    public void SetFrozen(bool frozen) {
        if(frozen) {
            if(secondaryUnlocked && !IsPrimaryActive()) {
                secondary.GetComponent<ICharacterActions>().StopMovement();
                secondary.GetComponent<ICharacterActions>().StopLookAround();
                secondary.GetComponent<ICharacterActions>().StopSprinting();
                secondary.GetComponent<ICharacterActions>().StopAttack();
            } else {
                primary.GetComponent<ICharacterActions>().StopMovement();
                primary.GetComponent<ICharacterActions>().StopLookAround();
                primary.GetComponent<ICharacterActions>().StopSprinting();
                primary.GetComponent<ICharacterActions>().StopAttack();
            }
        }

        if(IsPrimaryActive()) {
            primary.GetComponent<ICharacterActions>().SetFrozen(frozen);
        } else {
            secondary.GetComponent<ICharacterActions>().SetFrozen(frozen);
        }
    }

    public void HideSecondary() {
        secondary.SetActive(false);
    }

    public void ShowSecondary() {
        secondary.SetActive(true);
    }

    public void SetSwappingAvailable(bool available) {
        this.swappingAvailable = available;
    }

    public void SwapActive() {
        if(!secondaryUnlocked) return;
        if(!swappingAvailable) return;

        if(primaryActive) {
            primaryCamera.SetActive(false);
            secondaryCamera.SetActive(true);

            primary.GetComponent<NavMeshObstacle>().enabled = false;
            primary.GetComponent<NavMeshAgent>().enabled = true;
            primary.GetComponent<FollowerAgent>().enabled = true;
            primary.GetComponent<Rigidbody>().isKinematic = true;

            primary.GetComponent<ICharacterActions>().SetFrozen(true);
            primary.GetComponent<ICharacterActions>().StopLookAround();
            primary.GetComponent<ICharacterActions>().StopAttack();
            primary.GetComponent<ICharacterActions>().StopSprinting();
            primary.GetComponent<ICharacterActions>().StopMovement();

            secondary.GetComponent<FollowerAgent>().enabled = false;
            secondary.GetComponent<NavMeshAgent>().enabled = false;
            secondary.GetComponent<NavMeshObstacle>().enabled = true;
            secondary.GetComponent<Rigidbody>().isKinematic = false;

            secondary.GetComponent<ICharacterActions>().SetFrozen(false);

            Vector3 eulerAngles = secondary.transform.rotation.eulerAngles;
            secondary.transform.rotation = Quaternion.Euler(new Vector3(0, eulerAngles.y, 0));
            secondaryCamera.transform.rotation = secondary.transform.rotation;
        } else {
            primaryCamera.SetActive(true);
            secondaryCamera.SetActive(false);

            secondary.GetComponent<NavMeshObstacle>().enabled = false;
            secondary.GetComponent<NavMeshAgent>().enabled = true;
            secondary.GetComponent<FollowerAgent>().enabled = true;
            secondary.GetComponent<Rigidbody>().isKinematic = true;

            secondary.GetComponent<ICharacterActions>().SetFrozen(true);
            secondary.GetComponent<ICharacterActions>().StopLookAround();
            secondary.GetComponent<ICharacterActions>().StopAttack();
            secondary.GetComponent<ICharacterActions>().StopSprinting();
            secondary.GetComponent<ICharacterActions>().StopMovement();

            primary.GetComponent<FollowerAgent>().enabled = false;
            primary.GetComponent<NavMeshAgent>().enabled = false;
            primary.GetComponent<NavMeshObstacle>().enabled = true;
            primary.GetComponent<Rigidbody>().isKinematic = false;

            primary.GetComponent<ICharacterActions>().SetFrozen(false);

            Vector3 eulerAngles = primary.transform.rotation.eulerAngles;
            primary.transform.rotation = Quaternion.Euler(new Vector3(0, eulerAngles.y, 0));
            primaryCamera.transform.rotation = primary.transform.rotation;
        }

        primaryActive = !primaryActive;
        PropagateActiveChangeEvent(GetActiveCharacter());
    }

    public void RegisterActiveChangeListener(GameObject owner, System.Action<GameObject> action) {
        if(activeChangeListeners.ContainsKey(owner.GetInstanceID())) {
            System.Action<GameObject> existing;
            activeChangeListeners.TryGetValue(owner.GetInstanceID(), out existing);
            activeChangeListeners.Remove(owner.GetInstanceID());
            activeChangeListeners.Add(owner.GetInstanceID(), action + existing);
        } else {
            activeChangeListeners.Add(owner.GetInstanceID(), action);
        }

        action(GetActiveCharacter());
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
