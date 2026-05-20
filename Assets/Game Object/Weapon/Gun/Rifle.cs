using UnityEngine;

public class Rifle : BaseWeapon
{
    [Header("Component and Object")]
    [SerializeField] private Camera fpsCamera;
    [Header("Gun Data")]
    [SerializeField] private GameObject bulletHitEffect;
    [SerializeField] private float bulletHitEffectDuration = 0.5f;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(fpsCamera, "fpsCamera is missing");
        Debug.Assert(bulletHitEffect, "bulletHitEffect is empty");
    }
    #endregion

    public override void Trigger()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
            GameObject bulletHitEffectInstance = Instantiate(
                bulletHitEffect, hit.point, Quaternion.LookRotation(hit.normal)
            );
            Destroy(bulletHitEffectInstance, bulletHitEffectDuration);
        }
    }
}
