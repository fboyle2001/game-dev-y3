using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float maxHealth;
    private float currentHealth;

    private List<System.Action<CharacterStats, float>> healthUpdateListeners = new List<System.Action<CharacterStats, float>>();
    private List<System.Action<CharacterStats>> statUpdateListeners = new List<System.Action<CharacterStats>>();

    private float originalMaxHealth;
    private float currentDOTEffect = 0;
    private float appliedDOTTime = 0;
    private float maxDOTTime = 0;
    private PlayerStats stats;

    void Awake() {
        originalMaxHealth = maxHealth;
        stats = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<PlayerStats>();
    }

    void Start() {
        stats.RegisterStatChangeListener(OnMaxHealthMultiplierChange);
    }

    private void OnMaxHealthMultiplierChange(PlayerStats stats, float maxHealthChange, float dmgChange, float regenChange, float armourChange) {
        maxHealth = originalMaxHealth * stats.GetMaxHealthMultiplier();
        PropagateHealthEvent(0);
    }

    public float GetOriginalMaxHealth() {
        return maxHealth;
    }

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

        if(currentHealth < maxHealth && stats.GetRegenPerSecond() > 0) {
            Heal(stats.GetRegenPerSecond() * Time.deltaTime);
        }
    }

    public void RegisterHealthUpdateListener(System.Action<CharacterStats, float> listener) {
        healthUpdateListeners.Add(listener);
        PropagateHealthEvent(0);
    }

    public void ApplyDamage(float damage) {
        // Scale the damage based on armour, damage reduction = 1 / (0.04 * armour + 1)
        // e.g. 100 damage with 0 armour = 100 health taken, 100 damage with 100 armour = 20 health taken
        float realDamage = damage / (0.04f * stats.GetArmour() + 1);
        currentHealth = Mathf.Clamp(currentHealth - realDamage, 0, maxHealth);
        PropagateHealthEvent(-realDamage);
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        PropagateHealthEvent(amount);
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

    public float GetMaxHealth() {
        return maxHealth;
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    private void PropagateHealthEvent(float change) {
        healthUpdateListeners.ForEach(action => action(this, change));
    }

}
