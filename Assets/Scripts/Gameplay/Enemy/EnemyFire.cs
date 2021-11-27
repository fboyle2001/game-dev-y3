using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{

    public float reactionTime;
    public float roundsPerMinute;
    public float probabilityOfHit;
    public float maxRange;
    public float damagePerShot;

    private EnemyStatManager statManager;
    private ActiveCharacterManager characterManager;

    private float timeSinceSpotted;
    private float timeSinceLastShot;
    private float requiredTimeBetweenShot;
    private bool isFirstShot;

    void Start() {
        this.statManager = GetComponent<EnemyStatManager>();
        this.characterManager = GameObject.FindWithTag("Game Manager").GetComponent<ActiveCharacterManager>();
        this.timeSinceSpotted = 0;
        this.timeSinceLastShot = 0;
        this.requiredTimeBetweenShot = 1 / (roundsPerMinute / 60);
        this.isFirstShot = false;
    }

    void FixedUpdate() {
        timeSinceLastShot += Time.fixedDeltaTime;
        bool canSee = CanSeePlayer();

        if(!canSee) {
            this.timeSinceSpotted = 0;
            this.timeSinceLastShot = 0;
            this.isFirstShot = true;
        } else if (canSee) {
            timeSinceSpotted += Time.fixedDeltaTime;

            if(timeSinceSpotted > reactionTime && isFirstShot) {
                FireAtTarget();
                this.timeSinceLastShot = 0;
                this.isFirstShot = false;
            } else if(timeSinceLastShot > requiredTimeBetweenShot) {
                FireAtTarget();
                this.timeSinceLastShot = 0;
            }
        }
    }

    bool CanSeePlayer() {
        GameObject target = characterManager.GetActiveCharacter();
        Vector3 playerDirection = (target.transform.position - transform.position).normalized;

        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, playerDirection, out hit, maxRange);

        if(didHit) {
            if(hit.collider.gameObject.name == target.gameObject.name) {
                Debug.DrawRay(transform.position, playerDirection * hit.distance, Color.cyan, 2.0f);
                return true;
            } else {
                Debug.DrawRay(transform.position, playerDirection * hit.distance, Color.black, 2.0f);
            }
        } else {
            Debug.DrawRay(transform.position, playerDirection * maxRange, Color.magenta, 2.0f);
        }

        return false;
    }

    void FireAtTarget() {
        if(probabilityOfHit > Random.Range(0.0f, 1.0f)) {
            characterManager.GetActiveCharacter().GetComponent<CharacterStatManager>().ApplyDamage(damagePerShot * statManager.damageMultiplier);
        }
    }

}
