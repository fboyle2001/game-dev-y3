using UnityEngine;

/**
* Applies user input actions to the primary character
* e.g. movement, attacking, camera rotation
**/
public class PrimaryCharacterActions : MonoBehaviour, ICharacterActions {

    public AudioClip jumpingClip;

    public float forceScalar = 1500;
    public float sprintScalar = 2;
    public float maxWalkVelocity = 10;
    public float maxSprintVelocity = 15;
    public GameObject primaryCamera;

    private float mouseSpeed = 20;
    private Rigidbody rb;
    private GameObject gameManager;
    private WeaponManager weaponManager;
    private Animator animator;
    private AudioSource audioSource;

    private bool sprinting = false;
    private Vector2 movementDirection = new Vector2(0, 0);
    private Vector2 lookDirection = new Vector2(0, 0);
    private bool attacking = false;
    private bool frozen = false;
    private bool jump = false;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        weaponManager = gameManager.GetComponent<WeaponManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Use FixedUpdate since I use RigidBody which utilises the physics engine
    void FixedUpdate() {
        // If we freeze the character stop animation and stop them in place
        if(frozen) {
            rb.velocity = Vector3.zero;
            animator.SetBool("jumping", false);
            animator.SetFloat("speed", 0f);
            return;
        }

        // Check if the player is on the ground, do not allow them to jump if they aren't 
        // ~(1 << 8) prevents the raycast from hitting the character's collider
        // they would never be able to jump otherwise
        bool grounded = Physics.Raycast(GetComponent<Collider>().transform.position, Vector3.down, 0.5f, ~(1 << 8));

        // If they are on the ground, not jumping and slowed down enough then stop the jump animation
        if(grounded && !jump && rb.velocity.y < 0.02f) {
            animator.SetBool("jumping", false);
        }

        // If they are grounded and want to jump then apply an impulse force upwards
        // Also plays the jump audio
        if(grounded && jump) {
            rb.AddForce(Vector3.up * 400, ForceMode.Impulse);
            grounded = false;
            animator.SetBool("jumping", true);
            AudioSource.PlayClipAtPoint(jumpingClip, transform.position);
        }
        
        // Reset jump so they ascend repeatedly
        jump = false;

        // Move the character
        Move();
        // Measure their non-vertical velocity to determine if we should play footsteps
        float horizontalVelocity = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2));

        if(horizontalVelocity > 0.2f) {
            // Play footsteps and scale the pitch (and thus speed) to their velocity / maxVelocity ratio
            audioSource.pitch = Mathf.Min(Mathf.Max(0.2f, horizontalVelocity / 9), 1.1f);
            if(!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }

        // Set the animator speed variable so we get animations once we move fast enough
        animator.SetFloat("speed", horizontalVelocity);
    }

    void Update() {
        if(frozen) return;

        // Fire the weapon in Update rather than FixedUpdate to be more responsive
        // this doesn't depend on the physics engine
        if(attacking) {
            weaponManager.FireWeapon();
        }
    }

    void LateUpdate() {
        if(frozen) return;
        // Move the camera around after everything else
        LookAround();
    }

    private void Move() {
        float scalar = forceScalar;
        float maxVelocity = maxWalkVelocity;

        // Sprinting changes the upper bound and amount of force applied
        if(sprinting) {
            scalar *= sprintScalar;
            maxVelocity = maxSprintVelocity;
        }

        // Applies a force in the forwards direction of the transform 
        Vector3 force = transform.TransformVector(new Vector3(movementDirection.x, 0, movementDirection.y) * scalar);

        // If any input is set then apply a force
        if(movementDirection.x != 0 || movementDirection.y != 0) {
            rb.AddForce(force);
        }

        // maxVelocity is a soft upper bound, they can exceed it but they are punished
        // by the resistance force that is applied
        float velocitySquareDiff = rb.velocity.sqrMagnitude - maxVelocity * maxVelocity;

        // Apply a resistance force proportional to velocity diff squared in the opposite direction
        // this gives a more natural max velocity feel
        if(velocitySquareDiff > 0) {
            Vector3 resistance = transform.TransformVector(new Vector3(movementDirection.x, 0, movementDirection.y)) * scalar * -0.03f * velocitySquareDiff;
            rb.AddForce(resistance);
        } 
    }

    // These just set variables that are used by Move to determine the movement force

    public void StartSprinting() {
        this.sprinting = true && !frozen;
    }

    public void StopSprinting() {
        this.sprinting = false;
    }

    public void StartMovement(Vector2 direction) {
        if(frozen) return;
        this.movementDirection = direction;
    }

    public void StopMovement() {
        this.movementDirection = new Vector2(0, 0);
    }

    private void LookAround() {
        // The character's camera is not fixed to the character, rather it orbits them
        // Calculate the amount to rotate or move the camera
        float right = lookDirection.x * GlobalSettings.horizontalMouseSensitivity * mouseSpeed;
        float up = -lookDirection.y * GlobalSettings.verticalMouseSensitivity * mouseSpeed; // Inverts if sens goes to high!
        
        // Looking up and down changes the camera rotation
        Vector3 vectorCameraRot = primaryCamera.transform.rotation.eulerAngles + new Vector3(up, 0, 0);
        // Smoothly move up and down
        Quaternion cameraRot = Quaternion.Slerp(primaryCamera.transform.rotation, Quaternion.Euler(vectorCameraRot), Time.deltaTime);
        // Convert back to a vector so we can clip the angles
        Vector3 cameraEulerAngles = cameraRot.eulerAngles;
        
        cameraEulerAngles.x = Mathf.Clamp(cameraRot.eulerAngles.x, 0, 360);

        // Prevents looking too far up and down
        if(cameraEulerAngles.x > 60 && cameraEulerAngles.x < 150) {
            cameraEulerAngles.x = 60;
        } else if(cameraEulerAngles.x < 300 && cameraEulerAngles.x > 180) {
            cameraEulerAngles.x = 300;
        }

        // Update the camera rotation
        primaryCamera.transform.rotation = Quaternion.Euler(cameraEulerAngles);

        // Looking left and right orbits the camera around the character
        // Slerp smooths this motion
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, right, 0)), Time.deltaTime);
        // Orbit around the character's transform
        primaryCamera.transform.RotateAround(transform.position, Vector3.up, playerRot.eulerAngles.x);
        transform.rotation = playerRot;
    }

    // These update the direction to move the camera in

    public void StartLookAround(Vector2 direction) {
        if(frozen) return;
        this.lookDirection = direction;
    }

    public void StopLookAround() {
        this.lookDirection = new Vector2(0, 0);
    }

    // Attack is called in the Update function

    public void StartAttack() {
        attacking = true && weaponManager.HasWeapon() && !frozen;
    }

    public void StopAttack() {
        attacking = false;
    }

    public void Interact() {
        if(frozen) return;
        // Interactions are registered when they are available
        // So we don't need to perform many checks as they are handled on registeration instead
        gameManager.GetComponent<InteractionManager>().ExecuteInteractions();
    }

    public void Jump() {
        jump = true;
    }

    public void SetFrozen(bool frozen) {
        this.frozen = frozen;
    }

    public bool IsFrozen() {
        return frozen;
    }

}
