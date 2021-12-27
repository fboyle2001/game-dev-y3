using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour {
    
    public GameObject playableParent;
    public GameObject finishCamera;
    public GameObject deathUI;

    private GameObject gameManager;
    private CharacterManager characterManager;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        characterManager = gameManager.GetComponent<CharacterManager>();
    }

    void OnEnable() {
        characterManager.primary.GetComponent<CharacterStats>().RegisterHealthUpdateListener(OnDamage);
        characterManager.secondary.GetComponent<CharacterStats>().RegisterHealthUpdateListener(OnDamage);
    }

    private void OnDamage(CharacterStats stats, float change) {
        if(stats.GetCurrentHealth() <= 0) {
            finishCamera.SetActive(true);
            deathUI.SetActive(true);
            playableParent.SetActive(false);
        }
    }

}
