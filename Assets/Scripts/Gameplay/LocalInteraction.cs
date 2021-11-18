using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalInteraction : MonoBehaviour
{

    public GameObject desiredTarget;
    public int activationDistance = 8;
    public Text displayText;
    public string interactionMessage = "to interact";
    public ManageSecondaryUI secondaryUI;
    public bool playerOnly = true;

    private SwitchPlayer gameCharacterManager;

    void Start() {
        gameCharacterManager = GameObject.FindWithTag("Game Manager").GetComponent<SwitchPlayer>();
    }

    void Update() {
        if(playerOnly && !gameCharacterManager.isPlayerActive) {
            displayText.gameObject.SetActive(false);
            displayText.text = "";
            return;
        }

        if((desiredTarget.transform.position - transform.position).magnitude < activationDistance) {
            transform.LookAt(desiredTarget.transform);
            displayText.text = "Press E " + interactionMessage;
            displayText.gameObject.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E)) {
                if(secondaryUI) {
                    secondaryUI.OpenUI();
                }
            }
        } else {
            displayText.gameObject.SetActive(false);
            displayText.text = "";
        }
    }
}
