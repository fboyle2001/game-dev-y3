using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionManager : MonoBehaviour {
    
    private DialogueManager dialogueManager;
    private ObjectiveManager objectiveManager;

    void OnEnable() {
        dialogueManager = GetComponent<DialogueManager>();
        objectiveManager = GetComponent<ObjectiveManager>();
        EnemyBase.RegisterGlobalDamageHandler(gameObject, OnDamageHandler);
    }

    private void OnDamageHandler(EnemyBase enemy) {
        if(!enemy.GetStats().IsDead()) return;

        switch(enemy.identifier) {
            case "firstOrc":
                objectiveManager.CompleteObjective("findFirstOrcCave", 1);
                objectiveManager.AddObjective("freeCat", "Free your cat", "Free you cat from the cage inside the cave", new ObjectiveManager.RewardEntry(0, 0));
                dialogueManager.QueueDialogue("You", "Lets go free <CAT_NAME>! I can hear them just inside the cave.", 5);
                return;
            default:
                return;
        }
    }

}
