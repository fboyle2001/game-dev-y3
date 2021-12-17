using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCampCutScene : CutScene {

    public override bool IsCutSceneActivatable() {
        return gameManager.GetComponent<ObjectiveManager>().HasObjective("findInitialCamp");
    }

    protected override void QueueActions() {
        DialogueManager dialogueManager = gameManager.GetComponent<DialogueManager>();

        QueueAction(() => {
            Debug.Log("Cut scene starting");
            dialogueManager.QueueDialogue("You", "Hello? Is anybody there?", 3);
            dialogueManager.QueueDialogue("NPC1", "Stay out of here! The others are on their way back you'd better get running... wait who are you?", 3);
            dialogueManager.QueueDialogue("You", "I'm <CHARACTER NAME> I don't know how I got here, where am I?", 3);
            dialogueManager.QueueDialogue("NPC1", "Hmmm I'm not sure I believe that, we're the furtherest camp from <THE TOWN>." + 
                "How did you end up so far lost? Did the Orcs put you up to this?", 3);
            dialogueManager.QueueDialogue("You", "The Orcs? Last I remember was falling down that mountain over there <FOR WHAT REASON>.", 3);
            dialogueManager.QueueDialogue("You", "NPC1 you haven't seen a small cat running about anywhere have you? I swear they were with me...", 3);
            dialogueManager.QueueDialogue("NPC1", "Wait you're part of the Assassin Cats group? You must have taken some fall to not know where you are...", 3);
            dialogueManager.QueueDialogue("You", "What do you mean?", 3);
            dialogueManager.QueueDialogue("NPC1", "I think I might know where your cat is but I don't know whether to trust you..." + 
                "there is an Orc's cave just off the path you followed to get here, might be worth a look.", 3);
            dialogueManager.QueueDialogue("NPC1", "Take out the Orc and then come back here and I'll explain everything you want to know." + 
                "Here's a weapon to help you out, don't do anything stupid...", 3);
        }, 30);

        QueueAction(() => {
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("findInitialCamp", 0);
            gameManager.GetComponent<ObjectiveManager>().AddObjective("findFirstOrcCave", "Find an Orc Cave", "Find an Orc Cave and see if they have your cat"
                , new ObjectiveManager.RewardEntry(0, 0));
            Debug.Log("Cut scene over");
        }, 0);
    }

}
