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

            dialogueManager.QueueDialogue("NPC1", "Run! They're coming! Wait... you found them...", 3);
            dialogueManager.QueueDialogue("You", "What's going on?", 3);
            dialogueManager.QueueDialogue("NPC1", "They've destroyed the camp, but you've found the seeds. You can end this torment.", 3);
            dialogueManager.QueueDialogue("You", "What are these? What do I need to do?", 3);
            dialogueManager.QueueDialogue("NPC1", "Go back to the Orc Cave I sent you to when we first met and plant the seeds in the cave.", 3);
            dialogueManager.QueueDialogue("NPC1", "You'll need to gear up first, I managed to bring some stuff with me.", 3);
            dialogueManager.QueueDialogue("NPC1", "Good luck...", 3);
        }, 21);

        QueueAction(() => {
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("returnToCamp2");
            // gameManager.GetComponent<MapSectionManager>().EnableMountainPathSection();
            gameManager.GetComponent<ObjectiveManager>().AddObjective("plantSeeds", "Plant the Seeds of Life by the Orc Cave", "Summon and defeat the ancient orc"
                , new ObjectiveManager.RewardEntry(7500, 150));

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

            Debug.Log("Cut scene over");
        }, 0);
    }

}
