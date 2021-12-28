using UnityEngine;
using UnityEngine.AI;

/**
* Used when the character is not the leader but is following
* the active character instead
**/
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

        // If we are too far away then teleport nearer to the leader
        // prevents one getting lost
        if(targetDifference.magnitude > teleportDistance) {
            MoveTowardsTarget();
            TeleportNearTarget();
        } else {
            MoveTowardsTarget();
        }

        // Play footstep audio
        float horizontalVelocity = Mathf.Sqrt(Mathf.Pow(agent.velocity.x, 2) + Mathf.Pow(agent.velocity.z, 2));

        if(horizontalVelocity > 0.2f) {
            audioSource.pitch = Mathf.Min(Mathf.Max(0.2f, horizontalVelocity / 11), 1.1f);
            if(!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }

        // Enable the walking animation
        animator.SetFloat("speed", horizontalVelocity);
    }

    private void TeleportNearTarget() {
        // For the teleportation try a few areas around the leader to see if we can teleport
        Vector3[] directions = new Vector3[]{ Vector3.back, Vector3.left, Vector3.right };

        foreach(Vector3 direction in directions) {
            // Teleport next to the leader
            Vector3 teleportTarget = target.transform.position - 4 * direction + 2 * Vector3.up;
            Collider[] hits = Physics.OverlapSphere(teleportTarget, 1);
            bool safe = true;

            // Check the location is safe i.e. nothing else is occupying the spot
            foreach(Collider c in hits) {
                if(c.name != "Terrain" && c.name != "Player" && c.name != "Pipe" && c.name != "Floor Smooth 1" && c.name != "Floor Smooth 2") {
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
        // Move using the NavMeshAgent, smoothly face the leader
        Vector3 targetPosition = target.transform.position;
        agent.SetDestination(targetPosition);
        Quaternion lookAt = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.fixedDeltaTime);
    }

}
