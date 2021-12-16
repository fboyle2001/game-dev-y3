using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealthBar : MonoBehaviour
{

    public GameObject healthBar;
    public GameObject healthBarBlocker;
    private ActiveCharacterManager characterManager;
    private float maxWidth;

    void Start() {
        this.characterManager = GameObject.FindWithTag("Game Manager").GetComponent<ActiveCharacterManager>();
        this.maxWidth = healthBar.GetComponent<RectTransform>().sizeDelta.x;
    }

    void Update() {
        CharacterStatManager active = characterManager.GetActiveCharacter().GetComponent<CharacterStatManager>();
        RectTransform rectTransform = healthBarBlocker.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(maxWidth - (active.GetCurrentHealth() / active.maxHealth) * maxWidth, rectTransform.sizeDelta.y);
    }
}
