using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{

    public CutScene cutScene;

    private bool triggered = false;

    void OnTriggerEnter(Collider collider) {
        if(!cutScene.IsCutSceneActivatable() || triggered) return;

        if((collider.gameObject.name == "Primary" || collider.gameObject.name == "Secondary") && cutScene.IsCutSceneActivatable()) {
            triggered = true;
            cutScene.StartCutScene();
        }
    }
}
