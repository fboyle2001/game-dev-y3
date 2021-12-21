using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCatInteractable : MonoBehaviour, IInteractable
{

    private ObjectiveManager objectiveManager;
    private InteractionManager interactionManager;

    void OnEnable() {
        objectiveManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<ObjectiveManager>();
        interactionManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<InteractionManager>();
    }

    public void OnInteractPossible() {
        if(objectiveManager.HasObjective("freeCat")) {
            interactionManager.SetText("to free <CAT_NAME>");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("freeCatAction", () => {
                objectiveManager.CompleteObjective("freeCat", 1);
                interactionManager.HideText();
                gameObject.SetActive(false);
            });
        }
    }

    public void OnInteractImpossible() {
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("freeCatAction");
    }

}
