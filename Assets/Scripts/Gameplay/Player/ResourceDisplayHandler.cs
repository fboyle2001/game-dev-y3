using UnityEngine;
using TMPro;

/**
* Automatically updates the resource display in the shop and inventory UIs
**/
public class ResourceDisplayHandler : MonoBehaviour {

    public GameObject goldText;
    public GameObject xpText;
    public GameObject levelText;

    void Awake() {
        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<PlayerResources>().RegisterResourceUpdateListener((resources, discard_a, discard_b) => {
            goldText.GetComponent<TMP_Text>().SetText(resources.GetGold().ToString());
            xpText.GetComponent<TMP_Text>().SetText(resources.GetXP().ToString("0") + "/" + resources.GetXPForNextLevel().ToString());
            levelText.GetComponent<TMP_Text>().SetText(resources.GetCurrentExperienceLevel().ToString());
        });
    }
}
