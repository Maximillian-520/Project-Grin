using Nova;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Shotgun : BaseWeapon
{
    public UnityEvent GunAmmoEmptied;

    [Header("Component and Object")]
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private ParticleSystem muzzleFlashEffect;
    [SerializeField] private BarUI ammoBarUI;
    [SerializeField] private UIBlock crosshair;
    [Header("Gun Data")]
    [SerializeField] private float fireRate = 2;
    [SerializeField] private float maxSpread = 20f;
    [SerializeField] public int maxAmmo {private set; get;} = 8;
    [SerializeField] private int pellet = 10;
    [SerializeField] private int damage = 5;
    [SerializeField] private GameObject bulletHitEffectPrefab;
    [SerializeField] private GameObject bulletEnemyHitEffectPrefab;
    [SerializeField] private float bulletHitEffectDuration = 0.5f;
    [Header("Audio Settings")]
    [SerializeField] private string gunAudioName = "Shotgun Shot";

    private float nextFireTime = -1f;
    private int currentAmmo = 0;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(fpsCamera, "fpsCamera is missing");
        Debug.Assert(muzzleFlashEffect, "muzzleFlashEffect is missing");
        Debug.Assert(ammoBarUI, "ammoBarUI is missing");
        Debug.Assert(crosshair, "crosshair is missing");
        Debug.Assert(bulletHitEffectPrefab, "bulletHitEffectPrefab is empty");
        Debug.Assert(bulletEnemyHitEffectPrefab, "bulletEnemyHitEffectPrefab is missing");
    }
    #endregion

    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public override void Setup()
    {
        ammoBarUI.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(true);
    }

    public override void Trigger()
    {
        // Check can fire
        if (Time.time < nextFireTime) return;
        if (currentAmmo <= 0)
        {
            nextFireTime = Time.time + (1.0f / (float)fireRate);
            GunAmmoEmptied?.Invoke();
            return;
        }
        // Fire
        for (int index = 0; index < pellet; index++)
        {
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
        }
        muzzleFlashEffect.Play(true);
        AudioManager.Instance.PlaySFX(gunAudioName);
        // Set variables
        nextFireTime = Time.time + (1.0f / (float)fireRate);
        currentAmmo--;
        // Check ammo
        if (currentAmmo <= 0) GunAmmoEmptied?.Invoke();
        ammoBarUI.UpdateBar((float)currentAmmo / (float)maxAmmo);
    }

    public void Reload()
    {
        nextFireTime = Time.time;
        currentAmmo = maxAmmo;
        ammoBarUI.UpdateBar((float)currentAmmo / (float)maxAmmo);
    }
    #endregion
}
