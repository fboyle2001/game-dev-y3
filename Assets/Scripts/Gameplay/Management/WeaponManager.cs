using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponManager : MonoBehaviour {

    public GameObject weaponPanel;
    public GameObject weaponName;
    public GameObject weaponImage;
    public GameObject chargeBar;
    public GameObject chargeBackgroundBar;
    public GameObject chargeText;
    public GameObject crosshair;
    public GameObject fireSource;
    public GameObject primaryCamera;

    private WeaponInventoryItem equippedWeapon;
    private float maxWidth;
    private GameObject gameManager;
    private PlayerStats playerStats;

    private float timeBetweenShots = 0;
    private float timeSinceLastShot = 0;

    void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        playerStats = gameManager.GetComponent<PlayerStats>();
        maxWidth = (chargeBackgroundBar.transform as RectTransform).sizeDelta.x;
    }

    void Start() {
        GetComponent<PlayerInventory>().RegisterEquipUpdateListener(inventory => this.OnEquipmentChange(inventory));
        weaponPanel.SetActive(false);
        crosshair.SetActive(false);
    }

    void Update() {
        timeSinceLastShot = Mathf.Clamp(timeSinceLastShot + Time.deltaTime, 0, timeBetweenShots);
        UpdateUI();
    }

    public void SwitchCharacterMode(bool primary) {
        if(primary) {
            OnEquipmentChange(GetComponent<PlayerInventory>());
        } else {
            equippedWeapon = PlayerInventory.registeredItems["claws"] as WeaponInventoryItem;
            weaponPanel.SetActive(true);
            crosshair.SetActive(false);
            weaponImage.GetComponent<Image>().sprite = null;
            weaponName.GetComponent<TMP_Text>().text = "Claws";
            this.timeBetweenShots = 60 / equippedWeapon.roundsPerMinute;
            this.timeSinceLastShot = 0;
        }
    }

    private void OnEquipmentChange(PlayerInventory inventory) {
        if(inventory.GetCurrentWeapon() == null) {
            weaponPanel.SetActive(false);
            crosshair.SetActive(false);
            equippedWeapon = null;
            return;
        }

        if(equippedWeapon == null) {
            equippedWeapon = inventory.GetCurrentWeapon();
        } else if (equippedWeapon.itemIdentifier != inventory.GetCurrentWeapon().itemIdentifier) {
            equippedWeapon = inventory.GetCurrentWeapon();
        } else {
            return;
        }

        if(equippedWeapon == null) {
            weaponPanel.SetActive(false);
            crosshair.SetActive(false);
        } else {
            weaponPanel.SetActive(true);
            crosshair.SetActive(true);
            weaponImage.GetComponent<Image>().sprite = equippedWeapon.itemImage;
            weaponName.GetComponent<TMP_Text>().text = equippedWeapon.itemName;
            timeBetweenShots = 60 / equippedWeapon.roundsPerMinute;
        }

        this.timeSinceLastShot = 0;
    }

    private void UpdateUI() {
        float chargeBarWidth = Mathf.Clamp(maxWidth * timeSinceLastShot / timeBetweenShots, 0, maxWidth);
        (chargeBar.transform as RectTransform).sizeDelta = new Vector2(chargeBarWidth, (chargeBar.transform as RectTransform).sizeDelta.y);

        if(chargeBarWidth >= maxWidth) {
            chargeText.GetComponent<TMP_Text>().text = "Charged!";
        } else {
            chargeText.GetComponent<TMP_Text>().text = "Charging...";
        }
    }

    public bool HasWeapon() {
        return equippedWeapon != null;
    }

    public void FireWeapon() {
        if(!HasWeapon() || timeSinceLastShot < timeBetweenShots) return;

        List<EnemyStats> damagables = new List<EnemyStats>(); 

        if(equippedWeapon.itemIdentifier == "claws") {
            Collider[] hitColliders = Physics.OverlapSphere(fireSource.transform.position, 7, ~(1 << 8));

            foreach(Collider hitCollider in hitColliders)  {
                EnemyStats hitStats = hitCollider.GetComponent<EnemyStats>();

                if(hitStats != null) {
                    Debug.Log(hitCollider.name);
                    damagables.Add(hitStats);
                }
            }
        } else {
            RaycastHit hit;
            bool didHit = Physics.Raycast(fireSource.transform.position, primaryCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~(1 << 8));
            
            if(didHit && hit.collider != null) {
                EnemyStats enemyStats = hit.collider.gameObject.GetComponent<EnemyStats>();

                if(enemyStats != null) {
                    damagables.Add(enemyStats);
                }
                
                Debug.DrawRay(fireSource.transform.position, primaryCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red, 5.0f);
            } else {
                Debug.DrawRay(fireSource.transform.position, primaryCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.blue, 5.0f);
            }
        }

        damagables.ForEach(stats => stats.Damage(equippedWeapon.damagePerRound * playerStats.GetDamageMultiplier()));
        this.timeSinceLastShot = 0;
    }

}
