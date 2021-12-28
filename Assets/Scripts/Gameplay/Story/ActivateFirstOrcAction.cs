using UnityEngine;

/**
* Trigger action that activates the first Orc to begin path finding
**/
public class ActivateFirstOrcAction : MonoBehaviour, ITriggerAction {

    public GameObject orc;

    public void PerformAction() {
        orc.GetComponent<BasicOrc>().SetActive(true);
    }

}
