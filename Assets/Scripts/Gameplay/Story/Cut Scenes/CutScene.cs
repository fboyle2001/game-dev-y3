using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(actions.Count == 0) {
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
    
    public abstract bool IsCutSceneActivatable();
    protected abstract void QueueActions();

}
