using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{

    public string speaker;
    public string message;
    public int messageDisplayTime = 5;
    public string[] activeOnObjectiveIds = null;
    public string triggerTag = "Primary Character";
    public int maxTriggerCount = 1;
    public float delayBetweenTriggers = 5;

    private DialogueManager dialogueManager;
    private ObjectiveManager objectiveManager;
    private int triggeredCount = 0;
    private bool canTrigger = true;
    private float timeSinceLastTrigger = 0;

    void OnEnable() {
        dialogueManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<DialogueManager>();
        objectiveManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<ObjectiveManager>();
    }

    void FixedUpdate() {
        if(canTrigger || triggeredCount >= maxTriggerCount) return;
        timeSinceLastTrigger += Time.fixedDeltaTime;

        if(timeSinceLastTrigger > delayBetweenTriggers) {
            canTrigger = true;
        }
    }

    void OnTriggerEnter(Collider collider) {
        if(!canTrigger) return;

        if(collider.gameObject.tag == triggerTag && objectiveManager.HasAtLeastOneObjective(activeOnObjectiveIds)) {
            canTrigger = false;
            triggeredCount += 1;
            timeSinceLastTrigger = 0;

            dialogueManager.QueueDialogue(speaker, message, messageDisplayTime);
        }
    }

}
