using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    
    public GameObject tutorialPanel;
    public TMP_Text tutorialTitle;
    public TMP_Text tutorialMessage;

    void Awake() {

    }

    struct TutorialEntry {
        public readonly string title;
        public readonly string message;
        public readonly float durationInSeconds;

        public TutorialEntry(string title, string message, float durationInSeconds) {
            this.title = title;
            this.message = message;
            this.durationInSeconds = durationInSeconds;
        }
    }

    private Queue<TutorialEntry> tutorialQueue;
    private bool activeTutorial;

    void OnEnable() {
        ClearTutorial();
    }

    public void QueueTutorial(string title, string message, float durationInSeconds) {
        tutorialQueue.Enqueue(new TutorialEntry(title, message, durationInSeconds));

        if(!activeTutorial) {
            DisplayTutorial();
        }
    }

    public void ClearTutorial() {
        activeTutorial = false;
        tutorialQueue = new Queue<TutorialEntry>();
        tutorialPanel.SetActive(false);
        tutorialTitle.text = "";
        tutorialMessage.text = "";
    }

    void DisplayTutorial(string titleKey, string messageKey) {
        tutorialPanel.SetActive(true);
        tutorialTitle.text = GetComponent<LocaleManager>().GetString(titleKey);
        tutorialMessage.text = GetComponent<LocaleManager>().GetString(messageKey);
    }

    void DisplayTutorial() {
        if(tutorialQueue.Count == 0) {
            ClearTutorial();
            return;
        }
        
        activeTutorial = true;

        TutorialEntry nextEntry = tutorialQueue.Dequeue();

        // Display the text
        DisplayTutorial(nextEntry.title, nextEntry.message);

        // Display the next text after the delay 
        Invoke("DisplayTutorial", nextEntry.durationInSeconds);
    }

}
