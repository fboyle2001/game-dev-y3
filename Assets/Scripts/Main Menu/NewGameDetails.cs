using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameDetails : MonoBehaviour {
    
    public GameObject mainPanel;
    public TMP_InputField primaryNameInput;
    public TMP_InputField secondaryNameInput;
    public TMP_Text difficultyText;
    public Button startButton;

    private LocaleManager localeManager;
    private string primaryName;
    private string secondaryName;
    private int difficulty;

    void Awake() {
        localeManager = GameObject.FindGameObjectWithTag("Locale").GetComponent<LocaleManager>();
        primaryName = null;
        secondaryName = null;
        difficulty = 1;
    }

    void OnEnable() {
        UpdateDifficultyText();
        CheckCanStart();
    }

    public void Back() {
        mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPrimaryNameChange() {
        primaryName = primaryNameInput.text;
        CheckCanStart();
    }

    public void OnSecondaryNameChange() {
        secondaryName = secondaryNameInput.text;
        CheckCanStart();
    }

    public void OnDifficultyChange(int option) {
        difficulty = option;
        UpdateDifficultyText();
        CheckCanStart();
    }

    public void StartGame() {
        GlobalSettings.difficulty = difficulty;
        GlobalSettings.primaryName = primaryName;
        GlobalSettings.secondaryName = secondaryName;
        SceneManager.LoadScene("PrimaryGameScene");
    }

    private void CheckCanStart() {
        bool canStart = true;

        if(primaryName == null || primaryName == "") {
            canStart = false;
        }

        if(secondaryName == null || secondaryName == "") {
            canStart = false;
        }

        if(difficulty == -1) {
            canStart = false;
        }

        startButton.interactable = canStart;
    }

    private void UpdateDifficultyText() {
        string localeKey = "";

        switch(difficulty) {
            case 0:
                localeKey = "mm_diff_desc_easy";
                break;
            case 1:
                localeKey = "mm_diff_desc_normal";
                break;
            case 2:
                localeKey = "mm_diff_desc_hard";
                break;
            case 3:
                localeKey = "mm_diff_desc_impossible";
                break;
            default:
                return;
        }

        difficultyText.text = localeManager.GetString(localeKey);
    }

}
