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
        public readonly string speaker;
        public readonly string text;
        public readonly float durationInSeconds;
        public readonly Action startCallback;

        public DialogueEntry(string speaker, string text, float durationInSeconds, Action startCallback) {
            this.speaker = speaker;
            this.text = text;
            this.durationInSeconds = durationInSeconds;
            this.startCallback = startCallback;
        }
    }

    private Queue<DialogueEntry> dialogueQueue;
    private bool activeDialogue;

    void OnEnable() {
        ClearDialogue();
    }

    public void QueueDialogue(string speaker, string text, float durationInSeconds, Action startCallback = null) {
        dialogueQueue.Enqueue(new DialogueEntry(speaker, text, durationInSeconds, startCallback));

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

    void DisplayDialogue(string speaker, string text) {
        dialoguePanel.SetActive(true);
        speakerNameText.text = speaker;
        dialogueText.text = text;
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
        DisplayDialogue(nextEntry.speaker, nextEntry.text);

        // Display the next text after the delay 
        Invoke("DisplayDialogue", nextEntry.durationInSeconds);
    }

}
