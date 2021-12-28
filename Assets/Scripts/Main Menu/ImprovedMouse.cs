using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/**
* Virtual cursor that uses a virtual mouse to interact with the UI
* works seamlessly with both the real mouse and controller joysticks
* Supports multiple virtual cursors in the same scene as well
* Works off of a separate action map defined in PlayerControls.inputactions
**/
public class ImprovedMouse : MonoBehaviour {
    
    public InputActionProperty moveProp;
    public InputActionProperty clickProp;
    public InputActionProperty scrollProp;

    public RectTransform cursorTransform;
    public Canvas canvas;

    private Mouse virtualMouse;

    private Vector2 direction = Vector2.zero;
    private float mouseSpeed = 100;

    private Vector2 scrollDirection = Vector2.zero;
    private float scrollSpeed = 40;

    void Awake() {
        // Create a virtual mouse device
        virtualMouse = InputSystem.AddDevice("VirtualMouse") as Mouse;
        // Disable it immediately as we only want it if we are enabled
        InputSystem.DisableDevice(virtualMouse);
    }

    void OnEnable() {
        // Enable the virtual mouse device
        if(!virtualMouse.enabled) {
            InputSystem.EnableDevice(virtualMouse);
        }

        // Register event listeners for actions from the user
        moveProp.action.performed += UpdateCursorDirection;
        moveProp.action.canceled += ZeroCursorDirection;
        moveProp.action.Enable();

        clickProp.action.performed += Click;
        clickProp.action.Enable();

        scrollProp.action.performed += UpdateScrollDirection;
        scrollProp.action.canceled += ZeroScrollDirection;
        scrollProp.action.Enable();
    
        // Update the virtual mouse position to match the cursor image
        InputState.Change(virtualMouse.position, cursorTransform.anchoredPosition);
        // Update the positions after update
        InputSystem.onAfterUpdate += UpdateCursor;
    }

    void OnDisable() {
        // De-register event listeners
        moveProp.action.performed -= UpdateCursorDirection;
        moveProp.action.canceled -= ZeroCursorDirection;
        moveProp.action.Disable();

        clickProp.action.performed -= Click;
        clickProp.action.Disable();

        scrollProp.action.performed -= UpdateScrollDirection;
        scrollProp.action.canceled -= ZeroScrollDirection;
        scrollProp.action.Disable();
        
        InputSystem.onAfterUpdate -= UpdateCursor;

        // Disable the virtual mouse device
        InputSystem.DisableDevice(virtualMouse);
    }

    void OnDestroy() {
        // Remove the virtual mouse when the game closes
        InputSystem.RemoveDevice(virtualMouse);
    }

    // Same concepts as PlayerController and ICharacterActions

    private void ZeroScrollDirection(InputAction.CallbackContext ctx) {
        scrollDirection = Vector2.zero;
    }

    private void UpdateScrollDirection(InputAction.CallbackContext ctx) {
        scrollDirection = ctx.ReadValue<Vector2>();
    }

    private void ZeroCursorDirection(InputAction.CallbackContext ctx) {
        direction = Vector2.zero;
    }

    private void UpdateCursorDirection(InputAction.CallbackContext ctx) {
        direction = ctx.ReadValue<Vector2>();
    }

    private void UpdateCursor() {
        // Move the virtual cursor based on the physical input

        if(direction != Vector2.zero) {
            Vector2 currentPosition = virtualMouse.position.ReadValue();
            // Use unscaled delta time so that it works in the pause menu too
            float deltaX = direction.x * mouseSpeed * Time.unscaledDeltaTime * GlobalSettings.horizontalMouseSensitivity;
            float deltaY = direction.y * mouseSpeed * Time.unscaledDeltaTime * GlobalSettings.verticalMouseSensitivity;

            Vector2 delta = new Vector2(deltaX, deltaY);
            Vector2 newPosition = currentPosition + delta;

            // Keep it in the canvas
            newPosition.x = Mathf.Clamp(newPosition.x, canvas.pixelRect.xMin, canvas.pixelRect.xMax);
            newPosition.y = Mathf.Clamp(newPosition.y, canvas.pixelRect.yMin, canvas.pixelRect.yMax);

            // Update the position and delta of the virtual mouse
            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, delta);

            // Update the cursor image position
            cursorTransform.anchoredPosition = newPosition;
        }

        // Scrolling - only used for the difficulty dropdown

        if(scrollDirection != Vector2.zero) {
            // Simply update the virutal mouse scroll wheel
            InputState.Change(virtualMouse.scroll, scrollDirection * scrollSpeed);
        }
    }

    private void Click(InputAction.CallbackContext ctx) {
        // Copy the mouse state of the virtual mouse and apply the left mouse button click
        virtualMouse.CopyState<MouseState>(out var mouseState);
        mouseState.WithButton(MouseButton.Left, ctx.control.IsPressed());
        // Update the virtual mouse state to apply the action to the UI
        InputState.Change(virtualMouse, mouseState);
    }

}