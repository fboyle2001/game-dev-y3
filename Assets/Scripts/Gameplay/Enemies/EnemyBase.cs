using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyBase : MonoBehaviour {
    
    private static Dictionary<int, System.Action<EnemyBase>> globalDamageHandlers = new Dictionary<int, System.Action<EnemyBase>>();

    // Allow other objects to receive callback events when any enemy is damaged
    public static void RegisterGlobalDamageHandler(GameObject owner, System.Action<EnemyBase> action) {
        globalDamageHandlers.Add(owner.GetInstanceID(), action);
    }

    public GameObject nameText;
    public GameObject levelText;
    public GameObject enemyUI;
    public string enemyName;
    public string identifier;
    public int level;

    protected GameObject gameManager;
    protected GameObject xpOrbPrefab;

    protected Animator animator;
    protected AnimatedEnemyMovement movementController;
    protected EnemyStats stats;
    protected GameObject target;
    protected bool active;

    public void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");

        stats = GetComponent<EnemyStats>();
        movementController = GetComponent<AnimatedEnemyMovement>();
        animator = GetComponent<Animator>();

        active = false;
    }

    public void OnEnable() {
        xpOrbPrefab = gameManager.GetComponent<PrefabStorage>().xpOrb;

        // Want to track when the active character changes so we can target the new active instead
        gameManager.GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);
        // Track when the enemy takes damage
        stats.RegisterDamageListener(OnDamageHandler);
    }

    public void Start() {
        SetActive(true);
        // Update the UI above the head of the enemy with initial values
        nameText.GetComponent<TMP_Text>().text = gameManager.GetComponent<LocaleManager>().GetString(enemyName);
        levelText.GetComponent<TMP_Text>().text = gameManager.GetComponent<LocaleManager>().GetString("txt_enm_level") + " " + level;
    }

    public void SetActive(bool active) {
        this.active = active;
        target = gameManager.GetComponent<CharacterManager>().GetActiveCharacter();
        movementController.SetTarget(target);
        movementController.SetActive(active);
    }

    public EnemyStats GetStats() {
        return stats;
    }

    private void OnActiveCharacterChange(GameObject newActive) {
        // Update the path finding target to the new active character
        target = newActive;
        movementController.SetTarget(newActive);
    }

    protected virtual void OnDamageHandler(EnemyStats stats, float damage) {
        if(stats.IsDead()) {
            // Handle the death of this enemy
            OnDeath();
            gameManager.GetComponent<CharacterManager>().DeregisterActiveChangeListener(gameObject);
            SetActive(false);
            enemyUI.SetActive(false);
            // Play the death animation
            animator.SetBool("is_dead", true);
        }

        foreach(System.Action<EnemyBase> action in globalDamageHandlers.Values) {
            action(this);
        }
    }

    protected virtual void OnDeath() {
        // Spawn an XP orb on death that seeks the active player (using steering behaviour)
        GameObject xpOrb = Instantiate(xpOrbPrefab, transform.position + GetComponent<Collider>().bounds.extents.y * Vector3.up, xpOrbPrefab.transform.rotation);
        xpOrb.GetComponent<XPOrbSteering>().SetXpValue(stats.GetXPValue());
        // Award the player gold for killing the enemy
        gameManager.GetComponent<PlayerResources>().AddGold(stats.GetGoldValue());

        // Destroy the game object after 2 seconds so it clears the enemy and their collider
        Invoke("ClearBody", 2);
    }

    private void ClearBody() {
        gameObject.SetActive(false);
    }

}
