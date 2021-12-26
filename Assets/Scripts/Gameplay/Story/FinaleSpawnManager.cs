using UnityEngine;

public class FinaleSpawnManager : MonoBehaviour {

    public GameObject phaseOneParent;
    public GameObject phaseTwoParent;
    public GameObject phaseThreeParent;
    public GameObject treeParent;
    public GameObject tree;

    private int deadPhaseOne = 0;
    private int deadPhaseTwo = 0;
    private int deadPhaseThree = 0;

    private float maxScale = 1f;

    private GameObject gameManager;
    private DialogueManager dialogueManager;
    private float unscaledHalfTreeHeight;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        EnemyBase.RegisterGlobalDamageHandler(gameObject, OnDamageHandler);
    }

    void Start() {
        SpawnPhaseOne();
    }

    void Update() {
        if(tree.transform.localScale.y >= maxScale) {
            tree.GetComponent<AudioSource>().Stop();
            return;
        }

        float scaleFactor = Time.deltaTime;
        tree.transform.localScale += Vector3.one * scaleFactor;
        tree.GetComponent<AudioSource>().Play();
    }

    private void OnDamageHandler(EnemyBase enemy) {
        if(!enemy.GetStats().IsDead()) return;

        switch(enemy.identifier) {
            case "phase1":
                deadPhaseOne += 1;
                TrySpawnPhaseTwo();
                return;
            case "phase2":
                deadPhaseTwo += 1;
                TrySpawnPhaseThree();
                return;
            case "phase3":
                deadPhaseThree += 1;
                TryFinish();
                return;
            default:
                return;
        }
    }

    private void SpawnPhaseOne() {
        maxScale = 2;
        treeParent.SetActive(true);
        phaseOneParent.SetActive(true);
    }
    
    private void TrySpawnPhaseTwo() {
        if(deadPhaseOne < 6) return;
        dialogueManager.QueueDialogue("speaker_you", "fnl_phase_2_1", 5);
        Invoke("SpawnPhaseTwo", 5);
    }

    private void SpawnPhaseTwo() {
        tree.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        maxScale = 8;
        dialogueManager.QueueDialogue("speaker_you", "fnl_phase_2_2", 3);
        phaseTwoParent.SetActive(true);
    }

    private void TrySpawnPhaseThree() {
        if(deadPhaseTwo < 5) return;
        dialogueManager.QueueDialogue("speaker_you", "fnl_phase_3_1", 5);
        Invoke("SpawnPhaseThree", 5);
    }

    private void SpawnPhaseThree() {
        tree.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        maxScale = 16;
        dialogueManager.QueueDialogue("speaker_ancient", "fnl_ancient_speak", 5);
        dialogueManager.QueueDialogue("speaker_you", "fnl_phase_3_2", 3);
        phaseThreeParent.SetActive(true);
    }

    private void TryFinish() {
        if(deadPhaseThree < 4) return;
        tree.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        maxScale = 24;
        dialogueManager.QueueDialogue("speaker_you", "fnl_celebrate", 8);
        Invoke("EndGame", 5f);
    }

    private void EndGame() {
        Debug.Log("Now end the game!");
    }

}