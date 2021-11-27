using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationBehaviour : MonoBehaviour
{
    void Start() {
        Application.targetFrameRate = 144;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

}
