using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectCutScene : CutScene {

    public GameObject npc;
    public GameObject cutSceneCamera;

    private Vector3 npcReturnPosition = new Vector3(651.77f, 15.04f, 629.11f);
    private Quaternion npcReturnRotation = Quaternion.Euler(new Vector3(2.964f, -137.6f, -1.56f));
    private Vector3 npcCutScenePosition = new Vector3(651.77f, 15.04f, 629.11f);
    private Vector3 npcCutSceneRotationEulers = new Vector3(2.964f, -137.6f, -1.56f);

    private Vector3 playerReturnPosition;
    private Quaternion playerReturnRotation;
    private Vector3 playerCutScenePosition = new Vector3(644.66f, 14.92f, 621.34f);
    private Vector3 playerCutSceneRotationEulers = new Vector3(0f, -314.7f, 0f);

    new void OnEnable() {
        base.OnEnable();
    }

    public override bool IsCutSceneActivatable() {
        return gameManager.GetComponent<ObjectiveManager>().HasObjective("returnToCamp2");
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

            dialogueManager.QueueDialogue("npc_name", "cs_redirect_npc_1", 5);
            dialogueManager.QueueDialogue("speaker_you", "cs_redirect_you_1", 3);
            dialogueManager.QueueDialogue("npc_name", "cs_redirect_npc_2", 7);
            dialogueManager.QueueDialogue("speaker_you", "cs_redirect_you_2", 5);
            dialogueManager.QueueDialogue("npc_name", "cs_redirect_npc_3", 5);
            dialogueManager.QueueDialogue("npc_name", "cs_redirect_npc_4", 5);
            dialogueManager.QueueDialogue("npc_name", "cs_redirect_npc_5", 3);
        }, 33);

        QueueAction(() => {
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("returnToCamp2");
            gameManager.GetComponent<ObjectiveManager>().AddObjective("plantSeeds", "obj_plant_seeds", new ObjectiveManager.RewardEntry(7500, 150));

            cutSceneCamera.SetActive(false);
            
            // Move them back
            player.transform.position = playerReturnPosition;
            player.transform.rotation = playerReturnRotation;

            npc.transform.position = npcReturnPosition;
            npc.transform.rotation = npcReturnRotation;

            gameManager.GetComponent<CharacterManager>().SetFrozen(false);
            playerCamera.SetActive(true);

            gameManager.GetComponent<MapSectionManager>().DisableOriginalOrcCave();
            gameManager.GetComponent<MapSectionManager>().EnableFinalOrcCave();
        }, 0);
    }

}
