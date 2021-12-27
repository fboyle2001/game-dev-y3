using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour
{

    public GameObject mainPanel;

    public void Back() {
        mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
