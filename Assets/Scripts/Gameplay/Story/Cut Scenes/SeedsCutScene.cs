using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedsCutScene : CutScene {

    public GameObject cutSceneCamera;

    private Vector3 playerReturnPosition;
    private Quaternion playerReturnRotation;
    private Vector3 playerCutScenePosition = new Vector3(196.68f, 78.857f, 747.6f);
    private Vector3 playerCutSceneRotationEulers = new Vector3(0f, -55.37f, 0f);

    new void OnEnable() {
        base.OnEnable();
    }

    public override bool IsCutSceneActivatable() {
        return gameManager.GetComponent<ObjectiveManager>().HasObjective("findSeeds");
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
            
            cutSceneCamera.SetActive(true);

            dialogueManager.QueueDialogue("You", "Wow these must be the Seeds of Life!", 5);
            dialogueManager.QueueDialogue("You", "Hopefully this will help me get home", 5);
            dialogueManager.QueueDialogue("You", "I had better get these back to NPC1!", 5);
        }, 15);

        QueueAction(() => {
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("findSeeds");
            gameManager.GetComponent<ObjectiveManager>().AddObjective("returnToCamp2", "Return to Camp", "Return to the camp"
                , new ObjectiveManager.RewardEntry(3000, 100));

            cutSceneCamera.SetActive(false);
            
            // Move them back
            player.transform.position = playerReturnPosition;
            player.transform.rotation = playerReturnRotation;

            gameManager.GetComponent<CharacterManager>().SetFrozen(false);
            playerCamera.SetActive(true);

            Debug.Log("Cut scene over");
        }, 0);
    }

}
