using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private List<BaseWeapon> weaponList;
    [SerializeField] private BaseWeapon initialWeapon;

    private BaseWeapon currentWeapon;
    private int currentWeaponIndex;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(weaponList.Count > 0, "weaponList is empty");
        Debug.Assert(initialWeapon, "initialWeapon is empty");
        // Initialize
        currentWeapon = initialWeapon;
        currentWeaponIndex = weaponList.IndexOf(initialWeapon);
        foreach (BaseWeapon weapon in weaponList) weapon.gameObject.SetActive(weapon == currentWeapon);
        initialWeapon.Setup();
    }
    #endregion

    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public void TriggerWeapon() {currentWeapon.Trigger();}

    public void ChangeToWeapon(BaseWeapon newWeapon)
    {
        // Get check weapon
        if (weaponList.IndexOf(newWeapon) == -1) return;
        // Switch to new weapon
        if (!currentWeapon.IsUnityNull()) {currentWeapon.gameObject.SetActive(false);}
        newWeapon.gameObject.SetActive(true);
        newWeapon.Setup();
        currentWeapon = newWeapon;
    }

    // Change to the next weapon on the list
    public void ChangeToNextWeapon(bool isInverted)
    {
        // Get new weapon
        if (isInverted)
        {
            currentWeaponIndex += weaponList.Count;
            currentWeaponIndex = (currentWeaponIndex - 1) % weaponList.Count;
        }
        else {currentWeaponIndex = (currentWeaponIndex + 1) % weaponList.Count;}
        BaseWeapon newWeapon = weaponList[currentWeaponIndex];
        // Switch to new weapon
        if (!currentWeapon.IsUnityNull()) {currentWeapon.gameObject.SetActive(false);}
        newWeapon.gameObject.SetActive(true);
        newWeapon.Setup();
        currentWeapon = newWeapon;
    }
    #endregion
}
