using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeedInteractable : MonoBehaviour, IInteractable
{

    public GameObject finaleSpawnManager;

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
        if(objectiveManager.HasObjective("plantSeeds") && !registered) {
            registered = true;
            interactionManager.SetText("to plant the Seeds of Life");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("plantSeedsAction", () => {
                interactionManager.UnregisterInteraction("plantSeedsAction");
                gameObject.SetActive(false);
                interactionManager.HideText();
                objectiveManager.CompleteObjective("plantSeeds");
                gameManager.GetComponent<MapSectionManager>().EnableSeedParent();

                dialogueManager.QueueDialogue("You", "Woah!", 3);
                dialogueManager.QueueDialogue("You", "I don't think those seeds just summoned a tree...", 3, () => {
                    objectiveManager.AddObjective("finish", "Defeat the Ancient Orc", "Defeat the Ancient Orc", new ObjectiveManager.RewardEntry(7500, 150));
                    finaleSpawnManager.SetActive(true);
                });
            });
        }
    }

    public void OnInteractImpossible() {
        registered = false;
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("plantSeedsAction");
    }

}
