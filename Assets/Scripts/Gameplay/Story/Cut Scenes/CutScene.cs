using System.Collections.Generic;
using UnityEngine;

/**
* Represents a cut scene and gets the necessary scripts for them
**/
public abstract class CutScene : MonoBehaviour {

    struct CutSceneAction {
        public readonly System.Action action;
        public readonly float timeBeforeNext;

        public CutSceneAction(System.Action action, float timeBeforeNext) {
            this.action = action;
            this.timeBeforeNext = timeBeforeNext;
        }
    }

    protected GameObject gameManager;
    private Queue<CutSceneAction> actions = new Queue<CutSceneAction>();
    private bool started = false;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
    }

    public void OnEnable() {
        this.QueueRequiredActions();
        this.QueueActions();
    }

    public void StartCutScene() {
        if(started) return;
        gameManager.GetComponent<UIManager>().SetCrosshairActive(false);
        gameManager.GetComponent<CharacterManager>().primary.GetComponent<AudioSource>().Stop();
        started = true;

        if(!gameManager.GetComponent<CharacterManager>().IsPrimaryActive()) {
            gameManager.GetComponent<CharacterManager>().SwapActive();
        }

        if(gameManager.GetComponent<CharacterManager>().IsSecondaryUnlocked()) {
            gameManager.GetComponent<CharacterManager>().secondary.GetComponent<AudioSource>().Stop();
        }

        PlayNextAction();
    }

    private void QueueRequiredActions() {
        QueueAction(() => {
            gameManager.GetComponent<DialogueManager>().ClearDialogue();
        }, 0);
    }

    private void PlayNextAction() {
        // Cycle through sequentially
        if(actions.Count == 0) {
            // Restore the crosshair if needed, they are always forced back to the player at the start of a cut scene
            gameManager.GetComponent<UIManager>().SetCrosshairActive(gameManager.GetComponent<PlayerInventory>().GetCurrentWeapon() != null);
            return;
        }

        CutSceneAction action = actions.Dequeue();
        action.action();
        
        Invoke("PlayNextAction", action.timeBeforeNext);
    }

    protected void QueueAction(System.Action action, float timeBeforeNext) {
        actions.Enqueue(new CutSceneAction(action, timeBeforeNext));
    }
    
    // Used to check objectives etc.
    public abstract bool IsCutSceneActivatable();

    // Queue actions to be run sequentially in the cut scene
    protected abstract void QueueActions();

}
