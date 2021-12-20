using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponManager : MonoBehaviour
{

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
    private CharacterStats primaryStats;

    private float timeBetweenShots = 0;
    private float timeSinceLastShot = 0;

    void OnEnable() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        primaryStats = gameManager.GetComponent<CharacterManager>().primary.GetComponent<CharacterStats>();
        GetComponent<PlayerInventory>().RegisterEquipUpdateListener(inventory => this.OnEquipmentChange(inventory));
        maxWidth = (chargeBackgroundBar.transform as RectTransform).sizeDelta.x;
        weaponPanel.SetActive(false);
        crosshair.SetActive(false);
    }

    void Update() {
        timeSinceLastShot = Mathf.Clamp(timeSinceLastShot + Time.deltaTime, 0, timeBetweenShots);
        UpdateUI();
    }

    private void OnEquipmentChange(PlayerInventory inventory) {
        if(inventory.GetCurrentWeapon() == null) return;

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
        if(!HasWeapon() || timeSinceLastShot < timeBetweenShots || !gameManager.GetComponent<CharacterManager>().IsPrimaryActive()) return;

        RaycastHit hit;
        bool didHit = Physics.Raycast(fireSource.transform.position, primaryCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~(1 << 8));
        
        if(didHit && hit.collider != null) {
            EnemyStats enemyStats = hit.collider.gameObject.GetComponent<EnemyStats>();

            if(enemyStats != null) {
                enemyStats.Damage(equippedWeapon.damagePerRound * primaryStats.GetDamageMultiplier());
            }
            
            Debug.DrawRay(fireSource.transform.position, primaryCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red, 5.0f);
        } else {
            Debug.DrawRay(fireSource.transform.position, primaryCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.blue, 5.0f);
        }

        this.timeSinceLastShot = 0;
    }

}
