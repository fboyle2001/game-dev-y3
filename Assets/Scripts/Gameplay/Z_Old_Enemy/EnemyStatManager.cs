using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatManager : MonoBehaviour
{

    public float maxHealth;
    public float regenerationPerSecond;
    public float armour;
    public float initialHealth;
    public float damageMultiplier;

    private float currentHealth;

    void Start() {
        currentHealth = initialHealth;
    }

    public void ApplyDamage(float rawDamage) {
        float adjustedDamage = rawDamage;
        float remainingHealth = currentHealth - adjustedDamage;
        
        if(remainingHealth <= 0) {
            this.currentHealth = 0;
            this.OnDeath();
            return;
        }

        this.currentHealth = remainingHealth;
        Debug.Log(gameObject.name + " now has " + currentHealth + " health");
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

    private void OnDeath() {
        this.currentHealth = 0;
        gameObject.SetActive(false);
    }
    
}
