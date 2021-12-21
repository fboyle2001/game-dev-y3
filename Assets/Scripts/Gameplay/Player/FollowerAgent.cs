using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerAgent : MonoBehaviour {

    public GameObject target;
    public float teleportDistance = 40;

    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate() {
        Vector3 targetDifference = target.transform.position - transform.position;

        if(targetDifference.magnitude > teleportDistance) {
            TeleportNearTarget();
        } else {
            MoveTowardsTarget();
        }
    }

    private void TeleportNearTarget() {
        Vector3 teleportTarget = target.transform.position - 2 * Vector3.back + 2 * Vector3.up;
        Collider[] hits = Physics.OverlapSphere(teleportTarget, 1);
        bool safe = true;

        foreach(Collider c in hits) {
            if(c.name != "Terrain" && c.name != "Player") {
                safe = false;
            }
        }

        if(safe) {
            transform.position = teleportTarget;
        } else {
            Debug.Log("Not safe to teleport");
        }
    }

    private void MoveTowardsTarget() {
        Vector3 targetPosition = target.transform.position;
        agent.SetDestination(targetPosition);
        Quaternion lookAt = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.fixedDeltaTime);
    }

}
