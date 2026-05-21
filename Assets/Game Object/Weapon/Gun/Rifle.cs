using UnityEngine;

public class Rifle : BaseWeapon
{
    [Header("Component and Object")]
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Transform muzzlePosition;

    [Header("Gun Data")]
    [SerializeField] private int fireRate = 6;
    [SerializeField] private float maxSpread = 9f;
    [SerializeField] private GameObject muzzleFlashEffectPrefab;
    [SerializeField] private GameObject bulletHitEffectPrefab;
    [SerializeField] private float muzzleFlashEffectDuration = 0.5f;
    [SerializeField] private float bulletHitEffectDuration = 0.5f;

    private float nextFireTime = 0;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(fpsCamera, "fpsCamera is missing");
        Debug.Assert(muzzlePosition, "muzzlePosition is missing");
        Debug.Assert(muzzleFlashEffectPrefab, "muzzleFlashEffectPrefab is missing");
        Debug.Assert(bulletHitEffectPrefab, "bulletHitEffectPrefab is empty");
    }
    #endregion

    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public override void Trigger()
    {
        // Check can fire
        if (Time.time < nextFireTime) return;
        // Fire
        RaycastHit hit;
        bool isHit = Physics.Raycast(
            fpsCamera.transform.position,
            fpsCamera.transform.forward + GetSpreadOffset(fpsCamera.transform),
            out hit
        );
        if (isHit)
        {

            // Debug.Log(hit.transform.name);
            
            // Spawn bullet hit effect
            GameObject bulletHitEffectInstance = Instantiate(
                bulletHitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal)
            );
            Destroy(bulletHitEffectInstance, bulletHitEffectDuration);
        }
        // Spawn muzzle flash effect
        GameObject muzzleFlashEffectInstance = Instantiate(muzzleFlashEffectPrefab, muzzlePosition);
        Destroy(muzzleFlashEffectInstance, muzzleFlashEffectDuration);
        // Set fire time
        nextFireTime = Time.time + (1.0f / (float)fireRate);
    }

    private Vector3 GetSpreadOffset(Transform fpsTransform)
    {
        float offsetMagnitude = Random.Range(0f, maxSpread * Mathf.Deg2Rad);
        Vector2 spreadDirection = Random.insideUnitCircle.normalized;
        Vector3 offsetDirectionX = fpsTransform.right * spreadDirection.x * offsetMagnitude;
        Vector3 offsetDirectionY = fpsTransform.up * spreadDirection.y * offsetMagnitude;
        return offsetDirectionX + offsetDirectionY;
    }
    #endregion
}
