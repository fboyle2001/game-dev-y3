using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonOutlaw : EnemyBase {
    
    public AudioClip skeletonAttackClip;
    public AudioClip skeletonDamageClip;
    public AudioClip skeletonDeathClip;

    private AudioSource audioSource;

    private float timeBetweenAttacks = 1.5f;
    private float attackRange = 5;
    private float targetGap = 4;
    private float damagePerAttack;

    private float timeSinceLastAttack = 0;
    private bool attacking = false;

    // In the Awake and OnEnable we set the stats and damage of the creature based
    // on their level (constants are pre-determined by them being of the Orc class)

    new void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
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

    void FixedUpdate() {
        // If the enemy is not active (i.e. not path finding towards the target) then
        // don't waste time processing its' logic and physics

        if(!active) return;

        Vector3 targetDirection = (target.transform.position - transform.position);
        float distanceToTarget = targetDirection.magnitude;

        // Already attacking so just wait until that is over
        if(attacking) return;

        // Don't want to spam attacks so track time since the last attack
        timeSinceLastAttack += Time.fixedDeltaTime;

        // Only attack when in range of the target
        if(distanceToTarget <= attackRange) {
            // Don't get too close, if we are then slow down or stop
            if(distanceToTarget <= targetGap) {
                animator.SetFloat("speed", 0);
                movementController.SetSpeedFactor(0f);
            } else {
                movementController.SetSpeedFactor(0.8f);
            }

            // Trigger the attack and schedule the attack animation
            if(timeSinceLastAttack >= timeBetweenAttacks && !attacking) {
                animator.SetBool("waiting_to_attack", false);
                animator.SetBool("attacking", true);
                Attack();
            }
        } else {
            // Continue moving normally towards the target
            movementController.SetSpeedFactor(1);
            animator.SetBool("waiting_to_attack", false);
            animator.SetBool("attacking", false);
        }
    }

    private void Attack() {
        // Play the attacking audio and prepare to stop the animation
        AudioSource.PlayClipAtPoint(skeletonAttackClip, transform.position);
        attacking = true;
        movementController.SetSpeedFactor(0f);
        Invoke("StopAttackAnimation", 0.5f);
    }

    private void StopAttackAnimation() {
        timeSinceLastAttack = 0;
        // Apply the damage to the target
        target.GetComponent<CharacterStats>().ApplyDamage(damagePerAttack);
        animator.SetBool("waiting_to_attack", true);
        animator.SetBool("attacking", false);
        attacking = false;
    }

    protected override void OnDeath() {
        // EnemyBase handles the main death logic instead
        base.OnDeath();
        // Play the death sounds
        audioSource.Stop();
        audioSource.clip = skeletonDeathClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    protected override void OnDamageHandler(EnemyStats stats, float damage) {
        base.OnDamageHandler(stats, damage);
        
        if(!stats.IsDead()) {
            // Play some audio when they get damaged
            AudioSource.PlayClipAtPoint(skeletonDamageClip, transform.position, 0.6f);
        }
    }

}
