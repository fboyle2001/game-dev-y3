using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCharacterActions : MonoBehaviour, ICharacterActions {
    
    public float forceScalar = 1500;
    public float sprintScalar = 2;
    public float maxWalkVelocity = 10;
    public float maxSprintVelocity = 15;
    public float mouseSensitivityX = 5;
    public float mouseSensitivityY = 5;
    public GameObject primaryCamera;

    private Rigidbody rb;
    private GameObject gameManager;
    private WeaponManager weaponManager;
    private Animator animator;

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
    }

    void FixedUpdate() {
        if(frozen) {
            rb.velocity = Vector3.zero;
            animator.SetFloat("speed", 0f);
            return;
        }

        bool grounded = Physics.Raycast(GetComponent<Collider>().transform.position, Vector3.down, 0.5f, ~(1 << 8));

        if(grounded && jump) {
            rb.AddForce(Vector3.up * 50, ForceMode.Impulse);
            grounded = false;
        }
        
        jump = false;

        Move();
        animator.SetFloat("speed", Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2)));
    }

    void Update() {
        if(frozen) return;
        if(attacking) {
            weaponManager.FireWeapon();
        }
    }

    void LateUpdate() {
        if(frozen) return;
        LookAround();
    }

    private void Move() {
        float scalar = forceScalar;
        float maxVelocity = maxWalkVelocity;

        if(sprinting) {
            scalar *= sprintScalar;
            maxVelocity = maxSprintVelocity;
        }

        Vector3 force = transform.TransformVector(new Vector3(movementDirection.x, 0, movementDirection.y) * scalar);
        // bool moving = false;
        // bool resisted = false;

        if(movementDirection.x != 0 || movementDirection.y != 0) {
            rb.AddForce(force);
            // moving = true;
        }

        float velocitySquareDiff = rb.velocity.sqrMagnitude - maxVelocity * maxVelocity;

        if(velocitySquareDiff > 0) {
            Vector3 resistance = transform.TransformVector(new Vector3(movementDirection.x, 0, movementDirection.y)) * scalar * -0.03f * velocitySquareDiff;
            rb.AddForce(resistance);
            // resisted = true;
        }

        // Debug.Log("Applied Force: " + force);
        // Debug.Log("Velocity: " + rb.velocity + " m/s using applied force of " + force.magnitude + " N (sprinting: " + sprinting + ", moved: " + moving + ", resisted: " + resisted + ")");   
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
        float right = lookDirection.x * mouseSensitivityX;
        float up = -lookDirection.y * mouseSensitivityY;

        // Debug.Log("Mouse Up: " + up + ", Mouse Right: " + right);
        
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
