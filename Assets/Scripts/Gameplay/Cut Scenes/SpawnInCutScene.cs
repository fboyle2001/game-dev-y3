using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInCutScene : MonoBehaviour {

    public GameObject realPlayer;
    public Camera realCamera;

    private GameObject gameManager;
    private float lastVelocityMagnitude = 0;
    private float timeUnder = 0;
    private bool scriptFrozen = false;

    private DaylightManager daylightManager;
    private ObjectiveManager objectiveManager;
    private DialogueManager dialogueManager;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        daylightManager = gameManager.GetComponent<DaylightManager>();
        objectiveManager = gameManager.GetComponent<ObjectiveManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();

        daylightManager.SetLightIntensity(1);
        
        // TODO: Localise
        dialogueManager.QueueDialogue("You", "AHHHHHHHH!!!", 8);
    }

    void Update() {
        if(scriptFrozen) return;

        if(lastVelocityMagnitude <= 3) {
            timeUnder += Time.deltaTime;
        } else {
            timeUnder = 0;
        }

        lastVelocityMagnitude = GetComponent<Rigidbody>().velocity.magnitude;

        if(timeUnder > 1.5) {
            // Blacken screen
            realCamera.cullingMask = 0;
            realCamera.clearFlags = CameraClearFlags.SolidColor;
            realCamera.backgroundColor = Color.black;

            // Darken time
            daylightManager.SetLightIntensity(0);

            // Move the character
            realPlayer.transform.position = new Vector3(gameObject.transform.position.x, 17, gameObject.transform.position.z);
            gameObject.SetActive(false);
            realPlayer.SetActive(true);

            // Could add another step to display some text during the blackout phase
            Invoke("FinishCutScene", 1);
            scriptFrozen = true;
        }
    }

    void FinishCutScene() {
        realCamera.cullingMask = -1;
        realCamera.clearFlags = CameraClearFlags.Skybox;
        dialogueManager.QueueDialogue("You", "Where am I?", 3);
        dialogueManager.QueueDialogue("You", "Where is <CAT NAME>?", 3);
        dialogueManager.QueueDialogue("You", "Something smells like it's burning. Maybe there is a camp nearby?", 5, () => {
            objectiveManager.AddObjective("findInitialCamp", "Find help", "Someone might be nearby", new ObjectiveManager.RewardEntry(0, 0));
            objectiveManager.ShowObjectives();
        });
    }

}
