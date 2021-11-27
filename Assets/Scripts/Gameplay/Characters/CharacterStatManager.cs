using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatManager : MonoBehaviour
{

    public float maxHealth;
    public float regenerationPerSecond;
    public float armour;
    public float initialHealth;
    public float damageMultiplier;
    public Image damageOverlay;

    private ActiveCharacterManager characterManager;
    private float currentHealth;
    private Coroutine activeOverlayHider;

    void Start() {
        currentHealth = initialHealth;
        characterManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<ActiveCharacterManager>();
    }

    public void ApplyDamage(float rawDamage) {
        float adjustedDamage = rawDamage;
        float remainingHealth = currentHealth - adjustedDamage;
        
        if(remainingHealth <= 0) {
            this.currentHealth = 0;
            this.OnCharacterDeath();
            return;
        }

        this.currentHealth = remainingHealth;
        damageOverlay.color = new Color32(255, 255, 255, (byte) (255 - (255 * (currentHealth / maxHealth))));

        if(activeOverlayHider != null) {
            StopCoroutine(activeOverlayHider);
        } 

        activeOverlayHider = StartCoroutine(HideDamageOverlay(2));
    }

    private IEnumerator HideDamageOverlay(float delay) {
        yield return new WaitForSeconds(delay);
        damageOverlay.color = new Color32(255, 255, 255, 0);
    }

    public void Heal(float healAmount) {
        if(healAmount < 0) {
            return;
        }

        this.currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
    }

    public float GetCurrentHealth() {
        return this.currentHealth;
    }

    private void OnCharacterDeath() {
        this.currentHealth = 0;
        characterManager.OnCharacterDeath(gameObject.tag == "Primary Character");
    }

}
