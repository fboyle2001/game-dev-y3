using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayGold : MonoBehaviour
{

    public GameObject player;
    private PlayerAssets assetScript;
    private TMP_Text goldText;

    void Start() {
        assetScript = player.GetComponent<PlayerAssets>();
        goldText = GetComponent<TMP_Text>();
        goldText.text = assetScript.GetGold().ToString();
    }

    void Update() {
        goldText.text = assetScript.GetGold().ToString();
    }
}
