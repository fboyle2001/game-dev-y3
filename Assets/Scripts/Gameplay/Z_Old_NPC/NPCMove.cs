using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
Adapted from lectures
**/
public class NPCMove : MonoBehaviour
{
    
    public GameObject target;
    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        updateDest();
    }

    void Update() {
        updateDest();
    }

    private void updateDest() {
        agent.SetDestination(target.transform.position);
        // Debug.Log(target.transform.position);
    }
}
