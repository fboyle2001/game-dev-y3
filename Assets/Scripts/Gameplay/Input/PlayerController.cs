using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public GameObject inventoryCanvas;
    public GameObject primary;

    private ICharacterActions currentController;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction toggleInventoryAction;
    private InputAction sprintingAction;
    private InputAction lookAroundAction;
    private InputAction attackAction;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        
        moveAction = playerInput.actions["Move"];
        toggleInventoryAction = playerInput.actions["Toggle Inventory"];
        sprintingAction = playerInput.actions["Sprinting"];
        lookAroundAction = playerInput.actions["Look Around"];
        attackAction = playerInput.actions["Attack"];

        currentController = primary.GetComponent<ICharacterActions>();
    }

    private void OnEnable() {
        // For testing only
        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<DaylightManager>().SetLightIntensity(1);
        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<DaylightManager>().SetTimeFrozen(true);

        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCancelled;

        sprintingAction.performed += OnSprintingPerformed;
        sprintingAction.canceled += OnSprintingCancelled;

        toggleInventoryAction.performed += ToggleInventory;

        lookAroundAction.performed += StartLookAround;
        lookAroundAction.canceled += StopLookAround;

        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCancelled;
    }

    private void OnDisable() {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCancelled;

        sprintingAction.performed -= OnSprintingPerformed;
        sprintingAction.canceled -= OnSprintingCancelled;

        toggleInventoryAction.performed -= ToggleInventory;
        
        lookAroundAction.performed -= StartLookAround;
        lookAroundAction.canceled -= StopLookAround;

        attackAction.performed -= OnAttackPerformed;
        attackAction.canceled -= OnAttackCancelled;
    }

    private void ToggleInventory(InputAction.CallbackContext context) {
        // Doesn't work for controllers
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<PlayerInventory>().AddItemToInventory("regenPotion", 10);
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

}
