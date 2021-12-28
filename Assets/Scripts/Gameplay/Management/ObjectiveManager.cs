using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
* Handles all objectives in the story and grants rewards to the player
**/
public class ObjectiveManager : MonoBehaviour
{

    public GameObject objectivePanel;
    public TMP_Text objectiveTextList;
    private List<string> completedObjectives = new List<string>();
    private PlayerResources playerResources;

    // Encapsulates the rewards granted to the user for completing an objective
    public struct RewardEntry {
        public readonly int gold;
        public readonly float xp;

        public RewardEntry(int gold, float xp) {
            this.gold = gold;
            this.xp = xp;
        }
    }

    // Encapsulates an objective
    struct ObjectiveEntry {
        public readonly string identifier;
        public readonly string titleKey;
        public readonly RewardEntry reward;

        public ObjectiveEntry(string identifier, string titleKey, RewardEntry reward) {
            this.identifier = identifier;
            this.titleKey = titleKey;
            this.reward = reward;
        }
        
    }

    private Dictionary<string, ObjectiveEntry> objectives;

    void Awake() {
        playerResources = GetComponent<PlayerResources>();
    }

    void OnEnable() {
        ClearObjectives();
        HideObjectives();
    }

    public void HideObjectives() {
        objectivePanel.SetActive(false);
    }

    public void ShowObjectives() {
        objectivePanel.SetActive(true);
    }

    public bool HasAtLeastOneObjective(string[] potentialObjectivesIds) {
        // Checks if the player has at least one of the objectives in the array
        foreach (string objectiveId in potentialObjectivesIds) {
            if(this.HasObjective(objectiveId)) {
                return true;
            }
        }

        return false;
    }

    public bool HasObjective(string objectiveId) {
        return objectives.ContainsKey(objectiveId);
    }

    void ClearObjectives() {
        objectives = new Dictionary<string, ObjectiveEntry>();
    }

    void UpdateObjectiveDisplay() {
        // UI appears in the top right
        string text = "";

        foreach (ObjectiveEntry entry in objectives.Values) {
            if(text.Length != 0) {
                text += "\n";
            }

            text += GetComponent<LocaleManager>().GetString(entry.titleKey) + "\n";
        }

        objectiveTextList.text = text;
    }

    public void AddObjective(string identifier, string title, RewardEntry reward) {
        objectives.Add(identifier, new ObjectiveEntry(identifier, title, reward));
        UpdateObjectiveDisplay();
    }

    public void CompleteObjective(string idenitifer, float rewardScalar = 1) {
        // Mark the objective complete and grant the rewards
        ObjectiveEntry objective;
        bool found = objectives.TryGetValue(idenitifer, out objective);

        if(!found) {
            return;
        }

        objectives.Remove(idenitifer);
        completedObjectives.Add(idenitifer);
        UpdateObjectiveDisplay();

        RewardEntry reward = objective.reward;

        // Scale rewards according to difficulty
        playerResources.AddExperience(reward.xp * rewardScalar * GlobalSettings.GetRewardScalar());
        playerResources.AddGold(Mathf.RoundToInt(reward.gold * rewardScalar * GlobalSettings.GetRewardScalar()));
    }

    public List<string> GetCompletedObjectives() {
        return completedObjectives;
    }

}
