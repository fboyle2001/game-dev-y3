using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{

    public CutScene cutScene;
    public string triggerTag = "Primary Character";

    private bool triggered = false;

    void OnTriggerEnter(Collider collider) {
        if(!cutScene.IsCutSceneActivatable() || triggered) return;

        if(collider.gameObject.tag == triggerTag && cutScene.IsCutSceneActivatable()) {
            triggered = true;
            cutScene.StartCutScene();
        }
    }
}
