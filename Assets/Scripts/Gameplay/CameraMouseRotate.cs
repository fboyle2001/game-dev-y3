using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseRotate : MonoBehaviour
{
    public float horizontalSensitivity = 20f;
    public float verticalSensitivity = 20f;
    public float yClamp = 60f;

    private float currentYRotation = 0;
    private float currentXRotation = 0;

    void OnEnable() {
        currentXRotation = transform.parent.eulerAngles[1];
        // currentYRotation = 0;
        // currentYRotation = transform.parent.eulerAngles[0];
    }

    void LateUpdate() {
        float horizontalInputSpeed = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1)  * horizontalSensitivity;
        currentXRotation += horizontalInputSpeed;
        currentXRotation = Mathf.Repeat(currentXRotation, 360);

        Vector3 horizontalRotate = Vector3.up * horizontalInputSpeed;
        transform.parent.Rotate(horizontalRotate);

        float verticalInputSpeed = -Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1)  * verticalSensitivity;
        currentYRotation += verticalInputSpeed;
        currentYRotation = Mathf.Clamp(currentYRotation, -yClamp, yClamp);
        transform.rotation = Quaternion.Euler(currentYRotation, currentXRotation, 0);
    }
}
