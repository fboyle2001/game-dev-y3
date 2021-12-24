using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        interactionManager = gameManager.GetComponent<InteractionManager>();
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        characterManager = gameManager.GetComponent<CharacterManager>();
        primary = characterManager.primary;
        heightOffset = primary.GetComponent<Collider>().bounds.size.y + 0.4f;
    }

    void OnEnable() {
        EnemyBase.RegisterGlobalDamageHandler(gameObject, OnSkeletonZipLineDeath);
    }

    private void EnableZipline(bool postPosFreeze) {
        if(postPosFreeze) {
            target = endZipline.transform.position;
        } else {
            target = stopPosition.transform.position - heightOffset * Vector3.up;
            Vector3 travelDirection = (target - transform.position).normalized;
            primary.transform.position = transform.position + travelDirection * 2;// - Vector3.up * heightOffset;
        }

        this.postPosFreeze = postPosFreeze;
        enableZipline = true;
        primary.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnSkeletonZipLineDeath(EnemyBase enemy) {
        if(!(enemy.identifier == "zipLineSkeleton" && enemy.GetStats().IsDead())) return;
        dialogueManager.QueueDialogue("You", "Ah there we go we're moving again, phew!", 4);
        EnableZipline(true);
    }

    private void DisableZipline() {
        enableZipline = false;
        primary.GetComponent<Rigidbody>().isKinematic = false;
        primary.transform.position = new Vector3(196.68f, 78.857f, 747.6f);
        cutSceneOwner.GetComponent<SeedsCutScene>().StartCutScene();
    }

    void FixedUpdate() {
        if(!enableZipline || (hasStopped && !postPosFreeze)) return;

        Vector3 difference = target - primary.transform.position;
        float speedFactor = 1f;

        if(difference.magnitude < 40) {
            if(!triggeredSlowdownMessage) {
                gameManager.GetComponent<MapSectionManager>().EnableZipLineAttack();
                dialogueManager.QueueDialogue("You", "Errrr... why are we slowing down...", 3);
                triggeredSlowdownMessage = true;
            }

            speedFactor = Mathf.Max(0.4f, difference.magnitude / 40);

            if(difference.magnitude < 10) {
                speedFactor = 0.2f;
            }
        }

        if(difference.magnitude < 5) {
            if(!postPosFreeze) {
                hasStopped = true;
                dialogueManager.QueueDialogue("You", "Ah the skeleton is going for the zip line! Better take it out!", 5);
                return;
            } else {
                DisableZipline();
                return;
            }
        }

        primary.transform.position += difference.normalized * speed * Time.fixedDeltaTime * speedFactor;
    }

    public void OnInteractPossible() {
        if(objectiveManager.HasObjective("findSeeds")) {
            interactionManager.SetText("to use zip line");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("crossZiplineA", () => {
                characterManager.HideSecondary();
                characterManager.SetSwappingAvailable(false);
                interactionManager.HideText();
                interactionManager.UnregisterInteraction("crossZiplineA");
                dialogueManager.QueueDialogue("You", "Hop on <CAT_NAME> we are going across!", 5);
                EnableZipline(false);
            });
        }
    }

    public void OnInteractImpossible() {
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("crossZiplineA");
    }

}
