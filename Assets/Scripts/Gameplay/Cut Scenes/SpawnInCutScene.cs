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

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        gameManager.GetComponent<DaylightManager>().SetLightIntensity(1);
        
        // TODO: Localise
        gameManager.GetComponent<DialogueManager>().QueueDialogue("You", "AHHHHHHHH!!!", 5);
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
            gameManager.GetComponent<DaylightManager>().SetLightIntensity(0);

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
        gameManager.GetComponent<DialogueManager>().QueueDialogue("You", "Where am I?", 3);
        gameManager.GetComponent<DialogueManager>().QueueDialogue("You", "Where is <CAT NAME>?", 3);
        gameManager.GetComponent<DialogueManager>().QueueDialogue("You", "Is that smoke over there? Maybe someone is nearby...", 5, () => {
            gameManager.GetComponent<ObjectiveManager>().AddObjective("findInitialCamp", "Follow the smoke", "Someone might be nearby", new ObjectiveManager.RewardEntry(0, 0));
            gameManager.GetComponent<ObjectiveManager>().ShowObjectives();
        });
    }

}
