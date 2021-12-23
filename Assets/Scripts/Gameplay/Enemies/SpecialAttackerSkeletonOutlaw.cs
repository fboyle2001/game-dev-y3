using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackerSkeletonOutlaw : EnemyBase {
    
    public GameObject specialTarget;
    private float timeBetweenAttacks = 1.5f;
    private float attackRange = 2;
    private float targetGap = 2;
    private float damagePerAttack;

    private float timeSinceLastAttack = 0;
    private bool attacking = false;

    new void Awake() {
        base.Awake();
        damagePerAttack = 2 * level;
    }
    
    new void OnEnable() {
        base.OnEnable();
        stats.SetArmour(2 * level);
        stats.SetMaxHealth(15 * level);
        stats.SetRegenPerSecond(0f);
        stats.SetXPValue(2 * level);
        stats.SetGoldValue(30 * level);
    }

    void Start() {
        SetActive(true);
    }

    void FixedUpdate() {
        if(!active) return;
        GetComponent<AnimatedEnemyMovement>().SetTarget(specialTarget);

        Vector3 targetDirection = (specialTarget.transform.position - transform.position);
        float distanceToTarget = targetDirection.magnitude;

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

    private void Attack() {
        attacking = true;
        movementController.SetSpeedFactor(0f);
        Invoke("StopAttackAnimation", 2f);
    }

    private void StopAttackAnimation() {
        timeSinceLastAttack = 0;
        animator.SetBool("waiting_to_attack", true);
        animator.SetBool("attacking", false);
        attacking = false;
    }

}
