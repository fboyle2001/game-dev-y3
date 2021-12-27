using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

public class ImprovedMouse : MonoBehaviour {
    
    public InputActionProperty moveProp;
    public InputActionProperty clickProp;
    public RectTransform cursorTransform;
    public RectTransform canvasTransform;

    private Mouse virtualMouse;

    private InputAction moveCursorAction;
    private InputAction clickAction;

    private Vector2 direction = Vector2.zero;
    private float mouseSpeed = 1000;

    void Awake() {
    }

    void OnEnable() {
        virtualMouse = InputSystem.AddDevice("VirtualMouse") as Mouse;

        if(!virtualMouse.added) {
            Debug.Log("**Virtual Mouse is not added**");
        }

        if(!virtualMouse.enabled) {
            Debug.Log("**Virtual Mouse is not enabled**");
        }

        InputUser.PerformPairingWithDevice(virtualMouse);

        moveProp.action.performed += ctx => { direction = ctx.ReadValue<Vector2>(); };
        moveProp.action.canceled += _ => direction = Vector2.zero;
        moveProp.action.Enable();

        clickProp.action.started += Click;
        clickProp.action.canceled += Click;
        clickProp.action.Enable();

        cursorTransform.position = virtualMouse.position.ReadValue();

        InputSystem.onAfterUpdate += UpdateCursor;
    }

    void OnDisable() {
        InputSystem.RemoveDevice(virtualMouse);
        moveProp.action.Disable();
        clickProp.action.Disable();
        InputSystem.onAfterUpdate -= UpdateCursor;
    }

    void UpdateCursor() {
        if(direction == Vector2.zero) return;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 delta = direction * mouseSpeed * Time.deltaTime;
        Vector2 newPosition = currentPosition + delta;

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, delta);

        AnchorCursor(newPosition);
    }

    private void AnchorCursor(Vector2 pos) {
        Vector2 ap;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pos, null, out ap);
        cursorTransform.anchoredPosition = ap;
    }

    private void Click(InputAction.CallbackContext ctx) {
        virtualMouse.CopyState<MouseState>(out var mouseState);
        mouseState.WithButton(MouseButton.Left, ctx.control.IsPressed());
        InputState.Change(virtualMouse, mouseState);
    }

}