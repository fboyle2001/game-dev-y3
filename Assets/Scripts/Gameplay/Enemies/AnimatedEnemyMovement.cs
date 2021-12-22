using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatedEnemyMovement : MonoBehaviour {

    public GameObject cornerA;
    public GameObject cornerB;
    public GameObject home;
    public string animatorSpeedName;

    private Animator animator;
    private GameObject target;
    private GameObject gameManager;
    private bool active = false;
    private NavMeshAgent agent;

    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;
    private float speedFactor = 1;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        minX = Mathf.Min(cornerA.transform.position.x, cornerB.transform.position.x);
        maxX = Mathf.Max(cornerA.transform.position.x, cornerB.transform.position.x);
        minZ = Mathf.Min(cornerA.transform.position.z, cornerB.transform.position.z);
        maxZ = Mathf.Max(cornerA.transform.position.z, cornerB.transform.position.z);
    }

    void FixedUpdate() {
        agent.velocity = agent.velocity * speedFactor;
        animator.SetFloat(animatorSpeedName, agent.velocity.magnitude);
        if(!active) return;
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        Vector3 targetPosition = target.transform.position;

        if(targetPosition.x > maxX || targetPosition.x < minX) {
            agent.SetDestination(home.transform.position);
            return;
        }

        if(targetPosition.z > maxZ || targetPosition.z < minZ) {
            agent.SetDestination(home.transform.position);
            return;
        }

        agent.SetDestination(targetPosition);
        Quaternion lookAt = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.fixedDeltaTime);
    }

    public void SetActive(bool active) {
        this.active = active;
        agent.enabled = active;
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }

    public void SetSpeedFactor(float factor) {
        this.speedFactor = factor;
    }

}
