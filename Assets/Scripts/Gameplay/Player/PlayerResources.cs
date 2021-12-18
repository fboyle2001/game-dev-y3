using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{

    private static readonly float xpPowerScalar = Mathf.Log(2) / 5;
    private static readonly float xpScalar = 10;
    
    private List<System.Action<PlayerResources>> resourceUpdateListeners = new List<System.Action<PlayerResources>>();

    private int gold = 10;
    private int currentExperienceLevel = 0;
    private int xpForNextLevel;
    private float xp = 0;

    private GameObject gameManager;
    private CharacterStats stats;

    void OnEnable() {
        xpForNextLevel = CalculateXPForLevel(currentExperienceLevel);
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        stats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        PropagateResourceEvent();
    }

    private int CalculateXPForLevel(int level) {
        // Makes it exponentially harder to level up
        // Current constants means level 5 needs 2 times the xp, 10 needs 4 times, 15 needs 8 times etc (relative to 0 --> 1 xp req).
        // Capped at 999 per level at around level 33/34 and beyond
        return Mathf.CeilToInt(Mathf.Clamp(xpScalar * Mathf.Exp(xpPowerScalar * level), 0, 999));
    }

    public void RegisterResourceUpdateListener(System.Action<PlayerResources> listener) {
        resourceUpdateListeners.Add(listener);
        PropagateResourceEvent();
    }

    private void PropagateResourceEvent() {
        resourceUpdateListeners.ForEach(action => action(this));
    }

    public void AddExperience(float amount) {
        this.xp += amount; 

        while(xp >= xpForNextLevel) {
            currentExperienceLevel++;
            xp -= xpForNextLevel;
            xpForNextLevel = CalculateXPForLevel(currentExperienceLevel);
            GrantLevelUpReward(currentExperienceLevel);
        }

        PropagateResourceEvent();
    }

    public bool HasEnoughGold(int requiredAmount) {
        return gold > requiredAmount;
    }

    public void UpdateGold(int amount) {
        this.gold = Mathf.Max(this.gold + amount, 0);
        PropagateResourceEvent();
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
        // Rewards are pre-determined for the first 15 levels and then randomised
        switch(levelReached) {
            case 1:
                // Increase their max health
                stats.AddMaxHealth(10f);
                break;
            case 2:
                // Give them some regen
                stats.AddRegenPerSecond(0.2f);
                break;
            case 3:
                stats.AddMaxHealth(10f);
                break;
            case 4:
                stats.AddMaxHealth(10f);
                break;
            case 5:
                stats.AddMaxHealth(10f);
                break;
            case 6:
                stats.AddMaxHealth(10f);
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            default:
                break;
        }
    }

}
