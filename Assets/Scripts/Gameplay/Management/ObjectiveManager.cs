using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{

    public GameObject objectivePanel;
    public TMP_Text objectiveTextList;

    public struct RewardEntry {
        public readonly int gold;
        public readonly float xp;

        public RewardEntry(int gold, float xp) {
            this.gold = gold;
            this.xp = xp;
        }
    }

    struct ObjectiveEntry {
        public readonly string identifier;
        public readonly string title;
        public readonly string description;
        public readonly RewardEntry reward;

        public ObjectiveEntry(string identifier, string title, string description, RewardEntry reward) {
            this.identifier = identifier;
            this.title = title;
            this.description = description;
            this.reward = reward;
        }
        
    }

    private Dictionary<string, ObjectiveEntry> objectives;

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
        string text = "";

        foreach (ObjectiveEntry entry in objectives.Values) {
            if(text.Length != 0) {
                text += "\n";
            }

            text += "□ " + entry.title + "\n";
        }

        objectiveTextList.text = text;
    }

    public void AddObjective(string identifier, string title, string description, RewardEntry reward) {
        objectives.Add(identifier, new ObjectiveEntry(identifier, title, description, reward));
        UpdateObjectiveDisplay();
    }

    public void CompleteObjective(string idenitifer, float rewardScalar) {
        ObjectiveEntry objective;
        bool found = objectives.TryGetValue(idenitifer, out objective);

        if(!found) {
            return;
        }

        objectives.Remove(idenitifer);
        UpdateObjectiveDisplay();

        // TODO: Give rewards
        RewardEntry reward = objective.reward;
    }

}
