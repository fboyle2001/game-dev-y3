using UnityEngine;
using UnityEngine.InputSystem;

/**
* Handles player input and sends events to the correct character
* (i.e. the character that is active when the action is used)
* Uses the new Unity Input System to enable flawless transitioning
* between input devices
*
* UI cursor is handled by the ImprovedMouse class instead
**/
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
    private InputAction pauseAction;

    private void Awake() {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        uiManager = gameManager.GetComponent<UIManager>();
        playerInput = GetComponent<PlayerInput>();
        
        // These map to the actions available in PlayerControls.inputactions (namely in the Land action map)
        moveAction = playerInput.actions["Move"];
        toggleInventoryAction = playerInput.actions["Toggle Inventory"];
        sprintingAction = playerInput.actions["Sprinting"];
        lookAroundAction = playerInput.actions["Look Around"];
        attackAction = playerInput.actions["Attack"];
        interactAction = playerInput.actions["Interact"];
        jumpAction = playerInput.actions["Jump"];
        switchCharacterAction = playerInput.actions["Switch Character"];
        pauseAction = playerInput.actions["Pause"];

        // Get the primary's controller as they will be active at the start of the game
        currentController = primary.GetComponent<ICharacterActions>();
    }

    private void OnEnable() {
        // When the active changes we will want to change the currentController
        gameManager.GetComponent<CharacterManager>().RegisterActiveChangeListener(gameObject, OnActiveCharacterChange);

        // Register event handlers for each action the user can take
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCancelled;

        sprintingAction.performed += OnSprintingPerformed;
        sprintingAction.canceled += OnSprintingCancelled;

        toggleInventoryAction.performed += ToggleInventory;

        lookAroundAction.performed += StartLookAround;
        lookAroundAction.canceled += StopLookAround;

        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCancelled;

        interactAction.performed += OnInteractPerformed;

        jumpAction.performed += OnJumpPerformed;

        switchCharacterAction.started += OnSwitchPerformed;

        pauseAction.performed += OnPause;
    }

    private void OnDisable() {
        // Deregister the listener
        gameManager.GetComponent<CharacterManager>().DeregisterActiveChangeListener(gameObject);

        // Deregister event handlers
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCancelled;

        sprintingAction.performed -= OnSprintingPerformed;
        sprintingAction.canceled -= OnSprintingCancelled;
        
        toggleInventoryAction.performed -= ToggleInventory;
        
        lookAroundAction.performed -= StartLookAround;
        lookAroundAction.canceled -= StopLookAround;

        attackAction.performed -= OnAttackPerformed;
        attackAction.canceled -= OnAttackCancelled;

        interactAction.performed -= OnInteractPerformed;

        jumpAction.performed -= OnJumpPerformed;

        switchCharacterAction.started -= OnSwitchPerformed;

        pauseAction.performed -= OnPause;
    }

    private void OnActiveCharacterChange(GameObject active) {
        // When the active changes update the controller so that actions apply
        // to the correct character e.g. moving on the correct character
        this.currentController = active.GetComponent<ICharacterActions>();
        // End rumble if applicable
        InputSystem.ResetHaptics();
    }

    private void ToggleInventory(InputAction.CallbackContext context) {
        // Toggle the inventory only if no other UI is open and we aren't in a cut scene
        if(uiManager.shopPanel.activeSelf || uiManager.pausePanel.activeSelf || currentController.IsFrozen()) return;
        uiManager.inventoryPanel.SetActive(!uiManager.inventoryPanel.activeSelf);
    }

    private void OnMovePerformed(InputAction.CallbackContext context) {
        // Move the active character if the UI is closed
        if(uiManager.shopPanel.activeSelf || uiManager.pausePanel.activeSelf || uiManager.inventoryPanel.activeSelf) return;
        currentController.StartMovement(context.ReadValue<Vector2>());
    }

    private void OnMoveCancelled(InputAction.CallbackContext context) {
        // Stop applying force to the character
        currentController.StopMovement();
    }

    private void OnSprintingPerformed(InputAction.CallbackContext context) {
        // Changes the force being applied to the character
        currentController.StartSprinting();
    }

    private void OnSprintingCancelled(InputAction.CallbackContext context) {
        // Cancels the sprint action and returns to regular force application
        currentController.StopSprinting();
    }

    private void StartLookAround(InputAction.CallbackContext context) {
        // Move the camera around with the mouse if not in the UI
        if(uiManager.shopPanel.activeSelf || uiManager.pausePanel.activeSelf || uiManager.inventoryPanel.activeSelf) return;
        currentController.StartLookAround(context.ReadValue<Vector2>());
    }

    private void StopLookAround(InputAction.CallbackContext context) {
        // Stop the camera movement
        currentController.StopLookAround();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context) {
        // Trigger an attack if the UI is closed and they aren't frozen (e.g. not in a cut scene)
        if(uiManager.shopPanel.activeSelf || uiManager.inventoryPanel.activeSelf || uiManager.pausePanel.activeSelf || currentController.IsFrozen()) return;
        currentController.StartAttack();
    }

    private void OnAttackCancelled(InputAction.CallbackContext context) {
        // Stop attacking, this allows them to hold down the button rather than tapping repeatedly
        currentController.StopAttack();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context) {
        // Interact with any IInteractable if they are close enough
        // This is one case where behaviour differs between primary and secondary character 
        // only the primary can interact with IInteractables
        if(uiManager.inventoryPanel.activeSelf || uiManager.pausePanel.activeSelf || currentController.IsFrozen()) return;
        currentController.Interact();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context) {
        // Jump if UI is closed and not frozen
        if(uiManager.inventoryPanel.activeSelf || uiManager.shopPanel.activeSelf || uiManager.pausePanel.activeSelf || currentController.IsFrozen()) return;
        currentController.Jump();
    }

    private void OnSwitchPerformed(InputAction.CallbackContext context) {
        // Change the active character
        gameManager.GetComponent<CharacterManager>().SwapActive();
    }

    private void OnPause(InputAction.CallbackContext context) {
        // Open the pause menu, closes all other UI panels
        InputSystem.PauseHaptics();
        uiManager.inventoryPanel.SetActive(false);
        uiManager.shopPanel.SetActive(false);
        uiManager.pausePanel.SetActive(!uiManager.pausePanel.activeSelf);
    }

}
