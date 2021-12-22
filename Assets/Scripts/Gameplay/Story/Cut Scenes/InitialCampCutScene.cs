using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCampCutScene : CutScene {

    public GameObject npc;
    public GameObject cutSceneCamera;

    private Vector3 npcReturnPosition;
    private Quaternion npcReturnRotation;
    private Vector3 npcCutScenePosition = new Vector3(643.44f, 14.39f, 422.25f);
    private Vector3 npcCutSceneRotationEulers = new Vector3(2.978f, -78.3f, 6.369f);

    private Vector3 playerReturnPosition;
    private Quaternion playerReturnRotation;
    private Vector3 playerCutScenePosition = new Vector3(624.66f, 12.586f, 426.70f);
    private Vector3 playerCutSceneRotationEulers = new Vector3(0f, -245.3f, -4.519f);

    new void OnEnable() {
        base.OnEnable();
        npcReturnPosition = npc.transform.position;
        npcReturnRotation = npc.transform.rotation;
    }

    public override bool IsCutSceneActivatable() {
        return gameManager.GetComponent<ObjectiveManager>().HasObjective("findInitialCamp");
    }

    protected override void QueueActions() {
        DialogueManager dialogueManager = gameManager.GetComponent<DialogueManager>();
        GameObject player = gameManager.GetComponent<CharacterManager>().GetActiveCharacter();
        GameObject playerCamera = gameManager.GetComponent<CharacterManager>().GetActiveCamera();

        QueueAction(() => {
            Debug.Log("Cut scene starting");

            // Disable the player and their camera
            gameManager.GetComponent<CharacterManager>().SetFrozen(true);
            playerCamera.SetActive(false);

            // Move the player and store their previous position
            playerReturnPosition = player.transform.position;
            playerReturnRotation = player.transform.rotation;

            player.transform.position = playerCutScenePosition;
            player.transform.rotation = Quaternion.Euler(playerCutSceneRotationEulers);

            // Move the NPC and player in to position
            npc.transform.position = npcCutScenePosition;
            npc.transform.rotation = Quaternion.Euler(npcCutSceneRotationEulers);
            
            cutSceneCamera.SetActive(true);

            dialogueManager.QueueDialogue("You", "Hello? Is anybody there?", 3);
            dialogueManager.QueueDialogue("NPC1", "Stay out of here! The others are on their way back you'd better get running... wait who are you?", 3);
            dialogueManager.QueueDialogue("You", "I'm <CHARACTER NAME> I don't know how I got here, where am I?", 3);
            dialogueManager.QueueDialogue("NPC1", "Hmmm I'm not sure I believe that, we're the furtherest camp from <THE TOWN>." + 
                "How did you end up so far lost? Did the Orcs put you up to this?", 3);
            dialogueManager.QueueDialogue("You", "The Orcs? Last I remember was falling down that mountain over there <FOR WHAT REASON>.", 3);
            dialogueManager.QueueDialogue("You", "NPC1 you haven't seen a small cat running about anywhere have you? I swear they were with me...", 3);
            dialogueManager.QueueDialogue("NPC1", "Wait you're part of the Assassin Cats group? You must have taken some fall to not know where you are...", 3);
            dialogueManager.QueueDialogue("You", "What do you mean?", 3);
            dialogueManager.QueueDialogue("NPC1", "I think I might know where your cat is but I don't know whether to trust you..." + 
                "there is an Orc's cave just off the path you followed to get here, might be worth a look.", 3);
            dialogueManager.QueueDialogue("NPC1", "Take out the Orc and then come back here and I'll explain everything you want to know." + 
                "Here's some stuff to help you out, don't do anything stupid...", 3);
        }, 30);

        QueueAction(() => {
            gameManager.GetComponent<PlayerInventory>().AddItemToInventory("fullHealthPotion", 1);
            gameManager.GetComponent<PlayerInventory>().AddItemToInventory("rustedBow", 1);
            gameManager.GetComponent<TutorialManager>().QueueTutorial("Inventory",
                "Press [I] to open your inventory. On the left side you will find items that give you helpful bonuses throughout the game." + 
                " On the right side is your character, stats and equipped weaponry. NPC1 has gifted you a <WEAPON_NAME> and a rare full health potion." + 
                " Equip the weapon by clicking 'Equip Item' and drink the potion by clicking 'Use Item' - it will help a lot in the Orc cave!", 15);
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("findInitialCamp", 1);
            gameManager.GetComponent<ObjectiveManager>().AddObjective("findFirstOrcCave", "Find an Orc", "Find an Orc Cave and see if they have your cat"
                , new ObjectiveManager.RewardEntry(0, 0));

            cutSceneCamera.SetActive(false);
            
            // Move them back
            player.transform.position = playerReturnPosition;
            player.transform.rotation = playerReturnRotation;

            npc.transform.position = npcReturnPosition;
            npc.transform.rotation = npcReturnRotation;

            gameManager.GetComponent<CharacterManager>().SetFrozen(false);
            playerCamera.SetActive(true);

            Debug.Log("Cut scene over");
        }, 0);
    }

}
