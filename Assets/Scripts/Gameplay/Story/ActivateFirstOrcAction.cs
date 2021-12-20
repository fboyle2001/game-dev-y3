using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFirstOrcAction : MonoBehaviour, ITriggerAction {

    public GameObject orc;

    public void PerformAction() {
        orc.GetComponent<BasicOrc>().SetActive(true);
    }

}
