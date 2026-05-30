using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private Hand hand;
    [SerializeField] private Rifle rifle;
    [SerializeField] private Shotgun shotgun;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(weaponController, "weaponController is missing");
        Debug.Assert(hand, "hand is missing");
        Debug.Assert(rifle, "rifle is missing");
        Debug.Assert(shotgun, "shotgun is missing");
        // Connect events
        rifle.GunAmmoEmptied.AddListener(HoldHand);
        shotgun.GunAmmoEmptied.AddListener(HoldHand);
    }

    private void Update()
    {
        // Check is cursor locked (player focused on screen)
        if (Cursor.lockState != CursorLockMode.Locked) return;
        // Handle shoot input
        if (InputHandler.Instance.shootInput) weaponController.TriggerWeapon();
        // // Handle scroll input
        // if (Mouse.current.scroll.ReadValue().y != 0)
        // {
        //     int scrollValue = (int)Mathf.Sign(Mouse.current.scroll.ReadValue().y);
        //     weaponController.ChangeWeapon(scrollValue == -1);
        // }
    }
    #endregion

    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public void HoldHand() {weaponController.ChangeToWeapon(hand);}

    public void HoldRifle()
    {
        weaponController.ChangeToWeapon(rifle);
        rifle.Reload();
    }

    public void HoldShotgun()
    {
        weaponController.ChangeToWeapon(shotgun);
        shotgun.Reload();
    }
    #endregion
}
