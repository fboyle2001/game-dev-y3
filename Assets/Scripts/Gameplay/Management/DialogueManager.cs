using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public GameObject dialoguePanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;

    struct DialogueEntry {
        public readonly string speakerKey;
        public readonly string textKey;
        public readonly float durationInSeconds;
        public readonly Action startCallback;

        public DialogueEntry(string speakerKey, string textKey, float durationInSeconds, Action startCallback) {
            this.speakerKey = speakerKey;
            this.textKey = textKey;
            this.durationInSeconds = durationInSeconds;
            this.startCallback = startCallback;
        }
    }

    private Queue<DialogueEntry> dialogueQueue;
    private bool activeDialogue;

    void OnEnable() {
        ClearDialogue();
    }

    public void QueueDialogue(string speakerKey, string textKey, float durationInSeconds, Action startCallback = null) {
        dialogueQueue.Enqueue(new DialogueEntry(speakerKey, textKey, durationInSeconds, startCallback));

        if(!activeDialogue) {
            DisplayDialogue();
        }
    }

    public void ClearDialogue() {
        activeDialogue = false;
        dialogueQueue = new Queue<DialogueEntry>();
        dialoguePanel.SetActive(false);
        speakerNameText.text = "";
        dialogueText.text = "";
    }

    void DisplayDialogue(string speakerKey, string textKey) {
        dialoguePanel.SetActive(true);
        speakerNameText.text = GetComponent<LocaleManager>().GetString(speakerKey);
        dialogueText.text = GetComponent<LocaleManager>().GetString(textKey);
    }

    void DisplayDialogue() {
        if(dialogueQueue.Count == 0) {
            ClearDialogue();
            return;
        }
        
        activeDialogue = true;

        DialogueEntry nextEntry = dialogueQueue.Dequeue();

        // Callback to sync game events with dialogue
        if(nextEntry.startCallback != null) {
            nextEntry.startCallback();
        }

        // Display the text
        DisplayDialogue(nextEntry.speakerKey, nextEntry.textKey);

        // Display the next text after the delay 
        Invoke("DisplayDialogue", nextEntry.durationInSeconds);
    }

}
