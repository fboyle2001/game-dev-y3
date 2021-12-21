using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCatInteractable : MonoBehaviour, IInteractable
{

    private GameObject gameManager;
    private DialogueManager dialogueManager;
    private ObjectiveManager objectiveManager;
    private InteractionManager interactionManager;
    private TutorialManager tutorialManager;

    private bool registered = false;

    void OnEnable() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        interactionManager = gameManager.GetComponent<InteractionManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        tutorialManager = gameManager.GetComponent<TutorialManager>();
    }

    public void OnInteractPossible() {
        if(objectiveManager.HasObjective("freeCat") && !registered) {
            registered = true;
            interactionManager.SetText("to free <CAT_NAME>");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("freeCatAction", () => {
                gameObject.SetActive(false);
                interactionManager.HideText();
                objectiveManager.CompleteObjective("freeCat", 1);

                dialogueManager.QueueDialogue("You", "You're alive! I can't believe it!", 3);
                dialogueManager.QueueDialogue("You", "We'd better go back to this camp I've found hopefully they'll believe I'm not one of these Orcs now.", 3, () => {
                    objectiveManager.AddObjective("returnToCamp", "Return to the camp", "Return to the camp to prove you aren't an Orc", new ObjectiveManager.RewardEntry(0, 0));
                    tutorialManager.QueueTutorial("Assassin Cat Unlocked", "You've now unlocked your assassin cat. Press [SWAP_KEY] to switch between your player and cat. " + 
                        "Enemies will target your current active character. Both characters must be alive at all times. " + 
                        "Your cat cannot interact but can use the inventory to restore health etc.", 15);
                    gameManager.GetComponent<CharacterManager>().UnlockSecondary();
                });
            });
        }
    }

    public void OnInteractImpossible() {
        registered = false;
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("freeCatAction");
    }

}
