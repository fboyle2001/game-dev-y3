using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicOrc : EnemyBase {
    
    private float timeBetweenAttacks = 2;
    private float attackRange = 10;
    private float targetGap = 4;
    private float damagePerAttack;

    private float timeSinceLastAttack = 0;
    private bool attacking = false;

    new void Awake() {
        base.Awake();
        damagePerAttack = 8 * level;
    }

    new void OnEnable() {
        base.OnEnable();
        stats.SetArmour(3 * level);
        stats.SetMaxHealth(40 * level);
        stats.SetRegenPerSecond(0.1f * level);
        stats.SetXPValue(9 * level);
        stats.SetGoldValue(100 * level);
    }

    void Start() {
        SetActive(true);
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

}
