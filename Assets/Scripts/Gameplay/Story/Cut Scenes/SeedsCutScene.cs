using UnityEngine;

/**
* Handles the cut scene when you find the seeds
**/
public class SeedsCutScene : CutScene {

    public GameObject cutSceneCamera;
    public GameObject seeds;

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
            // Disable the player and their camera
            gameManager.GetComponent<CharacterManager>().SetFrozen(true);
            playerCamera.SetActive(false);

            // Move the player and store their previous position
            playerReturnPosition = player.transform.position;
            playerReturnRotation = player.transform.rotation;

            player.transform.position = playerCutScenePosition;
            player.transform.rotation = Quaternion.Euler(playerCutSceneRotationEulers);
            
            cutSceneCamera.SetActive(true);

            dialogueManager.QueueDialogue("speaker_you", "cs_seeds_1", 5, voice: "player");
            dialogueManager.QueueDialogue("speaker_you", "cs_seeds_2", 5, voice: "player");
        }, 10);

        QueueAction(() => {
            gameManager.GetComponent<ObjectiveManager>().CompleteObjective("findSeeds");
            gameManager.GetComponent<ObjectiveManager>().AddObjective("returnToCamp2", "obj_return_to_camp", new ObjectiveManager.RewardEntry(3000, 100));

            cutSceneCamera.SetActive(false);
            
            // Move them back
            player.transform.position = playerReturnPosition;
            player.transform.rotation = playerReturnRotation;

            gameManager.GetComponent<CharacterManager>().SetFrozen(false);
            playerCamera.SetActive(true);
            seeds.SetActive(false);

            gameManager.GetComponent<CharacterManager>().primary.GetComponent<ShowInteractText>().RecheckInterability();
        }, 0);
    }

}
