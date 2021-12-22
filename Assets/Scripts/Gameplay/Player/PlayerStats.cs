using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    private List<System.Action<PlayerStats, float, float, float, float>> statChangeListeners = new List<System.Action<PlayerStats, float, float, float, float>>();

    private float armour = 0;
    private float regen = 0;
    private float damageMultiplier = 1;
    private float maxHealthMultiplier = 1;

    public void RegisterStatChangeListener(System.Action<PlayerStats, float, float, float, float> action) {
        statChangeListeners.Add(action);
        PropagateStatChangeEvent(0, 0, 0, 0);
    }

    private void PropagateStatChangeEvent(float maxHealthChange, float dmgChange, float regenChange, float armourChange) {
        statChangeListeners.ForEach(action => action(this, maxHealthChange, dmgChange, regenChange, armourChange));
    }

    public void AddMaxHealthMultiplier(float amount) {
        this.maxHealthMultiplier += amount;
        PropagateStatChangeEvent(amount, 0, 0, 0);
    }
    
    public float GetMaxHealthMultiplier() {
        return maxHealthMultiplier;
    }

    public void AddDamageMultiplier(float amount) {
        this.damageMultiplier += amount;
        PropagateStatChangeEvent(0, amount, 0, 0);
    }

    public float GetDamageMultiplier() {
        return damageMultiplier;
    }
    
    public void AddRegenPerSecond(float regen) {
        this.regen += regen;
        PropagateStatChangeEvent(0, 0, regen, 0);
    }

    public float GetRegenPerSecond() {
        return regen;
    }

    public void AddArmour(float armour) {
        this.armour += armour;
        PropagateStatChangeEvent(0, 0, 0, armour);
    }
    public float GetArmour() {
        return armour;
    }

}
