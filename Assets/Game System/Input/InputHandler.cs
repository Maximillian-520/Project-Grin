using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// This script is a template to handle input system.
/// Used for handling all unity's input action system.
/// Please rewrite this script as needed!
/// </summary>

public class InputHandler : MonoBehaviour
{
    // Toggle cursor events
    public UnityEvent OnCursorTogglePressed;
    public UnityEvent OnCursorToggleReleased;

    public static InputHandler Instance {private set; get;}

    public Vector2 moveInput = Vector2.zero;
    public bool sprintInput = false;
    public bool jumpInput = false;
    public bool crouchInput = false;
    public bool cursorToggleInput = false;
    public bool shootInput = false;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Awake() {Instance = this;}

    private void OnDestroy() {Instance = null;}
    #endregion
    
    // ====================================================================================================
    //                     Input Functions
    // ====================================================================================================
    #region Input
    public void UpdateMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void UpdateSprintInput(InputAction.CallbackContext ctx)
    {
        sprintInput = ctx.performed;
    }

    public void UpdateJumpInput(InputAction.CallbackContext ctx)
    {
        jumpInput = ctx.performed;
    }

    public void UpdateCrouchInput(InputAction.CallbackContext ctx)
    {
        crouchInput = ctx.performed;
    }

    public void UpdateCursorToggleInput(InputAction.CallbackContext ctx)
    {
        if (!cursorToggleInput && ctx.performed) OnCursorTogglePressed.Invoke();
        if (cursorToggleInput && !ctx.performed) OnCursorToggleReleased.Invoke();
        cursorToggleInput = ctx.performed;
    }

    public void UpdateShootInput(InputAction.CallbackContext ctx)
    {
        shootInput = ctx.performed;
    }
    #endregion
}
