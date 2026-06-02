using Unity.VisualScripting;
using UnityEngine;

public class GunDrop : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private GunDropVisual gunDropVisual;
    [SerializeField] private GameObject rifleDisplay;
    [SerializeField] private GameObject shogunDisplay;
    [Header("Gun Drop Settings")]
    [SerializeField] private float despawnTime = 9f;
    [SerializeField] private float flickerTime = 6f;
    [Header("Audio Settings")]
    [SerializeField] private string rifleGunDropAudioName = "Rifle Gun Drop";
    [SerializeField] private string shotgunGunDropAudioName = "Shotgun Gun Drop";

    private bool isHoldingRifle = false;
    private bool isHoldingShotgun = false;
    private float despawnTimer;
    private float flickerTimer;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(gunDropVisual, "gunDropVisual is missing");
        Debug.Assert(rifleDisplay, "rifleDisplay is missing");
        Debug.Assert(shogunDisplay, "shogunDisplay is missing");
        // Initialize
        if (Random.Range(0, 2) == 1) isHoldingRifle = true;
        else isHoldingShotgun = true;
        rifleDisplay.SetActive(isHoldingRifle);
        shogunDisplay.SetActive(isHoldingShotgun);
        despawnTimer = despawnTime;
        flickerTimer = flickerTime;
    }

    private void Update()
    {
        // Update despawn timer
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0) Destroy(gameObject);
        // Update flicker timer
        if (flickerTimer > 0)
        {
            flickerTimer -= Time.deltaTime;
            if (flickerTimer <= 0) gunDropVisual.isFlickering = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerWeapon playerWeapon = other.gameObject.GetComponent<PlayerWeapon>();
        if (!playerWeapon.IsUnityNull())
        {
            if (isHoldingRifle)
            {
                // Rifle
                playerWeapon.HoldRifle();
                AudioManager.Instance.PlaySFX(rifleGunDropAudioName);
            }
            if (isHoldingShotgun)
            {
                // Shotgun
                playerWeapon.HoldShotgun();
                AudioManager.Instance.PlaySFX(shotgunGunDropAudioName);
            }
            Destroy(gameObject);
        }
    }
    #endregion
}
