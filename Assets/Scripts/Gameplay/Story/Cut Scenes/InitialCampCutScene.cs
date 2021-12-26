using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCampCutScene : CutScene {

    public GameObject npc;
    public GameObject cutSceneCamera;

    private Vector3 npcReturnPosition;
    private Quaternion npcReturnRotation;
    private Vector3 npcCutScenePosition = new Vector3(637.21f, 13.86f, 426.35f);
    private Vector3 npcCutSceneRotationEulers = new Vector3(5.402f, -87.44f, 4.361f);

    private Vector3 playerReturnPosition;
    private Quaternion playerReturnRotation;
    private Vector3 playerCutScenePosition = new Vector3(624.66f, 12.25f, 426.70f);
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

            dialogueManager.QueueDialogue("npc_name", "cs_camp_npc_1", 5, voice: "npc");
            dialogueManager.QueueDialogue("speaker_you", "cs_camp_you_1", 5, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_camp_npc_2", 7, voice: "npc");
            dialogueManager.QueueDialogue("speaker_you", "cs_camp_you_2", 5, voice: "player");
            dialogueManager.QueueDialogue("speaker_you", "cs_camp_you_3", 5, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_camp_npc_3", 5, voice: "npc");
            dialogueManager.QueueDialogue("speaker_you", "cs_camp_you_4", 3, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_camp_npc_4", 5, voice: "npc");
            dialogueManager.QueueDialogue("npc_name", "cs_camp_npc_5", 5, voice: "npc");
        }, 45);

        QueueAction(() => {
            gameManager.GetComponent<PlayerInventory>().AddItemToInventory("fullHealthPotion", 1);
            gameManager.GetComponent<PlayerInventory>().AddItemToInventory("wornBow", 1);
            gameManager.GetComponent<TutorialManager>().QueueTutorial("tut_inventory_title", "tut_inventory_text", 15);
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("findInitialCamp");
            gameManager.GetComponent<ObjectiveManager>().AddObjective("findFirstOrcCave", "obj_find_orc", new ObjectiveManager.RewardEntry(300, 10));

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
