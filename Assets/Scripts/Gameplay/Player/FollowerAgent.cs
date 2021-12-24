using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerAgent : MonoBehaviour {

    public GameObject target;
    public float teleportDistance = 40;

    private NavMeshAgent agent;
    private Animator animator;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        Vector3 targetDifference = target.transform.position - transform.position;

        if(targetDifference.magnitude > teleportDistance) {
            MoveTowardsTarget();
            TeleportNearTarget();
        } else {
            MoveTowardsTarget();
        }

        animator.SetFloat("speed", agent.velocity.magnitude);
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
