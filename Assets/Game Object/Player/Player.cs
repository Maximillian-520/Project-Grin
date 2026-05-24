using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance {private set; get;}

    [Header("Component and Object")]
    [SerializeField] private CameraMovementFPS cameraMovementFPS;
    [SerializeField] private VolumeAnimationHandler volumeAnimationHandler;
    [SerializeField] private BarUI healthBarUI;
    [Header("Player Data")]
    [SerializeField] private int maxHealth = 100;

    private bool isCursorLocked = true;
    public float currentHealth {private set; get;}

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Awake() {Instance = this;}

    private void OnDestroy() {Instance = null;}

    private void Start()
    {
        // Assertion check
        Debug.Assert(cameraMovementFPS, "cameraMovementFPS is missing");
        Debug.Assert(volumeAnimationHandler, "volumeAnimationHandler is missing");
        Debug.Assert(healthBarUI, "healthBarUI is missing");
        // Connect events
        InputHandler.Instance.OnCursorTogglePressed.AddListener(ToggleCursorLock);
        // Initialize
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentHealth = maxHealth;
        healthBarUI.UpdateBar(1.0f);
    }
    #endregion

    // ====================================================================================================
    //                     Damageable Functions
    // ====================================================================================================
    #region Damageable
    public void ReceiveDamage(int damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        healthBarUI.UpdateBar((float)currentHealth / (float)maxHealth);
        volumeAnimationHandler.DoVignettePulse();
        if (currentHealth <= 0) Debug.Log("Player dead");
    }
    #endregion

    // ====================================================================================================
    //                     Cursor Functions
    // ====================================================================================================
    #region Cursor
    private void ToggleCursorLock()
    {
        if (isCursorLocked)
        {
            // Unlock
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cameraMovementFPS.enabled = false;
            isCursorLocked = false;
        }
        else
        {
            // Lock
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameraMovementFPS.enabled = true;
            isCursorLocked = true;
        }
    }
    #endregion
}
