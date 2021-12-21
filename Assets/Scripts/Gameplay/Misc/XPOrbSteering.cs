using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrbSteering : MonoBehaviour {


    private GameObject gameManager;
    private GameObject target;
    private Rigidbody rb;

    private float xpValue = 1;
    private float brakingRadius = 10;
    private float absorbRadius = 5;
    private float maxVelocity = 8;

    public void SetXpValue(float value) {
        xpValue = Mathf.Max(value, 1);
        Debug.Log("Set xpValue to " + xpValue);
    }

    void OnEnable() {
        rb = GetComponent<Rigidbody>();

        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        target = gameManager.GetComponent<CharacterManager>().GetActiveCharacter();
    }

    void FixedUpdate() {
        // Calculate steering so we get smooth seeking paths
        Vector3 direction = target.transform.position - transform.position;

        if(direction.magnitude < absorbRadius) {
            AbsorbXpOrb();
            return;
        }

        Vector3 newVelocity = rb.velocity + direction.normalized * maxVelocity * Time.fixedDeltaTime;
        
        // If distance >= 10 then do not scale
        // If distance < 10 then scale according to exp(distance * ln(2) / brakingRadius) - 1 to get smooth braking
        // If braking factor < 0.2 keep it at 0.2
        float braking = Mathf.Max(0.2f, Mathf.Min(1, Mathf.Exp(Mathf.Log(2) * direction.magnitude / brakingRadius) - 1));

        // If velocity is greater than max * braking 
        if(newVelocity.magnitude > maxVelocity * braking) {
            newVelocity = newVelocity.normalized * maxVelocity * braking;
        }

        // Steering direction
        // Debug.DrawRay(transform.position, newVelocity, Color.red, Time.fixedDeltaTime); 
        
        // Update velocity
        rb.velocity = newVelocity;
    }

    private void AbsorbXpOrb() {
        gameManager.GetComponent<PlayerResources>().AddExperience(xpValue);
        gameObject.SetActive(false);
    }

}
