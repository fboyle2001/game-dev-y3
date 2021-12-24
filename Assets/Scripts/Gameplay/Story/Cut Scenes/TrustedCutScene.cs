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

            dialogueManager.QueueDialogue("NPC1", "Wow! I can't believe it. The prophecy is true...", 3);
            dialogueManager.QueueDialogue("You", "The prophecy?", 3);
            dialogueManager.QueueDialogue("NPC1", "Years ago the elders mentioned this day would come. " + 
            "The Orcs arrived in our territory 432 years ago and have wrecked havoc on our lands and settlements ever since. " +
            "It has long been dreamt that an assassin cat would come and rescue us from these wretched creatures.", 3);
            dialogueManager.QueueDialogue("You", "Well I don't know what to say. Could it just be coincidence? I have no idea how I got here...", 3);
            dialogueManager.QueueDialogue("NPC1", "The only way to tell is to see if you can retrieve the Seeds of Life.", 3);
            dialogueManager.QueueDialogue("You", "Where would they be?", 3);
            dialogueManager.QueueDialogue("NPC1", "Go past the Orc cave to the river... you'll see where to go", 3);
            dialogueManager.QueueDialogue("NPC1", "If you need anything before your journey come and have a look at my shop under the wood canopy", 3);
        }, 24);

        QueueAction(() => {
            gameManager.GetComponent<TutorialManager>().QueueTutorial("Vendor",
                "Get close to NPC1 and press [Interact] to visit their shop. You can only do this from the main character and not your cat. " + 
                "It's a good idea to purchase some items every time you are at the camp.", 10);
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("returnToCamp");
            gameManager.GetComponent<MapSectionManager>().EnableMountainPathSection();
            gameManager.GetComponent<ObjectiveManager>().AddObjective("findSeeds", "Climb the Mountain", "Climb the Mountain and find clues"
                , new ObjectiveManager.RewardEntry(1000, 25));

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
