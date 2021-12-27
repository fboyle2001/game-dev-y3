using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

public class InputControllableMouse : MonoBehaviour {
   
    public RectTransform cursorTransform;
    public PlayerInput playerInput;
    public RectTransform canvasTransform;

    private float mouseSpeed = 200;
    private Mouse virtualMouse;
    private InputAction moveCursorAction;
    private InputAction clickAction;

    private Resolution resolution;
    private Vector2 currentPosition;
    private bool visible;
    private bool prevMouseState;

    private Vector2 cursorDirection = Vector2.zero;

    void OnEnable() {
        virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        resolution = Screen.currentResolution;
        currentPosition = new Vector2(resolution.width / 2, resolution.height / 2);
        AnchorCursor(currentPosition);
        InputState.Change(virtualMouse.position, currentPosition);

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    void OnDisable() {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion() {
        if(virtualMouse == null || Gamepad.current == null) return;

        Vector2 stickValue = Gamepad.current.rightStick.ReadValue() * mouseSpeed * Time.deltaTime;
        Vector2 current = virtualMouse.position.ReadValue();
        Vector2 newPos = current + stickValue;

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, stickValue);

        if(prevMouseState != Gamepad.current.aButton.IsPressed()) {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState = mouseState.WithButton(MouseButton.Left, Gamepad.current.aButton.IsPressed());
            InputState.Change(virtualMouse, mouseState);
            Debug.Log("Updated state: " +Gamepad.current.aButton.IsPressed() );

            if(Gamepad.current.aButton.IsPressed()) {
                Debug.Log(mouseState.position);
            }

            prevMouseState = Gamepad.current.aButton.IsPressed();
        }

        AnchorCursor(newPos);
    }

    private void AnchorCursor(Vector2 pos) {
        Vector2 ap;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pos, null, out ap);
        cursorTransform.anchoredPosition = ap;
    }

}
