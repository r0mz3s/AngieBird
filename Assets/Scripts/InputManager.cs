using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    public static Vector2 MousePosition;
    public static bool WasLeftMouseButtonPressed;
    public static bool WasLeftMouseButtonReleased;
    public static bool IsLeftMousePressed;

    private InputAction mousePositionAction;
    private InputAction mouseAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        mousePositionAction = PlayerInput.actions["MousePosition"];
        mouseAction = PlayerInput.actions["Mouse"];

    }

    private void Update()
    {
        MousePosition = mousePositionAction.ReadValue<Vector2>();
        WasLeftMouseButtonPressed = mouseAction.WasPressedThisFrame();
        WasLeftMouseButtonReleased = mouseAction.WasReleasedThisFrame();
        IsLeftMousePressed = mouseAction.IsPressed();
    }
}