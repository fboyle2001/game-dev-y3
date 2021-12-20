using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFirstOrcAction : MonoBehaviour, ITriggerAction {

    public GameObject orc;

    public void PerformAction() {
        Debug.Log("Woke Orc");
        orc.GetComponent<EnemyMovement>().SetActive(true);
    }

}
