using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    private List<System.Action<PlayerStats>> statChangeListeners = new List<System.Action<PlayerStats>>();

    private float armour = 0;
    private float regen = 0;
    private float damageMultiplier = 1;
    private float maxHealthMultiplier = 1;

    public void RegisterStatChangeListener(System.Action<PlayerStats> action) {
        statChangeListeners.Add(action);
        PropagateStatChangeEvent();
    }

    private void PropagateStatChangeEvent() {
        statChangeListeners.ForEach(action => action(this));
    }

    public void AddMaxHealthMultiplier(float amount) {
        this.maxHealthMultiplier += amount;
        PropagateStatChangeEvent();
    }
    
    public float GetMaxHealthMultiplier() {
        return maxHealthMultiplier;
    }

    public void AddDamageMultiplier(float amount) {
        this.damageMultiplier += amount;
        PropagateStatChangeEvent();
    }

    public float GetDamageMultiplier() {
        return damageMultiplier;
    }
    
    public void AddRegenPerSecond(float regen) {
        this.regen += regen;
        PropagateStatChangeEvent();
    }

    public float GetRegenPerSecond() {
        return regen;
    }

    public void AddArmour(float armour) {
        this.armour += armour;
        PropagateStatChangeEvent();
    }
    public float GetArmour() {
        return armour;
    }

}
