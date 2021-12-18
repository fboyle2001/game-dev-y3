using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayHandler : MonoBehaviour
{

    public GameObject armourText;
    public GameObject regenText;
    public GameObject maxHealthText;
    public GameObject damageMultiplierText;

    private CharacterStats primaryStats;

    void OnEnable() {
        primaryStats = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        primaryStats.RegisterStatUpdateListener(new System.Action<CharacterStats>((stats) => {
            armourText.GetComponent<TMP_Text>().SetText(stats.GetArmour().ToString("0.0"));
            regenText.GetComponent<TMP_Text>().SetText(stats.GetRegenPerSecond().ToString("0.0"));
            maxHealthText.GetComponent<TMP_Text>().SetText(stats.GetMaxHealth().ToString("0.0"));
            damageMultiplierText.GetComponent<TMP_Text>().SetText(stats.GetDamageMultiplier().ToString("0.0"));
        }));
    }

}
