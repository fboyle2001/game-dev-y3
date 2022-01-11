using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

/**
* Stats specific to each character rather than globally to the player
* e.g. current health
**/
public class CharacterStats : MonoBehaviour {

    public float maxHealth;
    public AudioClip damageClip;

    private List<System.Action<CharacterStats, float>> healthUpdateListeners = new List<System.Action<CharacterStats, float>>();
    private List<System.Action<CharacterStats>> statUpdateListeners = new List<System.Action<CharacterStats>>();

    private float rumbleDuration = 1.0f;

    private float currentHealth;
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
        // Need to scale max health as we go along
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
        // Apply damage over time effect

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

        // Regeneration of health smoothly over time

        if(currentHealth < maxHealth && stats.GetRegenPerSecond() > 0) {
            Heal(stats.GetRegenPerSecond() * Time.deltaTime);
        }
    }

    // Allow other objects to track the health of the characters

    public void RegisterHealthUpdateListener(System.Action<CharacterStats, float> listener) {
        healthUpdateListeners.Add(listener);
        PropagateHealthEvent(0);
    }

    public void ApplyDamage(float damage) {
        // Non-linear scale for the damage taken based on armour, damage reduction = 1 / (0.04 * armour + 1)
        // e.g. 100 damage with 0 armour = 100 health taken, 100 damage with 25 armour = 50 health taken, 100 damage with 100 armour = 20 health taken
        // essentially more and more armour is needed to have the same effect as low armour at the start
        float realDamage = damage / (0.04f * stats.GetArmour() + 1);
        currentHealth = Mathf.Clamp(currentHealth - realDamage, 0, maxHealth);
        AudioSource.PlayClipAtPoint(damageClip, transform.position);

        // Feedback via rumble if playing on controller
        Gamepad.current?.SetMotorSpeeds(0.3f, 0.6f);

        PropagateHealthEvent(-realDamage);

        CancelInvoke("EndRumble");
        Invoke("EndRumble", rumbleDuration);
    }

    public void Heal(float amount) {
        // Heal a fixed amount
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        PropagateHealthEvent(amount);
    }

    public void HealAsPercent(float percentage) {
        // Heal as a percentage of max health
        Heal(maxHealth * Mathf.Clamp01(percentage));
    }

    public void SetHealth(float health) {
        health = Mathf.Clamp(health, 0, maxHealth);
        float change = currentHealth - health;
        currentHealth = health;
        PropagateHealthEvent(change);
    }

    public void ApplyDamageOverTimeWithGoal(float goal, float time) {
        float damagePerSecond = (currentHealth - goal) / time;
        currentDOTEffect = damagePerSecond;
        maxDOTTime = time;
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

    private void EndRumble() {
        InputSystem.ResetHaptics();
    }

}
