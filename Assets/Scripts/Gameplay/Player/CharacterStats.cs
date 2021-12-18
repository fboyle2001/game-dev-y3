using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float maxHealth;
    private float currentHealth;
    private float armour = 0;
    private float regen = 0;
    private float damageMultiplier = 1;

    private List<System.Action<CharacterStats, float>> healthUpdateListeners = new List<System.Action<CharacterStats, float>>();
    private List<System.Action<CharacterStats>> statUpdateListeners = new List<System.Action<CharacterStats>>();

    private float currentDOTEffect = 0;
    private float appliedDOTTime = 0;
    private float maxDOTTime = 0;

    void OnEnable() {
        currentHealth = maxHealth;
        PropagateHealthEvent(0);
    }

    void Update() {
        if(currentDOTEffect != 0) {
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

        if(currentHealth < maxHealth && regen > 0) {
            Heal(regen * Time.deltaTime);
        }
    }

    public void RegisterHealthUpdateListener(System.Action<CharacterStats, float> listener) {
        healthUpdateListeners.Add(listener);
        PropagateHealthEvent(0);
    }

    public void RegisterStatUpdateListener(System.Action<CharacterStats> listener) {
        statUpdateListeners.Add(listener);
        PropagateStatEvent();
    }

    public void ApplyDamage(float damage) {
        // Scale the damage based on armour, damage reduction = 1 / (0.04 * armour + 1)
        // e.g. 100 damage with 0 armour = 100 health taken, 100 damage with 100 armour = 20 health taken
        float realDamage = damage / (0.04f * armour + 1);
        currentHealth = Mathf.Clamp(currentHealth - realDamage, 0, maxHealth);
        PropagateHealthEvent(realDamage);
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        PropagateHealthEvent(amount);
    }

    public void SetMaxHealth(float maxHealth) {
        this.maxHealth = maxHealth;
        PropagateHealthEvent(0);
        PropagateStatEvent();
    }

    public void AddMaxHealth(float amount) {
        this.maxHealth += amount;
        PropagateHealthEvent(0);
        PropagateStatEvent();
    }

    public void SetHealth(float health) {
        health = Mathf.Clamp(health, 0, maxHealth);
        float change = currentHealth - health;
        currentHealth = health;
        PropagateHealthEvent(change);
    }

    public void ApplyDamageOverTime(float damage, float time) {
        float damagePerSecond = damage / time;
        currentDOTEffect = damagePerSecond;
        maxDOTTime = time;
    }

    public void SetDamageMultiplier(float damageMultiplier) {
        this.damageMultiplier = damageMultiplier;
        PropagateStatEvent();
    }

    public void AddDamageMultiplier(float amount) {
        this.damageMultiplier += amount;
        PropagateStatEvent();
    }

    public void SetRegenPerSecond(float regen) {
        this.regen = regen;
        PropagateStatEvent();
    }

    public void AddRegenPerSecond(float amount) {
        this.regen += amount;
        PropagateStatEvent();
    }

    public void SetArmour(float armour) {
        this.armour = armour;
        PropagateStatEvent();
    }

    public void AddArmour(float amount) {
        this.armour += amount;
        PropagateStatEvent();
    }

    public float GetMaxHealth() {
        return maxHealth;
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    public float GetArmour() {
        return armour;
    }

    public float GetRegenPerSecond() {
        return regen;
    }

    public float GetDamageMultiplier() {
        return damageMultiplier;
    }

    private void PropagateHealthEvent(float change) {
        healthUpdateListeners.ForEach(action => action(this, change));
    }

    private void PropagateStatEvent() {
        statUpdateListeners.ForEach(action => action(this));
    }

}
