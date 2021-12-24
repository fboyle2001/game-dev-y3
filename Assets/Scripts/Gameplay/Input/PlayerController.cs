using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public GameObject primary;

    private ICharacterActions currentController;
    private GameObject gameManager;
    private UIManager uiManager;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction toggleInventoryAction;
    private InputAction sprintingAction;
    private InputAction lookAroundAction;
    private InputAction attackAction;
    private InputAction interactAction;
    private InputAction jumpAction;
    private InputAction switchCharacterAction;
    private InputAction closeUIAction;

    private void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        uiManager = gameManager.GetComponent<UIManager>();
        playerInput = GetComponent<PlayerInput>();
        
        moveAction = playerInput.actions["Move"];
        toggleInventoryAction = playerInput.actions["Toggle Inventory"];
        sprintingAction = playerInput.actions["Sprinting"];
        lookAroundAction = playerInput.actions["Look Around"];
        attackAction = playerInput.actions["Attack"];
        interactAction = playerInput.actions["Interact"];
        jumpAction = playerInput.actions["Jump"];
        switchCharacterAction = playerInput.actions["Switch Character"];
        closeUIAction = playerInput.actions["Close UI"];

        currentController = primary.GetComponent<ICharacterActions>();
    }

    private void OnEnable() {
        gameManager.GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);

        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCancelled;

        sprintingAction.performed += OnSprintingPerformed;
        sprintingAction.canceled += OnSprintingCancelled;

        toggleInventoryAction.started += ToggleInventory;

        lookAroundAction.performed += StartLookAround;
        lookAroundAction.canceled += StopLookAround;

        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCancelled;

        interactAction.started += OnInteractPerformed;

        jumpAction.performed += OnJumpPerformed;

        switchCharacterAction.started += OnSwitchPerformed;

        closeUIAction.started += OnCloseAllUI;
    }

    private void OnDisable() {
        gameManager.GetComponent<CharacterManager>().DeregisterActiveChangeListener(gameObject);

        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCancelled;

        sprintingAction.performed -= OnSprintingPerformed;
        sprintingAction.canceled -= OnSprintingCancelled;
        
        toggleInventoryAction.started -= ToggleInventory;
        
        lookAroundAction.performed -= StartLookAround;
        lookAroundAction.canceled -= StopLookAround;

        attackAction.performed -= OnAttackPerformed;
        attackAction.canceled -= OnAttackCancelled;

        interactAction.started -= OnInteractPerformed;

        jumpAction.performed -= OnJumpPerformed;

        switchCharacterAction.started -= OnSwitchPerformed;

        closeUIAction.started -= OnCloseAllUI;
    }

    private void OnActiveCharacterChange(GameObject active) {
        this.currentController = active.GetComponent<ICharacterActions>();
    }

    private void ToggleInventory(InputAction.CallbackContext context) {
        if(uiManager.shopPanel.activeSelf || currentController.IsFrozen()) return;
        uiManager.inventoryPanel.SetActive(!uiManager.inventoryPanel.activeSelf);
        Cursor.visible = !Cursor.visible;
    }

    private void OnMovePerformed(InputAction.CallbackContext context) {
        currentController.StartMovement(context.ReadValue<Vector2>());
    }

    private void OnMoveCancelled(InputAction.CallbackContext context) {
        currentController.StopMovement();
    }

    private void OnSprintingPerformed(InputAction.CallbackContext context) {
        currentController.StartSprinting();
    }

    private void OnSprintingCancelled(InputAction.CallbackContext context) {
        currentController.StopSprinting();
    }

    private void StartLookAround(InputAction.CallbackContext context) {
        if(uiManager.shopPanel.activeSelf || uiManager.inventoryPanel.activeSelf) return;
        currentController.StartLookAround(context.ReadValue<Vector2>());
    }

    private void StopLookAround(InputAction.CallbackContext context) {
        currentController.StopLookAround();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context) {
        currentController.StartAttack();
    }

    private void OnAttackCancelled(InputAction.CallbackContext context) {
        currentController.StopAttack();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context) {
        if(uiManager.inventoryPanel.activeSelf || currentController.IsFrozen()) return;
        currentController.Interact();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context) {
        currentController.Jump();
    }

    private void OnSwitchPerformed(InputAction.CallbackContext context) {
        gameManager.GetComponent<CharacterManager>().SwapActive();
    }

    private void OnCloseAllUI(InputAction.CallbackContext context) {
        uiManager.inventoryPanel.SetActive(false);
        uiManager.shopPanel.SetActive(false);
        Cursor.visible = false;
    }

}
