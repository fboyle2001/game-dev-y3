using UnityEngine;

/**
* Displays text when walking through a specific trigger with specific objectives
**/
public class MessageTrigger : MonoBehaviour {

    public string speakerKey;
    public string messageKey;
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
    private ITriggerAction additionalTriggerAction = null;

    void Awake() {
        dialogueManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<DialogueManager>();
        objectiveManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<ObjectiveManager>();
        additionalTriggerAction = GetComponent<ITriggerAction>();
    }

    void FixedUpdate() {
        // Limited number of times it can be triggered
        if(canTrigger || triggeredCount >= maxTriggerCount) return;
        timeSinceLastTrigger += Time.fixedDeltaTime;

        // Don't show everytime we go through use a timeout delay
        if(timeSinceLastTrigger > delayBetweenTriggers) {
            canTrigger = true;
        }
    }

    void OnTriggerEnter(Collider collider) {
        if(!canTrigger) return;

        // Trigger if possible and meets conditions
        if(collider.gameObject.tag == triggerTag && objectiveManager.HasAtLeastOneObjective(activeOnObjectiveIds)) {
            canTrigger = false;
            triggeredCount += 1;
            timeSinceLastTrigger = 0;

            dialogueManager.QueueDialogue(speakerKey, messageKey, messageDisplayTime);

            if(additionalTriggerAction != null) {
                additionalTriggerAction.PerformAction();
            }
        }
    }

}
