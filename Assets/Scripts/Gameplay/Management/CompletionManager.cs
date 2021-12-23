using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionManager : MonoBehaviour {
    
    private DialogueManager dialogueManager;
    private ObjectiveManager objectiveManager;

    void Awake() {
        dialogueManager = GetComponent<DialogueManager>();
        objectiveManager = GetComponent<ObjectiveManager>();
    }

    void Start() {
        EnemyBase.RegisterGlobalDamageHandler(gameObject, OnDamageHandler);
    }

    private void OnDamageHandler(EnemyBase enemy) {
        if(!enemy.GetStats().IsDead()) return;

        switch(enemy.identifier) {
            case "firstOrc":
                objectiveManager.CompleteObjective("findFirstOrcCave");
                objectiveManager.AddObjective("freeCat", "Free your cat", "Free you cat from the cage inside the cave", new ObjectiveManager.RewardEntry(1000, 20));
                dialogueManager.QueueDialogue("You", "Lets go free <CAT_NAME>! I can hear them just inside the cave.", 5);
                return;
            default:
                return;
        }
    }

}
