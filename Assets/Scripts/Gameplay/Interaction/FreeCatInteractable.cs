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

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        interactionManager = gameManager.GetComponent<InteractionManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        tutorialManager = gameManager.GetComponent<TutorialManager>();
    }

    public void OnInteractPossible() {
        if(objectiveManager.HasObjective("freeCat") && !registered) {
            registered = true;
            interactionManager.SetText("int_free_cat");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("freeCatAction", () => {
                interactionManager.UnregisterInteraction("freeCatAction");
                gameManager.GetComponent<CharacterManager>().UnlockSecondary();
                gameManager.GetComponent<CharacterManager>().secondary.GetComponent<CharacterStats>().HealAsPercent(1f);
                tutorialManager.QueueTutorial("tut_cat_title", "tut_cat_text", 15);
                interactionManager.HideText();
                objectiveManager.CompleteObjective("freeCat");

                dialogueManager.QueueDialogue("speaker_you", "txt_freed_cat_1", 3);
                dialogueManager.QueueDialogue("speaker_you", "txt_freed_cat_2", 7, () => {
                    objectiveManager.AddObjective("returnToCamp", "obj_return_to_camp", new ObjectiveManager.RewardEntry(1000, 20));
                });
                
                gameObject.SetActive(false);
            });
        }
    }

    public void OnInteractImpossible() {
        registered = false;
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("freeCatAction");
    }

}
