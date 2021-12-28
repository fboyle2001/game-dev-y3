using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
* Handles firing and charging of weapon
**/
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
    public AudioSource secondaryWeaponAudioSource;

    [Header("Crosshair Feedback")]
    public Sprite hitCrosshairSprite;
    public Sprite missCrosshairSprite;
    public float revertTime = 0.8f;

    private WeaponInventoryItem equippedWeapon;
    private float maxWidth;
    private PlayerStats playerStats;
    private CharacterManager characterManager;
    private Sprite defaultCrosshairSprite;
    private AudioSource weaponAudioSource;

    private float timeBetweenShots = 0;
    private float timeSinceLastShot = 0;

    void Awake() {
        playerStats = GetComponent<PlayerStats>();
        characterManager = GetComponent<CharacterManager>();
        maxWidth = (chargeBackgroundBar.transform as RectTransform).sizeDelta.x;
        defaultCrosshairSprite = crosshair.GetComponent<Image>().sprite;
        weaponAudioSource = fireSource.GetComponent<AudioSource>();
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
            // Override for when playing as the cat
            // They use the claw item regardless of the inventory setting
            equippedWeapon = PlayerInventory.registeredItems["claws"] as WeaponInventoryItem;
            weaponPanel.SetActive(true);
            crosshair.SetActive(false);
            weaponImage.GetComponent<Image>().sprite = equippedWeapon.itemImage;
            weaponName.GetComponent<TMP_Text>().text = equippedWeapon.GetItemName(GetComponent<LocaleManager>());
            this.timeBetweenShots = 60 / equippedWeapon.roundsPerMinute;
            this.timeSinceLastShot = 0;
        }
    }

    private void OnEquipmentChange(PlayerInventory inventory) {
        // Update the fire rate and weapon UI
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
            weaponName.GetComponent<TMP_Text>().text = equippedWeapon.GetItemName(GetComponent<LocaleManager>());
            timeBetweenShots = 60 / equippedWeapon.roundsPerMinute;
        }

        this.timeSinceLastShot = 0;
    }

    private void UpdateUI() {
        // Updates the charging UI of the weapon 
        float chargeBarWidth = Mathf.Clamp(maxWidth * timeSinceLastShot / timeBetweenShots, 0, maxWidth);
        (chargeBar.transform as RectTransform).sizeDelta = new Vector2(chargeBarWidth, (chargeBar.transform as RectTransform).sizeDelta.y);

        if(chargeBarWidth >= maxWidth) {
            chargeText.GetComponent<TMP_Text>().text = GetComponent<LocaleManager>().GetString("ui_weapon_charged");
        } else {
            chargeText.GetComponent<TMP_Text>().text = GetComponent<LocaleManager>().GetString("ui_weapon_charging");
        }
    }

    public bool HasWeapon() {
        return equippedWeapon != null;
    }

    public void FireWeapon() {
        if(!HasWeapon() || timeSinceLastShot < timeBetweenShots) return;

        List<EnemyStats> damagables = new List<EnemyStats>(); 

        if(equippedWeapon.itemIdentifier == "claws") {
            // Claws are area of effect damage, ignores players
            Collider[] hitColliders = Physics.OverlapSphere(characterManager.secondary.transform.position, 5, ~(1 << 8 | 1 << 2));

            foreach(Collider hitCollider in hitColliders)  {
                EnemyStats hitStats = hitCollider.GetComponent<EnemyStats>();

                if(hitStats != null) {
                    damagables.Add(hitStats);
                }
            }

            secondaryWeaponAudioSource.Stop();
            secondaryWeaponAudioSource.Play();
        } else {
            // Raycast in the direction of the camera, take the first hit. Ignores players
            RaycastHit hit;
            bool didHit = Physics.Raycast(fireSource.transform.position, primaryCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~(1 << 8 | 1 << 2));
            bool hitCrosshair = false;
            
            if(didHit && hit.collider != null) {
                EnemyStats enemyStats = hit.collider.gameObject.GetComponent<EnemyStats>();

                if(enemyStats != null) {
                    damagables.Add(enemyStats);
                    hitCrosshair = true;
                }
            } 

            ChangeCrosshair(hitCrosshair);
            weaponAudioSource.Stop();
            weaponAudioSource.Play();
        }

        // Apply damage to everything hit, scale by the player's damage stat
        damagables.ForEach(stats => stats.Damage(equippedWeapon.damagePerRound * playerStats.GetDamageMultiplier()));
        this.timeSinceLastShot = 0;
    }

    private void ChangeCrosshair(bool hit) {
        // Change the crosshair based on if we hit an enemy
        CancelInvoke("RevertCrosshair");
        crosshair.GetComponent<Image>().sprite = hit ? hitCrosshairSprite : missCrosshairSprite;
        Invoke("RevertCrosshair", revertTime);        
    }

    private void RevertCrosshair() {
        crosshair.GetComponent<Image>().sprite = defaultCrosshairSprite;
    }

}
