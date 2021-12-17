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

    public void OnEnable() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        this.QueueRequiredActions();
        this.QueueActions();
    }

    public void StartCutScene() {
        if(started) return;
        started = true;
        PlayNextAction();
    }

    private void QueueRequiredActions() {
        QueueAction(() => {
            gameManager.GetComponent<DialogueManager>().ClearDialogue();
        }, 0);
    }

    private void PlayNextAction() {
        if(actions.Count == 0) return;

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
