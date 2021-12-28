using UnityEngine;
using UnityEngine.AI;

/**
* Utilises Nav Mesh Agents to path find towards the player while
* also managing the rotation of the enemy and playing audio
* and animation as they move
**/
public class AnimatedEnemyMovement : MonoBehaviour {

    public GameObject cornerA;
    public GameObject cornerB;
    public GameObject home;
    public string animatorSpeedName;
    public AudioClip walkingClip;
    public float timeBetweenWalkingClip;
    public float minVelocityForWalkingClip;

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
    private float timeSinceLastWalkPlayed;

    private GameObject currentAim;
    private AudioSource audioSource;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // These values form a bounding rectangle on the area that the enemy 
        // seek the target within
        minX = Mathf.Min(cornerA.transform.position.x, cornerB.transform.position.x);
        maxX = Mathf.Max(cornerA.transform.position.x, cornerB.transform.position.x);
        minZ = Mathf.Min(cornerA.transform.position.z, cornerB.transform.position.z);
        maxZ = Mathf.Max(cornerA.transform.position.z, cornerB.transform.position.z);
    }

    void FixedUpdate() {
        // Want to slow down as we approach the target so scale velocity by speedFactor
        agent.velocity = agent.velocity * speedFactor;
        // The animation controllers use the speed to determine the state of animation
        animator.SetFloat(animatorSpeedName, agent.velocity.magnitude);

        if(!active) return;
        
        // Moves the game object towards the target
        MoveTowardsTarget();

        timeSinceLastWalkPlayed += Time.fixedDeltaTime;

        // Don't want to spam play the audio so play it every timeSinceLastWalkPlayed seconds 
        // if they are moving fast enough
        if(timeSinceLastWalkPlayed >= timeBetweenWalkingClip && agent.velocity.magnitude > minVelocityForWalkingClip) {
            timeSinceLastWalkPlayed = 0;
            audioSource.PlayOneShot(walkingClip, 0.4f);
        }
    }

    private void MoveTowardsTarget() {
        Vector3 targetPosition = target.transform.position;

        // If the target is outside the bounding rectangle return to the
        // home position instead
        if(targetPosition.x > maxX || targetPosition.x < minX) {
            agent.SetDestination(home.transform.position);
            currentAim = home;
            return;
        }

        if(targetPosition.z > maxZ || targetPosition.z < minZ) {
            agent.SetDestination(home.transform.position);
            currentAim = home;
            return;
        }

        // Path find to the target position while smoothly rotating to face the position
        currentAim = target;
        agent.SetDestination(targetPosition);
        Quaternion lookAt = Quaternion.LookRotation(targetPosition - transform.position);
        // Slerp handles the smoothing of the rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, 10 * Time.fixedDeltaTime);
    }

    public void SetActive(bool active) {
        this.active = active;
        agent.enabled = active;
    }

    public GameObject GetCurrentTarget() {
        return currentAim;
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }

    public void SetSpeedFactor(float factor) {
        this.speedFactor = factor;
    }

}
