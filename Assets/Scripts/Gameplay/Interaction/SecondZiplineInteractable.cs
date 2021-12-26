using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondZiplineInteractable : MonoBehaviour, IInteractable {
    
    public GameObject npc;
    public GameObject startZipline;
    public GameObject endZipline;
    public float speed = 8f;

    private GameObject gameManager;
    private InteractionManager interactionManager;
    private ObjectiveManager objectiveManager;
    private DialogueManager dialogueManager;
    private CharacterManager characterManager;

    private bool enableZipline = false;
    private Vector3 target;
    private GameObject primary;
    private float heightOffset;

    private bool used = false;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        interactionManager = gameManager.GetComponent<InteractionManager>();
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        characterManager = gameManager.GetComponent<CharacterManager>();
        primary = characterManager.primary;
        heightOffset = primary.GetComponent<Collider>().bounds.size.y + 0.4f;
    }

    private void EnableZipline() {
        target = endZipline.transform.position - heightOffset * Vector3.up;
        Vector3 travelDirection = (target - transform.position).normalized;
        primary.transform.position = transform.position + travelDirection * 2;// - Vector3.up * heightOffset;

        enableZipline = true;
        primary.GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<AudioSource>().Play();
    }

    private void DisableZipline() {
        enableZipline = false;
        GetComponent<AudioSource>().Stop();
        primary.GetComponent<Rigidbody>().isKinematic = false;
        primary.transform.position = new Vector3(481.66f, 49.92f, 711.97f);
        npc.transform.position = new Vector3(651.77f, 15.04f, 629.11f);
        npc.transform.rotation = Quaternion.Euler(new Vector3(2.964f, -137.6f, -1.56f));
        characterManager.ShowSecondary();
        characterManager.SetSwappingAvailable(true);
    }

    void FixedUpdate() {
        if(!enableZipline) return;

        Vector3 difference = target - primary.transform.position;
        float speedFactor = 1f;

        if(difference.magnitude < 40) {
            speedFactor = Mathf.Max(0.4f, difference.magnitude / 40);

            if(difference.magnitude < 10) {
                speedFactor = 0.2f;
            }
        }

        if(difference.magnitude < 5) {
            DisableZipline();
            return;
        }

        primary.transform.position += difference.normalized * speed * Time.fixedDeltaTime * speedFactor;
    }

    public void OnInteractPossible() {
        if(objectiveManager.HasObjective("returnToCamp2") && !used) {
            interactionManager.SetText("int_use_zipline");
            interactionManager.ShowText();
            interactionManager.RegisterInteraction("crossZiplineB", () => {
                used = true;
                characterManager.SetSwappingAvailable(false);
                dialogueManager.QueueDialogue("speaker_you", "int_zipline_stay_on", 5);
                interactionManager.UnregisterInteraction("crossZiplineB");
                interactionManager.HideText();
                EnableZipline();
            });
        }
    }

    public void OnInteractImpossible() {
        interactionManager.HideText();
        interactionManager.UnregisterInteraction("crossZiplineB");
    }

}
