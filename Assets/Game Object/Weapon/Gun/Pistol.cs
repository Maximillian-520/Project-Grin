using Unity.VisualScripting;
using UnityEngine;

public class Pistol : BaseWeapon
{
    [Header("Component and Object")]
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private ParticleSystem muzzleFlashEffect;
    [Header("Gun Data")]
    [SerializeField] private float fireRate = 4;
    [SerializeField] private float maxSpread = 12f;
    [SerializeField] private int damage = 5;
    [SerializeField] private GameObject bulletHitEffectPrefab;
    [SerializeField] private GameObject bulletEnemyHitEffectPrefab;
    [SerializeField] private float bulletHitEffectDuration = 0.5f;

    private float nextFireTime = -1f;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(fpsCamera, "fpsCamera is missing");
        Debug.Assert(muzzleFlashEffect, "muzzleFlashEffect is missing");
        Debug.Assert(bulletHitEffectPrefab, "bulletHitEffectPrefab is empty");
        Debug.Assert(bulletEnemyHitEffectPrefab, "bulletEnemyHitEffectPrefab is missing");
    }
    #endregion

    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public override void Setup() {}

    public override void Trigger()
    {
        // Check can fire
        if (Time.time < nextFireTime) return;
        // Fire
        RaycastHit hit;
        bool isHit = Physics.Raycast(
            fpsCamera.transform.position,
            fpsCamera.transform.forward + GetSpreadOffset(fpsCamera.transform, maxSpread),
            out hit
        );
        if (isHit)
        {
            // Check for damageable
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (!damageable.IsUnityNull()) damageable.ReceiveDamage(damage);
            // Spawn bullet hit effect
            GameObject bulletHitEffectInstance;
            if (!damageable.IsUnityNull())
            {
                bulletHitEffectInstance = Instantiate(
                    bulletEnemyHitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal)
                );
            }
            else
            {
                bulletHitEffectInstance = Instantiate(
                    bulletHitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal)
                );
            }
            Destroy(bulletHitEffectInstance, bulletHitEffectDuration);
        }
        muzzleFlashEffect.Play(true);
        // Set fire time
        nextFireTime = Time.time + (1.0f / (float)fireRate);
    }
    #endregion
}
