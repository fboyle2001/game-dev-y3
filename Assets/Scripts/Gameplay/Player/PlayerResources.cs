using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{

    private static readonly float xpPowerScalar = Mathf.Log(2) / 5;
    private static readonly float xpScalar = 10;
    
    private List<System.Action<PlayerResources, float, int>> resourceUpdateListeners = new List<System.Action<PlayerResources, float, int>>();
    private List<System.Action<int>> levelUpListeners = new List<System.Action<int>>();

    private int gold = 0;
    private int currentExperienceLevel = 1;
    private int xpForNextLevel;
    private float xp = 0;

    private GameObject gameManager;
    private PlayerStats stats;

    void OnEnable() {
        xpForNextLevel = CalculateXPForLevel(currentExperienceLevel);
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        stats = gameManager.GetComponent<PlayerStats>();
        PropagateResourceEvent(0, 0);
    }

    private int CalculateXPForLevel(int level) {
        // Makes it exponentially harder to level up
        // Current constants means level 5 needs 2 times the xp, 10 needs 4 times, 15 needs 8 times etc (relative to 0 --> 1 xp req).
        // Capped at 999 per level at around level 33/34 and beyond
        return Mathf.CeilToInt(Mathf.Clamp(xpScalar * Mathf.Exp(xpPowerScalar * (level - 1)), 0, 999));
    }

    public void RegisterResourceUpdateListener(System.Action<PlayerResources, float, int> listener) {
        resourceUpdateListeners.Add(listener);
        PropagateResourceEvent(0, 0);
    }
    public void RegisterLevelUpListener(System.Action<int> listener) {
        levelUpListeners.Add(listener);
    }

    private void PropagateResourceEvent(float xpChange, int goldChange) {
        resourceUpdateListeners.ForEach(action => action(this, xpChange, goldChange));
    }

    private void PropagateLevelUpEvent(int newLevel) {
        levelUpListeners.ForEach(action => action(newLevel));
    }

    public void AddExperience(float amount) {
        this.xp += amount; 

        while(xp >= xpForNextLevel) {
            xp -= xpForNextLevel;
            GrantLevelUpReward(currentExperienceLevel);
            currentExperienceLevel++;
            xpForNextLevel = CalculateXPForLevel(currentExperienceLevel);
            PropagateLevelUpEvent(currentExperienceLevel);
        }

        PropagateResourceEvent(amount, 0);
    }

    public bool HasEnoughGold(int requiredAmount) {
        return gold > requiredAmount;
    }

    public void AddGold(int amount) {
        this.gold = Mathf.Max(this.gold + amount, 0);
        PropagateResourceEvent(0, amount);
    }

    public int GetGold() {
        return gold;
    }

    public int GetXPForNextLevel() {
        return xpForNextLevel;
    }

    public float GetXP() {
        return xp;
    }
    
    public int GetCurrentExperienceLevel() {
        return currentExperienceLevel;
    }

    private void GrantLevelUpReward(int levelReached) {
        // Rewards are pre-determined for the first 15 levels
        switch(levelReached) {
            case 1:
                stats.AddMaxHealthMultiplier(0.1f);
                break;
            case 2:
                stats.AddRegenPerSecond(0.2f);
                break;
            case 3:
                stats.AddMaxHealthMultiplier(0.1f);
                break;
            case 4:
                stats.AddMaxHealthMultiplier(0.1f);
                break;
            case 5:
                stats.AddMaxHealthMultiplier(0.1f);
                break;
            case 6:
                stats.AddMaxHealthMultiplier(0.1f);
                break;
            case 7:
                stats.AddArmour(5f);
                break;
            case 8:
                stats.AddDamageMultiplier(0.2f);
                break;
            case 9:
                stats.AddMaxHealthMultiplier(0.1f);
                break;
            case 10:
                stats.AddRegenPerSecond(0.2f);
                break;
            case 11:
                stats.AddMaxHealthMultiplier(0.15f);
                break;
            case 12:
                stats.AddArmour(5f);
                break;
            case 13:
                stats.AddMaxHealthMultiplier(0.2f);
                break;
            case 14:
                stats.AddDamageMultiplier(0.2f);
                break;
            case 15:
                stats.AddRegenPerSecond(0.2f);
                break;
            default:
                break;
        }
    }

}
