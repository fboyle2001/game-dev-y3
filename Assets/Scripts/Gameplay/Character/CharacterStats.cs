using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float maxHealth;
    private float currentHealth { get; set; }
    private float armour = 0;

    private List<System.Action<CharacterStats, float>> healthUpdateEvents = new List<System.Action<CharacterStats, float>>();

    private float currentDOTEffect = 0;
    private float appliedDOTTime = 0;
    private float maxDOTTime = 0;

    void OnEnable() {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " " + currentHealth);
    }

    void Update() {
        if(currentDOTEffect == 0) return;

        appliedDOTTime += Time.deltaTime;
        float time = Time.deltaTime;

        if(appliedDOTTime > maxDOTTime) {
            time = maxDOTTime - appliedDOTTime;
        }

        ApplyDamage(time * currentDOTEffect);

        if(appliedDOTTime > maxDOTTime) {
            currentDOTEffect = 0;
            appliedDOTTime = 0;
            maxDOTTime = 0;
        }
    }

    public void RegisterHealthUpdateListener(System.Action<CharacterStats, float> listener) {
        healthUpdateEvents.Add(listener);
    }

    public void ApplyDamage(float damage) {
        // Scale the damage based on armour, damage reduction = 1 / (0.04 * armour + 1)
        // e.g. 100 damage with 0 armour = 100 health taken, 100 damage with 100 armour = 20 health taken
        float realDamage = damage / (0.04f * armour + 1);
        currentHealth = Mathf.Clamp(currentHealth - realDamage, 0, maxHealth);
        PropagateHealthEvent(this, realDamage);
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        PropagateHealthEvent(this, amount);
    }

    public void SetMaxHealth(float maxHealth) {
        this.maxHealth = maxHealth;
        PropagateHealthEvent(this, 0);
    }

    public void SetHealth(float health) {
        health = Mathf.Clamp(health, 0, maxHealth);
        float change = currentHealth - health;
        currentHealth = health;
        PropagateHealthEvent(this, change);
    }

    public void ApplyDamageOverTime(float damage, float time) {
        float damagePerSecond = damage / time;
        currentDOTEffect = damagePerSecond;
        maxDOTTime = time;
    }

    public float GetMaxHealth() {
        return maxHealth;
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    private void PropagateHealthEvent(CharacterStats stats, float change) {
        healthUpdateEvents.ForEach(action => action(stats, change));
    }

}
