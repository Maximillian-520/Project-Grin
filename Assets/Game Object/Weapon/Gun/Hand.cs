using Nova;
using UnityEngine;

public class Hand : BaseWeapon
{
    [Header("Component and Object")]
    [SerializeField] private BarUI ammoBarUI;
    [SerializeField] private UIBlock crosshair;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(ammoBarUI, "ammoBarUI is missing");
        Debug.Assert(crosshair, "crosshair is missing");
    }
    #endregion

    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public override void Setup()
    {
        ammoBarUI.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
    }

    public override void Trigger(){}
    #endregion
}
