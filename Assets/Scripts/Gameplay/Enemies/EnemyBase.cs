using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {
    
    private static Dictionary<int, System.Action<EnemyBase>> globalDamageHandlers = new Dictionary<int, System.Action<EnemyBase>>();

    public static void RegisterGlobalDamageHandler(GameObject owner, System.Action<EnemyBase> action) {
        globalDamageHandlers.Add(owner.GetInstanceID(), action);
    }

    public GameObject enemyUI;
    public string identifier;

    protected GameObject gameManager;

    protected Animator animator;
    protected AnimatedEnemyMovement movementController;
    protected EnemyStats stats;
    protected GameObject target;
    protected bool active;

    public void OnEnable() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        stats = GetComponent<EnemyStats>();
        movementController = GetComponent<AnimatedEnemyMovement>();
        animator = GetComponent<Animator>();
        active = false;
        
        gameManager.GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);
        stats.RegisterDamageListener(OnDamageHandler);
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
        target = newActive;
        movementController.SetTarget(newActive);
    }

    private void OnDamageHandler(EnemyStats stats, float damage) {
        if(stats.IsDead()) {
            gameManager.GetComponent<CharacterManager>().DeregisterActiveChangeListener(gameObject);
            SetActive(false);
            enemyUI.SetActive(false);
            animator.SetBool("is_dead", true);
        }

        foreach(System.Action<EnemyBase> action in globalDamageHandlers.Values) {
            action(this);
        }
    }

}
