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
        foreach (BaseWeapon weapon in weaponList)
        {
            weapon.gameObject.SetActive(weapon == currentWeapon);
        }
    }
    #endregion

    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public void TriggerWeapon() {currentWeapon.Trigger();}

    // Change to the next weapon on the list
    public void ChangeWeapon(bool isInverted)
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
        currentWeapon = newWeapon;
    }
    #endregion
}
