using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyBase : MonoBehaviour {
    
    private static Dictionary<int, System.Action<EnemyBase>> globalDamageHandlers = new Dictionary<int, System.Action<EnemyBase>>();

    public static void RegisterGlobalDamageHandler(GameObject owner, System.Action<EnemyBase> action) {
        globalDamageHandlers.Add(owner.GetInstanceID(), action);
    }

    public GameObject nameText;
    public GameObject levelText;
    public GameObject enemyUI;
    public string enemyName;
    public string identifier;
    public int level;

    protected GameObject gameManager;
    protected GameObject xpOrbPrefab;

    protected Animator animator;
    protected AnimatedEnemyMovement movementController;
    protected EnemyStats stats;
    protected GameObject target;
    protected bool active;

    public void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");

        stats = GetComponent<EnemyStats>();
        movementController = GetComponent<AnimatedEnemyMovement>();
        animator = GetComponent<Animator>();

        active = false;
    }

    public void OnEnable() {
        xpOrbPrefab = gameManager.GetComponent<PrefabStorage>().xpOrb;
        gameManager.GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);
        stats.RegisterDamageListener(OnDamageHandler);
    }

    public void Start() {
        SetActive(true);
        nameText.GetComponent<TMP_Text>().text = gameManager.GetComponent<LocaleManager>().GetString(enemyName);
        levelText.GetComponent<TMP_Text>().text = gameManager.GetComponent<LocaleManager>().GetString("txt_enm_level") + " " + level;
    }

    public void SetActive(bool active) {
        this.active = active;
        target = gameManager.GetComponent<CharacterManager>().GetActiveCharacter();
        movementController.SetTarget(target);
        movementController.SetActive(active);
    }

    public EnemyStats GetStats() {
        return stats;
    }

    private void OnActiveCharacterChange(GameObject newActive) {
        target = newActive;
        movementController.SetTarget(newActive);
    }

    private void OnDamageHandler(EnemyStats stats, float damage) {
        if(stats.IsDead()) {
            OnDeath();
            gameManager.GetComponent<CharacterManager>().DeregisterActiveChangeListener(gameObject);
            SetActive(false);
            enemyUI.SetActive(false);
            animator.SetBool("is_dead", true);
        }

        foreach(System.Action<EnemyBase> action in globalDamageHandlers.Values) {
            action(this);
        }
    }

    protected void OnDeath() {
        GameObject xpOrb = Instantiate(xpOrbPrefab, transform.position + GetComponent<Collider>().bounds.extents.y * Vector3.up, xpOrbPrefab.transform.rotation);
        xpOrb.GetComponent<XPOrbSteering>().SetXpValue(stats.GetXPValue());
        gameManager.GetComponent<PlayerResources>().AddGold(stats.GetGoldValue());

        Invoke("ClearBody", 2);
    }

    private void ClearBody() {
        gameObject.SetActive(false);
    }

}
