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

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        daylightManager = gameManager.GetComponent<DaylightManager>();
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        characterManager = gameManager.GetComponent<CharacterManager>();

        daylightManager.SetLightIntensity(1);
        characterManager.DisplaySecondaryPanel(false);
        realPlayer.SetActive(true);
        realCamera.SetActive(false);
        
        // TODO: Localise
        gameManager.GetComponent<CharacterManager>().SetFrozen(true);
        dialogueManager.QueueDialogue("You", "AHHHHHHHH!!!", 8);
        gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>().ApplyDamageOverTime(99, 16);
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
            realPlayer.transform.position = new Vector3(gameObject.transform.position.x, 17, gameObject.transform.position.z);
            gameObject.SetActive(false);
            realCamera.SetActive(true);

            // Could add another step to display some text during the blackout phase
            Invoke("FinishCutScene", 1);
            scriptFrozen = true;
        }
    }

    void FinishCutScene() {
        realCamera.GetComponent<Camera>().cullingMask = -1;
        realCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        gameManager.GetComponent<CharacterManager>().SetFrozen(false);
        gameManager.GetComponent<TutorialManager>().QueueTutorial("Movement and Display",
            "To move your character around use WASD and your mouse to look around. In the top left corner you will see your health." +
            " Right now you are on the verge of death! Objectives appear in the top right corner in case you forget what you are aiming for." + 
            " Speech appears in the bottom middle of the screen when interacting with others.", 15);
        dialogueManager.QueueDialogue("You", "Where am I?", 3);
        dialogueManager.QueueDialogue("You", "Where is <CAT NAME>?", 3);
        dialogueManager.QueueDialogue("You", "Something smells like it's burning. Maybe there is a camp nearby?", 5, () => {
            objectiveManager.AddObjective("findInitialCamp", "Find help", "Someone might be nearby", new ObjectiveManager.RewardEntry(0, 0));
            objectiveManager.ShowObjectives();
        });
    }

}
