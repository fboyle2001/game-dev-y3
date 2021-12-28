using UnityEngine;

/**
* Handles the behaviour for the Orc class of enemies
**/
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
    
    // In the Awake and OnEnable we set the stats and damage of the creature based
    // on their level (constants are pre-determined by them being of the Orc class)

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
        // If the enemy is not active (i.e. not path finding towards the target) then
        // don't waste time processing its' logic and physics

        if(!active) return;

        // Plays audio when the target enters the bounding rectangle
        if(lastTargetName == null) {
            lastTargetName = movementController.GetCurrentTarget()?.name;
        } else if (lastTargetName == movementController.home.name && movementController.GetCurrentTarget()?.name != lastTargetName) {
            audioSource.PlayOneShot(orcAttentionClip);
        }

        lastTargetName = movementController.GetCurrentTarget()?.name;

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
        AudioSource.PlayClipAtPoint(orcAttackClip, transform.position);
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
        audioSource.clip = orcDeathClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    protected override void OnDamageHandler(EnemyStats stats, float damage) {
        base.OnDamageHandler(stats, damage);
        
        if(!stats.IsDead()) {
            // Play some audio when they get damaged
            AudioSource.PlayClipAtPoint(orcDamageClip, transform.position, 0.6f);
        }
    }

}
