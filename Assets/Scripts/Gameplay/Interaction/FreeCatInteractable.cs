using UnityEngine;

/**
* This interaction happens in the Orc Cave and allows the primary
* to free their cat and unlock the secondary character
**/
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
        // Can only be called if thet have the right objective (i.e. in the right spot in the story)
        if(objectiveManager.HasObjective("freeCat") && !registered) {
            registered = true;
            interactionManager.SetText("int_free_cat");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("freeCatAction", () => {
                // Prevent double unlocking
                interactionManager.UnregisterInteraction("freeCatAction");

                // Unlock the ability to swap and heal the cat as they will have
                // received some max health increases by now
                gameManager.GetComponent<CharacterManager>().UnlockSecondary();
                gameManager.GetComponent<CharacterManager>().secondary.GetComponent<CharacterStats>().HealAsPercent(1f);
                // Explain how to use the cat
                tutorialManager.QueueTutorial("tut_cat_title", "tut_cat_text", 15);
                interactionManager.HideText();

                // Complete the objective and give them their rewards
                objectiveManager.CompleteObjective("freeCat");

                // Schedule some dialogue
                dialogueManager.QueueDialogue("speaker_you", "txt_freed_cat_1", 3);
                dialogueManager.QueueDialogue("speaker_you", "txt_freed_cat_2", 7, () => {
                    objectiveManager.AddObjective("returnToCamp", "obj_return_to_camp", new ObjectiveManager.RewardEntry(1000, 20));
                });
                
                // Hide the cage this is attached to
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
