using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeedInteractable : MonoBehaviour, IInteractable
{

    public GameObject finaleSpawnManager;
    public AudioClip seedsPlantedClip;

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
            interactionManager.SetText("int_plant_seeds");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("plantSeedsAction", () => {
                interactionManager.UnregisterInteraction("plantSeedsAction");
                AudioSource.PlayClipAtPoint(seedsPlantedClip, transform.position);
                gameObject.SetActive(false);
                interactionManager.HideText();
                objectiveManager.CompleteObjective("plantSeeds");
                gameManager.GetComponent<MapSectionManager>().EnableSeedParent();

                dialogueManager.QueueDialogue("speaker_you", "int_seeds_initial", 3);
                dialogueManager.QueueDialogue("speaker_you", "int_seeds_second", 5, () => {
                    objectiveManager.AddObjective("finish", "obj_defeat_ancient_orc", new ObjectiveManager.RewardEntry(7500, 150));
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
