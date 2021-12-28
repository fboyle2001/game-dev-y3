using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

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
        virtualMouse = InputSystem.AddDevice("VirtualMouse") as Mouse;
        InputSystem.DisableDevice(virtualMouse);
    }

    void OnEnable() {
        if(!virtualMouse.enabled) {
            InputSystem.EnableDevice(virtualMouse);
        }

        moveProp.action.performed += UpdateCursorDirection;
        moveProp.action.canceled += ZeroCursorDirection;
        moveProp.action.Enable();

        clickProp.action.performed += Click;
        clickProp.action.Enable();

        scrollProp.action.performed += UpdateScrollDirection;
        scrollProp.action.canceled += ZeroScrollDirection;
        scrollProp.action.Enable();

        InputState.Change(virtualMouse.position, cursorTransform.anchoredPosition);
        InputSystem.onAfterUpdate += UpdateCursor;
    }

    void OnDisable() {
        moveProp.action.performed -= UpdateCursorDirection;
        moveProp.action.canceled -= ZeroCursorDirection;
        moveProp.action.Disable();

        clickProp.action.performed -= Click;
        clickProp.action.Disable();

        scrollProp.action.performed -= UpdateScrollDirection;
        scrollProp.action.canceled -= ZeroScrollDirection;
        scrollProp.action.Disable();
        
        InputSystem.onAfterUpdate -= UpdateCursor;

        InputSystem.DisableDevice(virtualMouse);
    }

    void OnDestroy() {
        InputSystem.RemoveDevice(virtualMouse);
    }

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
        if(direction != Vector2.zero) {
            Vector2 currentPosition = virtualMouse.position.ReadValue();
            float deltaX = direction.x * mouseSpeed * Time.unscaledDeltaTime * GlobalSettings.horizontalMouseSensitivity;
            float deltaY = direction.y * mouseSpeed * Time.unscaledDeltaTime * GlobalSettings.verticalMouseSensitivity;
            Vector2 delta = new Vector2(deltaX, deltaY);
            Vector2 newPosition = currentPosition + delta;

            newPosition.x = Mathf.Clamp(newPosition.x, canvas.pixelRect.xMin, canvas.pixelRect.xMax);
            newPosition.y = Mathf.Clamp(newPosition.y, canvas.pixelRect.yMin, canvas.pixelRect.yMax);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, delta);

            cursorTransform.anchoredPosition = newPosition;
        }

        if(scrollDirection != Vector2.zero) {
            InputState.Change(virtualMouse.scroll, scrollDirection * scrollSpeed);
        }
    }

    private void Click(InputAction.CallbackContext ctx) {
        virtualMouse.CopyState<MouseState>(out var mouseState);
        mouseState.WithButton(MouseButton.Left, ctx.control.IsPressed());
        InputState.Change(virtualMouse, mouseState);
    }

}