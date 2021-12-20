using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    public GameObject cornerA;
    public GameObject cornerB;
    public GameObject home;

    private GameObject target;
    private GameObject gameManager;
    private bool active = false;
    private NavMeshAgent agent;

    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        agent = GetComponent<NavMeshAgent>();

        gameManager.GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);

        minX = Mathf.Min(cornerA.transform.position.x, cornerB.transform.position.x);
        maxX = Mathf.Max(cornerA.transform.position.x, cornerB.transform.position.x);
        minZ = Mathf.Min(cornerA.transform.position.z, cornerB.transform.position.z);
        maxZ = Mathf.Max(cornerA.transform.position.z, cornerB.transform.position.z);
    }

    void OnDisable() {
        gameManager.GetComponent<CharacterManager>().DeregisterActiveChangeListener(gameObject);
    }

    void FixedUpdate() {
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
    }

    public void SetActive(bool active) {
        this.active = active;

        if(active) {
            target = gameManager.GetComponent<CharacterManager>().GetActiveCharacter();
            Debug.Log("Active");
        }
    }

    private void OnActiveCharacterChange(GameObject newActive) {
        target = newActive;
    }
}
