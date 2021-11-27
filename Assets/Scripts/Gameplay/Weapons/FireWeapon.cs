using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{

    public GameObject player;
    public GameObject playerCamera;
    public float roundsPerMinute;
    public float magSize;
    public float initialAmmo;
    public float damagePerHit;

    private float requiredTimeBetweenShot;
    private float timeSinceLastShot;
    private CharacterStatManager stats;
    private float currentMagSize;
    private float reserveAmmo;

    void Start() {
        stats = player.GetComponent<CharacterStatManager>();
        requiredTimeBetweenShot = 1 / (roundsPerMinute / 60);
        timeSinceLastShot = 0;
        currentMagSize = initialAmmo >= magSize ? magSize : initialAmmo;
        reserveAmmo = initialAmmo >= magSize ? initialAmmo - magSize : 0;
    }

    void Update() {
        bool fired = false;

        if(Input.GetKey(KeyCode.Mouse0) && currentMagSize > 0) { 
            if(timeSinceLastShot > requiredTimeBetweenShot) {
                Fire();
                Debug.Log("Time since last " + timeSinceLastShot);
                this.timeSinceLastShot = 0;
                this.currentMagSize -= 1;
                fired = true;
                Debug.Log(currentMagSize + " / " + magSize + "; reserve = " + reserveAmmo);
            }
        } else if (Input.GetKeyDown(KeyCode.R)) {
            float newMagSize = reserveAmmo >= (magSize - currentMagSize) ? magSize : reserveAmmo + currentMagSize;
            reserveAmmo = reserveAmmo >= (magSize - currentMagSize) ? reserveAmmo - (magSize - currentMagSize) : 0;
            currentMagSize = newMagSize;
        }

        if(!fired) {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    private void Fire() {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~(1 << 8));

        if(didHit) {
            if(hit.collider == null) {
                return;
            }

            EnemyStatManager enemyStats = hit.collider.gameObject.GetComponent<EnemyStatManager>();

            if(enemyStats != null) {
                enemyStats.ApplyDamage(damagePerHit * stats.damageMultiplier);
                Debug.DrawRay(transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green, 5.0f);
            } else {
                Debug.DrawRay(transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red, 5.0f);
            }
        } else {
            Debug.DrawRay(transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.blue, 5.0f);
        }

    }
}
