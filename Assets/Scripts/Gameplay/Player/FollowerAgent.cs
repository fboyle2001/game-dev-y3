using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerAgent : MonoBehaviour {

    public GameObject target;
    public float teleportDistance = 40;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        Vector3 targetDifference = target.transform.position - transform.position;

        if(targetDifference.magnitude > teleportDistance) {
            MoveTowardsTarget();
            TeleportNearTarget();
        } else {
            MoveTowardsTarget();
        }

        float horizontalVelocity = Mathf.Sqrt(Mathf.Pow(agent.velocity.x, 2) + Mathf.Pow(agent.velocity.z, 2));

        if(horizontalVelocity > 0.2f) {
            audioSource.pitch = Mathf.Min(Mathf.Max(0.2f, horizontalVelocity / 11), 1.1f);
            if(!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }

        animator.SetFloat("speed", horizontalVelocity);
    }

    private void TeleportNearTarget() {
        Vector3[] directions = new Vector3[]{Vector3.back, Vector3.left, Vector3.right};

        foreach(Vector3 direction in directions) {
            Vector3 teleportTarget = target.transform.position - 4 * direction + 2 * Vector3.up;
            Collider[] hits = Physics.OverlapSphere(teleportTarget, 1);
            bool safe = true;

            foreach(Collider c in hits) {
                if(c.name != "Terrain" && c.name != "Player" && c.name != "Pipe" && c.name != "Floor Smooth 1" && c.name != "Floor Smooth 2") {
                    Debug.Log("[DIR " + direction + "] unable to teleport due to " + c.name);
                    safe = false;
                }
            }

            if(safe) {
                transform.position = teleportTarget;
                return;
            }
        }
    }

    private void MoveTowardsTarget() {
        Vector3 targetPosition = target.transform.position;
        agent.SetDestination(targetPosition);
        Quaternion lookAt = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.fixedDeltaTime);
    }

}
