using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicOrc : MonoBehaviour
{

    public float timeBetweenAttacks;
    public float attackRange;
    public float targetGap;
    public float damagePerAttack;
    public GameObject enemyUI;

    private Animator animator;
    private AnimatedEnemyMovement movementController;
    private GameObject target;
    private GameObject gameManager;
    private bool active = false;
    private float timeSinceLastAttack = 0;
    private bool attacking = false;
    private EnemyStats stats;

    void OnEnable() {
        movementController = GetComponent<AnimatedEnemyMovement>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();
        stats.RegisterDamageListener(OnDamage);
    }

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        gameManager.GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);
        SetActive(true);

        gameManager.GetComponent<PlayerInventory>().AddItemToInventory("rustedBow", 1);
    }

    void FixedUpdate() {
        if(!active) return;

        Vector3 targetDirection = (target.transform.position - transform.position);
        float distanceToTarget = targetDirection.magnitude;

        if(attacking) return;

        timeSinceLastAttack += Time.fixedDeltaTime;

        if(distanceToTarget <= attackRange) {
            if(distanceToTarget <= targetGap) {
                animator.SetFloat("speed", 0);
                movementController.SetSpeedFactor(0f);
            } else {
                movementController.SetSpeedFactor(0.8f);
            }

            if(timeSinceLastAttack >= timeBetweenAttacks && !attacking) {
                animator.SetBool("waiting_to_attack", false);
                animator.SetBool("attacking", true);
                Attack();
            }
        } else {
            movementController.SetSpeedFactor(1);
            animator.SetBool("waiting_to_attack", false);
            animator.SetBool("attacking", false);
        }
    }

    void OnDisable() {
        gameManager.GetComponent<CharacterManager>().DeregisterActiveChangeListener(gameObject);
    }

    public void SetActive(bool active) {
        this.active = active;
        target = gameManager.GetComponent<CharacterManager>().GetActiveCharacter();
        movementController.SetTarget(target);
        movementController.SetActive(active);
    }

    private void OnActiveCharacterChange(GameObject newActive) {
        target = newActive;
        movementController.SetTarget(newActive);
    }

    private void Attack() {
        attacking = true;
        movementController.SetSpeedFactor(0f);
        Invoke("StopAttackAnimation", 0.5f);
    }

    private void StopAttackAnimation() {
        timeSinceLastAttack = 0;
        target.GetComponent<CharacterStats>().ApplyDamage(damagePerAttack);
        animator.SetBool("waiting_to_attack", true);
        animator.SetBool("attacking", false);
        attacking = false;
    }

    private void OnDamage(EnemyStats stats, float damage) {
        bool dead = stats.IsDead();

        if(dead) {
            animator.SetBool("is_dead", true);
            SetActive(false);
            enemyUI.SetActive(false);
        }
    }

}
