using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private WeaponController weaponController;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(weaponController, "weaponController is missing");
    }

    private void Update()
    {
        // Check is cursor locked (player focused on screen)
        if (Cursor.lockState != CursorLockMode.Locked) return;
        // Handle shoot input
        if (InputHandler.Instance.shootInput) weaponController.TriggerWeapon();
        // Handle scroll input
        if (Mouse.current.scroll.ReadValue().y != 0)
        {
            int scrollValue = (int)Mathf.Sign(Mouse.current.scroll.ReadValue().y);
            weaponController.ChangeWeapon(scrollValue == -1);
        }
    }
    #endregion
}
