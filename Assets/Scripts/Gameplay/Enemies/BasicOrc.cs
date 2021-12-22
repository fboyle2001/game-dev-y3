using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicOrc : EnemyBase {
    
    public float timeBetweenAttacks;
    public float attackRange;
    public float targetGap;
    public float damagePerAttack;

    private float timeSinceLastAttack = 0;
    private bool attacking = false;

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
        Debug.Log("attacked");
        animator.SetBool("waiting_to_attack", true);
        animator.SetBool("attacking", false);
        attacking = false;
    }

}
