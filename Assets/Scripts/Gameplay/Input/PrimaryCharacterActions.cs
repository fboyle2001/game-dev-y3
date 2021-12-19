using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryCharacterActions : MonoBehaviour, ICharacterActions {
    
    public float forceScalar = 1500;
    public float sprintScalar = 2;
    public float maxWalkVelocity = 10;
    public float maxSprintVelocity = 15;
    public float mouseSensitivityX = 5;
    public float mouseSensitivityY = 5;
    public GameObject primaryCamera;
    private Rigidbody rb;

    private bool sprinting = false;
    private Vector2 movementDirection = new Vector2(0, 0);
    private Vector2 lookDirection = new Vector2(0, 0);

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        Move();
    }

    void Update() {
    }

    void LateUpdate() {
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
        this.sprinting = true;
    }

    public void StopSprinting() {
        this.sprinting = false;
    }

    public void StartMovement(Vector2 direction) {
        this.movementDirection = direction;
    }

    public void StopMovement() {
        this.movementDirection = new Vector2(0, 0);
    }

    private void LookAround() {
        float right = lookDirection.x * mouseSensitivityX;
        float up = -lookDirection.y * mouseSensitivityY;

        // Debug.Log("Mouse Up: " + up + ", Mouse Right: " + right);
        
        Quaternion cameraRot = Quaternion.Slerp(primaryCamera.transform.rotation, Quaternion.Euler(primaryCamera.transform.rotation.eulerAngles + new Vector3(up, 0, 0)), Time.deltaTime);
        primaryCamera.transform.rotation = cameraRot;

        Quaternion playerRot = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, right, 0)), Time.deltaTime);
        primaryCamera.transform.RotateAround(transform.position, Vector3.up, playerRot.eulerAngles.x);
        transform.rotation = playerRot;
        
    }

    public void StartLookAround(Vector2 direction) {
        this.lookDirection = direction;
    }

    public void StopLookAround() {
        this.lookDirection = new Vector2(0, 0);;
    }

}
