using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrustedCutScene : CutScene {

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
        return gameManager.GetComponent<ObjectiveManager>().HasObjective("returnToCamp");
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

            dialogueManager.QueueDialogue("npc_name", "cs_trusted_npc_1", 5, voice: "npc");
            dialogueManager.QueueDialogue("speaker_you", "cs_trusted_you_1", 3, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_trusted_npc_2", 7, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_trusted_npc_3", 5, voice: "player");
            dialogueManager.QueueDialogue("speaker_you", "cs_trusted_you_2", 5, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_trusted_npc_4", 5, voice: "player");
            dialogueManager.QueueDialogue("speaker_you", "cs_trusted_you_3", 3, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_trusted_npc_5", 7, voice: "player");
            dialogueManager.QueueDialogue("npc_name", "cs_trusted_npc_6", 7, voice: "player");
        }, 47);

        QueueAction(() => {
            gameManager.GetComponent<TutorialManager>().QueueTutorial("tut_vendor_title", "tut_vendor_text", 12);
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("returnToCamp");
            gameManager.GetComponent<MapSectionManager>().EnableMountainPathSection();
            gameManager.GetComponent<ObjectiveManager>().AddObjective("findSeeds", "obj_climb_mountain", new ObjectiveManager.RewardEntry(1000, 25));

            cutSceneCamera.SetActive(false);
            
            // Move them back
            player.transform.position = playerReturnPosition;
            player.transform.rotation = playerReturnRotation;

            npc.transform.position = npcReturnPosition;
            npc.transform.rotation = npcReturnRotation;

            gameManager.GetComponent<CharacterManager>().SetFrozen(false);
            playerCamera.SetActive(true);
        }, 0);
    }

}
