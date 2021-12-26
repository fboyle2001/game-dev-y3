using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicOrc : EnemyBase {

    public AudioClip orcAttackClip;
    public AudioClip orcAttentionClip;
    public AudioClip orcDeathClip;
    public AudioClip orcDamageClip;
    
    private AudioSource audioSource;

    private float timeBetweenAttacks = 2;
    private float attackRange = 10;
    private float targetGap = 4;
    private float damagePerAttack;

    private float timeSinceLastAttack = 0;
    private bool attacking = false;
    private string lastTargetName = null;

    new void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
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

    void FixedUpdate() {
        if(!active) return;

        if(lastTargetName == null) {
            lastTargetName = movementController.GetCurrentTarget()?.name;
        } else if (lastTargetName == movementController.home.name && movementController.GetCurrentTarget()?.name != lastTargetName) {
            audioSource.PlayOneShot(orcAttentionClip);
        }

        lastTargetName = movementController.GetCurrentTarget()?.name;

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
        AudioSource.PlayClipAtPoint(orcAttackClip, transform.position);
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

    protected override void OnDeath() {
        base.OnDeath();
        audioSource.Stop();
        audioSource.clip = orcDeathClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    protected override void OnDamageHandler(EnemyStats stats, float damage) {
        base.OnDamageHandler(stats, damage);
        
        if(!stats.IsDead()) {
            AudioSource.PlayClipAtPoint(orcDamageClip, transform.position, 0.6f);
        }
    }

}
