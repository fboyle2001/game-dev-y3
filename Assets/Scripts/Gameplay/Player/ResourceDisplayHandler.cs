using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceDisplayHandler : MonoBehaviour
{

    public GameObject goldText;
    public GameObject xpText;
    public GameObject levelText;

    void OnEnable() {
        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<PlayerResources>().RegisterResourceUpdateListener((resources) => {
            goldText.GetComponent<TMP_Text>().SetText(resources.GetGold().ToString());
            xpText.GetComponent<TMP_Text>().SetText(resources.GetXP().ToString("0") + "/" + resources.GetXPForNextLevel().ToString());
            levelText.GetComponent<TMP_Text>().SetText(resources.GetCurrentExperienceLevel().ToString());
        });
    }
}
