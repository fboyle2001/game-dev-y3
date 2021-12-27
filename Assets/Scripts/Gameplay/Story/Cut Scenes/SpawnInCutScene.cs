using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInCutScene : MonoBehaviour {

    public GameObject realPlayer;
    public GameObject realCamera;

    private GameObject gameManager;
    private float lastVelocityMagnitude = 0;
    private float timeUnder = 0;
    private bool scriptFrozen = false;

    private DaylightManager daylightManager;
    private ObjectiveManager objectiveManager;
    private DialogueManager dialogueManager;
    private CharacterManager characterManager;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        daylightManager = gameManager.GetComponent<DaylightManager>();
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        characterManager = gameManager.GetComponent<CharacterManager>();
    }

    void Start() {
        daylightManager.SetLightIntensity(1);
        realPlayer.SetActive(true);
        realCamera.SetActive(false);
        
        gameManager.GetComponent<CharacterManager>().SetFrozen(true);
        dialogueManager.QueueDialogue("speaker_you", "cs_spawn_falling", 8);
        gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>().ApplyDamageOverTimeWithGoal(1, 17);
    }

    void FixedUpdate() {
        if(scriptFrozen) return;

        if(lastVelocityMagnitude <= 3) {
            timeUnder += Time.fixedDeltaTime;
        } else {
            timeUnder = 0;
        }

        lastVelocityMagnitude = GetComponent<Rigidbody>().velocity.magnitude;

        if(timeUnder > 1.5) {
            // Blacken screen
            realCamera.GetComponent<Camera>().cullingMask = 0;
            realCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            realCamera.GetComponent<Camera>().backgroundColor = Color.black;

            // Darken time
            daylightManager.SetLightIntensity(0);

            // Move the character
            realPlayer.transform.position = new Vector3(gameObject.transform.position.x, 16, gameObject.transform.position.z);
            gameObject.SetActive(false);
            realCamera.SetActive(true);
            gameManager.GetComponent<CharacterManager>().SetFrozen(false);

            // Could add another step to display some text during the blackout phase
            Invoke("FinishCutScene", 1);
            scriptFrozen = true;
        }
    }

    void FinishCutScene() {
        realCamera.GetComponent<Camera>().cullingMask = -1;
        realCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        gameManager.GetComponent<TutorialManager>().QueueTutorial("tut_movement_title", "tut_movement_text", 10);
        gameManager.GetComponent<TutorialManager>().QueueTutorial("tut_display_title", "tut_display_text", 10);
        dialogueManager.QueueDialogue("speaker_you", "cs_spawn_where", 3);
        dialogueManager.QueueDialogue("speaker_you", "cs_spawn_where_cat", 3);
        dialogueManager.QueueDialogue("speaker_you", "cs_spawn_nearby_camp", 5, () => {
            objectiveManager.AddObjective("findInitialCamp", "obj_find_help", new ObjectiveManager.RewardEntry(300, 10));
            objectiveManager.ShowObjectives();
        });
    }

}
