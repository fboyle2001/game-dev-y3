using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerWithFollow : MonoBehaviour
{
    public bool movementEnabled = true;
    public float horizontalForce = 5f;
    public float rotateSpeed = 90f;
    public GameObject followTarget;
    public float sprintFactor = 2f;
    public float followDistance = 10f;

    private Rigidbody controllerRb;
    private Animator controllerAnimator;

    void Start() {
        controllerRb = GetComponent<Rigidbody>();
        controllerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        float sprint = 1;
        
        if(Input.GetKey(KeyCode.LeftShift)) {
            sprint = sprintFactor;
            controllerAnimator.SetFloat("Speed_f", 0.5f);
        } else {
            if(Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")) > 0.02) {
                controllerAnimator.SetFloat("Speed_f", 0.25f);
            } else {
                controllerAnimator.SetFloat("Speed_f", 0f);
            }
        }

        if(!movementEnabled) {
            Vector3 goalPosition = followTarget.transform.position - followTarget.transform.TransformDirection(Vector3.back);
            Vector3 steering = Vector3.zero;

            if((goalPosition - transform.position).magnitude > followDistance) {
                steering = (goalPosition - transform.position).normalized * horizontalForce * Time.fixedDeltaTime * sprint;
            }

            transform.position += steering;
            transform.LookAt(followTarget.transform);
        } else {
            Vector3 movement = transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * Time.fixedDeltaTime * horizontalForce * sprint;
            movement += transform.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal") * Time.fixedDeltaTime * horizontalForce * sprint;
            controllerRb.MovePosition(transform.position + movement);
        }
    }
}
