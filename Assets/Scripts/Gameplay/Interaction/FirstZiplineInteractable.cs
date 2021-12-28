using UnityEngine;

/**
* Interact with the zip line to go across to the seeds
* handles the zip line motion while they are on it
* and also spawns the skeleton outlaw for the pause 
**/
public class FirstZiplineInteractable : MonoBehaviour, IInteractable {
    
    public GameObject startZipline;
    public GameObject stopPosition;
    public GameObject endZipline;
    public float speed = 8f;
    public GameObject cutSceneOwner;

    private GameObject gameManager;
    private InteractionManager interactionManager;
    private ObjectiveManager objectiveManager;
    private DialogueManager dialogueManager;
    private CharacterManager characterManager;

    private bool enableZipline = false;
    private bool hasStopped = false;
    private bool postPosFreeze = false;
    private Vector3 target;
    private GameObject primary;
    private float heightOffset;
    private bool triggeredSlowdownMessage = false;
    private bool used = false;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        interactionManager = gameManager.GetComponent<InteractionManager>();
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        characterManager = gameManager.GetComponent<CharacterManager>();

        // Only the primary character will ride the zip line, heightOffset determines gap between zip line
        // and their transform
        primary = characterManager.primary;
        heightOffset = primary.GetComponent<Collider>().bounds.size.y + 0.4f;
    }

    void OnEnable() {
        // Used to check when the skeleton attacking the zip line dies
        EnemyBase.RegisterGlobalDamageHandler(gameObject, OnSkeletonZipLineDeath);
    }

    private void EnableZipline(bool postPosFreeze) {
        // At the mid way point we want to freeze so they can kill the skeleton
        // postPosFreeze handles the target of where we are moving
        if(postPosFreeze) {
            target = endZipline.transform.position;
        } else {
            target = stopPosition.transform.position - heightOffset * Vector3.up;
            // Put them on the zipline
            Vector3 travelDirection = (target - transform.position).normalized;
            primary.transform.position = transform.position + travelDirection * 2;
        }

        this.postPosFreeze = postPosFreeze;
        enableZipline = true;
        // Prevent the physics engine trying to apply physics to us
        primary.GetComponent<Rigidbody>().isKinematic = true;
        // Play the wind sound
        GetComponent<AudioSource>().Play();
    }

    private void OnSkeletonZipLineDeath(EnemyBase enemy) {
        if(!(enemy.identifier == "zipLineSkeleton" && enemy.GetStats().IsDead())) return;
        // Unpause the zip line
        dialogueManager.QueueDialogue("speaker_you", "int_zipline_stuck", 4);
        EnableZipline(true);
    }

    private void DisableZipline() {
        enableZipline = false;
        GetComponent<AudioSource>().Stop();
        // Re-enable physics simulation on the character
        primary.GetComponent<Rigidbody>().isKinematic = false;
        primary.transform.position = new Vector3(196.68f, 78.857f, 747.6f);
        cutSceneOwner.GetComponent<SeedsCutScene>().StartCutScene();
    }

    void FixedUpdate() {
        if(!enableZipline || (hasStopped && !postPosFreeze)) return;

        Vector3 difference = target - primary.transform.position;
        float speedFactor = 1f;

        // Slowdown as we approach target
        if(difference.magnitude < 40) {
            if(!triggeredSlowdownMessage) {
                // If this is before the stop then spawn the skeleton and queue dialogue
                gameManager.GetComponent<MapSectionManager>().EnableZipLineAttack();
                dialogueManager.QueueDialogue("speaker_you", "int_zipline_slow", 4);
                triggeredSlowdownMessage = true;
            }

            // Limit the slowdown effect
            speedFactor = Mathf.Max(0.4f, difference.magnitude / 40);

            if(difference.magnitude < 10) {
                speedFactor = 0.2f;
            }
        }

        // If we are close enough then we disable the zip line instead
        if(difference.magnitude < 5) {
            if(!postPosFreeze) {
                hasStopped = true;
                dialogueManager.QueueDialogue("speaker_you", "int_zipline_skeleton", 5);
                return;
            } else {
                DisableZipline();
                return;
            }
        }

        // Move using translation rather than Rigidbody forces
        primary.transform.position += difference.normalized * speed * Time.fixedDeltaTime * speedFactor;
    }

    public void OnInteractPossible() {
        // Only allowed to use it if they are at the correct part
        // of the story and aren't already on it
        if(objectiveManager.HasObjective("findSeeds") && !used) {
            interactionManager.SetText("int_use_zipline");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("crossZiplineA", () => {
                // When they use the zip line hide the secondary character,
                // prevent swapping to them, and explain it with dialogue
                used = true;
                characterManager.HideSecondary();
                characterManager.SetSwappingAvailable(false);
                interactionManager.HideText();
                // Remove the interaction once they're on it
                interactionManager.UnregisterInteraction("crossZiplineA");
                dialogueManager.QueueDialogue("speaker_you", "int_zipline_hop_on", 5);
                // Start the zip line
                EnableZipline(false);
            });
        }
    }

    public void OnInteractImpossible() {
        // Called when out of range
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("crossZiplineA");
    }

}
