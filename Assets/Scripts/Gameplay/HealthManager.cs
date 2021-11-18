using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public float maxHealth;
    public float health;

    public void AddHealth(float add) {
        health = Mathf.Clamp(health + add, 0, maxHealth);
    }
}
