using UnityEngine;

/**
* Applies user input actions to the secondary character
* e.g. movement, attacking, camera rotation
* Refer to PrimaryCharacterActions for more information about each method's implementation
**/
public class SecondaryCharacterActions : MonoBehaviour, ICharacterActions {
    
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

    void FixedUpdate() {
        // See PrimaryCharacterActions.FixedUpdate for details

        if(frozen) {
            rb.velocity = Vector3.zero;
            animator.SetFloat("speed", 0f);
            return;
        }

        bool grounded = Physics.Raycast(GetComponent<Collider>().transform.position, Vector3.down, 0.5f, ~(1 << 8));

        if(grounded && jump) {
            rb.AddForce(Vector3.up * 85, ForceMode.Impulse);
            grounded = false;
        }
        
        jump = false;

        Move();
        float horizontalVelocity = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2));

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

    void Update() {
        // See PrimaryCharacterActions.Update for details
        if(frozen) return;
        if(attacking) {
            weaponManager.FireWeapon();
        }
    }

    void LateUpdate() {
        // See PrimaryCharacterActions.LateUpdate for details
        if(frozen) return;
        LookAround();
    }

    private void Move() {
        // See PrimaryCharacterActions.Move for details
        float scalar = forceScalar;
        float maxVelocity = maxWalkVelocity;

        if(sprinting) {
            scalar *= sprintScalar;
            maxVelocity = maxSprintVelocity;
        }

        Vector3 force = transform.TransformVector(new Vector3(movementDirection.x, 0, movementDirection.y) * scalar);

        if(movementDirection.x != 0 || movementDirection.y != 0) {
            rb.AddForce(force);
        }

        float velocitySquareDiff = rb.velocity.sqrMagnitude - maxVelocity * maxVelocity;

        if(velocitySquareDiff > 0) {
            Vector3 resistance = transform.TransformVector(new Vector3(movementDirection.x, 0, movementDirection.y)) * scalar * -0.03f * velocitySquareDiff;
            rb.AddForce(resistance);
        }   
    }

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
        // See PrimaryCharacterActions.LookAround for details
        float right = lookDirection.x * GlobalSettings.horizontalMouseSensitivity * mouseSpeed;
        float up = -lookDirection.y * GlobalSettings.verticalMouseSensitivity * mouseSpeed;
        
        Vector3 vectorCameraRot = primaryCamera.transform.rotation.eulerAngles + new Vector3(up, 0, 0);
        Quaternion cameraRot = Quaternion.Slerp(primaryCamera.transform.rotation, Quaternion.Euler(vectorCameraRot), Time.deltaTime);
        Vector3 cameraEulerAngles = cameraRot.eulerAngles;
        
        cameraEulerAngles.x = Mathf.Clamp(cameraRot.eulerAngles.x, 0, 360);

        if(cameraEulerAngles.x > 60 && cameraEulerAngles.x < 150) {
            cameraEulerAngles.x = 60;
        } else if(cameraEulerAngles.x < 300 && cameraEulerAngles.x > 180) {
            cameraEulerAngles.x = 300;
        }

        primaryCamera.transform.rotation = Quaternion.Euler(cameraEulerAngles);

        Quaternion playerRot = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, right, 0)), Time.deltaTime);
        primaryCamera.transform.RotateAround(transform.position, Vector3.up, playerRot.eulerAngles.x);
        transform.rotation = playerRot;
    }

    public void StartLookAround(Vector2 direction) {
        if(frozen) return;
        this.lookDirection = direction;
    }

    public void StopLookAround() {
        this.lookDirection = new Vector2(0, 0);
    }

    public void StartAttack() {
        attacking = true && !frozen;
    }

    public void StopAttack() {
        attacking = false;
    }

    public void Interact() {
        // Secondary cannot interact so do nothing if they try
        return;
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
