using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStats : MonoBehaviour {
    
    public GameObject healthBarBackground;
    public GameObject healthBar;
    public GameObject healthText;

    private float maxHealth = 1;
    private float armour = 0;
    private float regenPerSecond = 0;
    private float xpValue = 0;
    private int goldValue = 0;

    private List<System.Action<EnemyStats, float>> damageListeners = new List<System.Action<EnemyStats, float>>();

    private float maxHealthBarWidth;
    private float health;
    private RectTransform healthBarTransform;
    private bool dead = false;

    void Awake() {
        // Apply scaling based on game difficulty
        maxHealth *= GlobalSettings.GetEnemyHealthScalar();
        health = maxHealth;
        // Health Bar UI 
        maxHealthBarWidth = (healthBarBackground.transform as RectTransform).rect.width;
        healthBarTransform = (healthBar.transform as RectTransform);
    }

    void Start() {
        UpdateHealthBar();
    }

    void Update() {
        if(dead || regenPerSecond == 0 || health >= maxHealth) return;
        // Heal the target if they have any regen
        Heal(regenPerSecond * Time.deltaTime);
    }

    public void SetMaxHealth(float maxHealth) {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public void SetArmour(float armour) {
        this.armour = armour;
    }

    public void SetRegenPerSecond(float regenPerSecond) {
        this.regenPerSecond = regenPerSecond;
    }

    public void SetXPValue(float xpValue) {
        this.xpValue = xpValue;
    }

    public float GetXPValue() {
        return xpValue;
    }

    public void SetGoldValue(int goldValue) {
        this.goldValue = goldValue;
    }

    public int GetGoldValue() {
        return goldValue;
    }

    public void RegisterDamageListener(System.Action<EnemyStats, float> action) {
        // Allows other classes to receive callback events when this enemy is damaged
        damageListeners.Add(action);
    }

    private void Heal(float amount) {
        // Heal this enemy, do not allow health to become negative or exceed their max health
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UpdateHealthBar();
    }

    public void Damage(float amount) {
        if(dead) return;
        
        float realDamage = amount / (0.04f * armour + 1);
        health = Mathf.Clamp(health - realDamage, 0, maxHealth);

        if(health == 0) {
            OnDeath();
        }

        UpdateHealthBar();
        PropagateDamageEvent(realDamage);
    }

    public bool IsDead() {
        return dead;
    }

    private void PropagateDamageEvent(float damage) {
        damageListeners.ForEach(action => action(this, damage));
    }

    private void UpdateHealthBar() {
        // Updates the Health Bar UI by changing bar overlay width
        float healthPercentage = health / maxHealth;
        healthBarTransform.sizeDelta = new Vector2(healthPercentage * maxHealthBarWidth, healthBarTransform.sizeDelta.y);
        healthText.GetComponent<TMP_Text>().text = Mathf.RoundToInt(health) + " / " + Mathf.RoundToInt(maxHealth);
    }
    
    private void OnDeath() {
        this.dead = true;
    }

}
