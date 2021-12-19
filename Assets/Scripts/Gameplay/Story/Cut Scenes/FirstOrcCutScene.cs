using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstOrcCutScene : CutScene
{
    public override bool IsCutSceneActivatable() {
        return gameManager.GetComponent<ObjectiveManager>().HasObjective("findFirstOrcCave");
    }

    protected override void QueueActions() {
        DialogueManager dialogueManager = gameManager.GetComponent<DialogueManager>();

        QueueAction(() => {
            Debug.Log("Cut scene starting");
            
        }, 10);

        QueueAction(() => {
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("findFirstOrcCave", 0);
            gameManager.GetComponent<ObjectiveManager>().AddObjective("slayFirstOrc", "Slay the Orc", "Kill the Orc and save the cat"
                , new ObjectiveManager.RewardEntry(0, 0));
            Debug.Log("Cut scene over");
        }, 0);
    }

}
