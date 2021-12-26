using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public AudioClip npcVoice;
    public AudioClip playerVoice;
    public GameObject dialoguePanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;

    private AudioSource audioSource;

    struct DialogueEntry {
        public readonly string speakerKey;
        public readonly string textKey;
        public readonly float durationInSeconds;
        public readonly System.Action startCallback;
        public readonly string voice;

        public DialogueEntry(string speakerKey, string textKey, float durationInSeconds, System.Action startCallback, string voice) {
            this.speakerKey = speakerKey;
            this.textKey = textKey;
            this.durationInSeconds = durationInSeconds;
            this.startCallback = startCallback;
            this.voice = voice;
        }
    }

    private Queue<DialogueEntry> dialogueQueue;
    private bool activeDialogue;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() {
        ClearDialogue();
    }

    public void QueueDialogue(string speakerKey, string textKey, float durationInSeconds, System.Action startCallback = null, string voice = null) {
        dialogueQueue.Enqueue(new DialogueEntry(speakerKey, textKey, durationInSeconds, startCallback, voice));

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
        audioSource.Stop();
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

        audioSource.Stop();

        // Display the text and play the audio
        if(nextEntry.voice == "player") {
            audioSource.clip = playerVoice;
            audioSource.loop = true;
            audioSource.time = playerVoice.length * Random.Range(0f, 1f);
            audioSource.Play();    
        } else if (nextEntry.voice == "npc") {
            audioSource.clip = npcVoice;
            audioSource.loop = true;
            audioSource.time = npcVoice.length * Random.Range(0f, 1f);    
            audioSource.Play();        
        }

        DisplayDialogue(nextEntry.speakerKey, nextEntry.textKey);

        // Display the next text after the delay 
        Invoke("DisplayDialogue", nextEntry.durationInSeconds);
    }

}
