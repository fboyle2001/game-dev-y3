using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInCutScene : MonoBehaviour {

    public GameObject realPlayer;
    public Camera realCamera;

    private float lastVelocityMagnitude = 0;
    private float timeUnderOne = 0;
    private bool scriptFrozen = false;

    void Update() {
        if(scriptFrozen) return;

        if(lastVelocityMagnitude <= 1) {
            timeUnderOne += Time.deltaTime;
        } else {
            timeUnderOne = 0;
        }

        lastVelocityMagnitude = GetComponent<Rigidbody>().velocity.magnitude;

        if(timeUnderOne > 2) {
            // Blacken screen
            realCamera.cullingMask = 0;
            realCamera.clearFlags = CameraClearFlags.SolidColor;
            realCamera.backgroundColor = Color.black;
            // TODO: darken time and spawn the character
            realPlayer.transform.position = new Vector3(gameObject.transform.position.x, 17, gameObject.transform.position.z);
            gameObject.SetActive(false);
            realPlayer.SetActive(true);

            Invoke("FinishCutScene", 1);
            scriptFrozen = true;
        }
    }

    void FinishCutScene() {
        realCamera.cullingMask = -1;
        realCamera.clearFlags = CameraClearFlags.Skybox;
    }

}
