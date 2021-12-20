using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStats : MonoBehaviour {
    
    public GameObject healthBarBackground;
    public GameObject healthBar;
    public GameObject healthText;

    public float maxHealth;
    public float armour;
    public float regenPerSecond;

    private List<System.Action<EnemyStats, float>> damageListeners = new List<System.Action<EnemyStats, float>>();

    private float maxHealthBarWidth;
    private float health;
    private RectTransform healthBarTransform;
    private bool dead = false;

    void Start() {
        health = maxHealth;
        maxHealthBarWidth = (healthBarBackground.transform as RectTransform).rect.width;
        healthBarTransform = (healthBar.transform as RectTransform);
        UpdateHealthBar();
    }

    void Update() {
        if(dead || regenPerSecond == 0 || health >= maxHealth) return;
        Heal(regenPerSecond * Time.deltaTime);
    }

    public void RegisterDamageListener(System.Action<EnemyStats, float> action) {
        damageListeners.Add(action);
    }

    private void Heal(float amount) {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UpdateHealthBar();
    }

    public void Damage(float amount) {
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
        float healthPercentage = health / maxHealth;
        healthBarTransform.sizeDelta = new Vector2(healthPercentage * maxHealthBarWidth, healthBarTransform.sizeDelta.y);
        healthText.GetComponent<TMP_Text>().text = Mathf.RoundToInt(health) + " / " + Mathf.RoundToInt(maxHealth);
    }
    
    private void OnDeath() {
        this.dead = true;
    }

}
