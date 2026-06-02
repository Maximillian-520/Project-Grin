using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamageable
{
    public UnityEvent PlayerDied;

    public static Player Instance {private set; get;}

    [Header("Component and Object")]
    [SerializeField] private CameraMovementFPS cameraMovementFPS;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private VolumeAnimationHandler volumeAnimationHandler;
    [SerializeField] private BarUI healthBarUI;
    [Header("Player Data")]
    [SerializeField] private int maxHealth = 100;
    [Header("Audio Settings")]
    [SerializeField] private string injuredAudioName = "Player Injured";
    [SerializeField] private string diedAudioName = "Player Died";

    private bool isEnabled = false;
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
        Debug.Assert(playerCamera, "playerCamera is missing");
        Debug.Assert(volumeAnimationHandler, "volumeAnimationHandler is missing");
        Debug.Assert(healthBarUI, "healthBarUI is missing");
        // Connect events
        InputHandler.Instance.OnCursorTogglePressed.AddListener(ToggleCursorLock);
        // Initialize
        EnablePlayer();
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
        if (currentHealth <= 0) return;
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        healthBarUI.UpdateBar((float)currentHealth / (float)maxHealth);
        volumeAnimationHandler.DoVignettePulse();
        if (currentHealth > 0) AudioManager.Instance.PlaySFX(injuredAudioName);
        else AudioManager.Instance.PlaySFX(diedAudioName);
        if (currentHealth <= 0)
        {
            UnlockCursor();
            playerCamera.DoCameraDeath();
            PlayerDied?.Invoke();
        }
    }
    #endregion

    // ====================================================================================================
    //                     Cursor Functions
    // ====================================================================================================
    #region Cursor
    private void ToggleCursorLock()
    {
        if (!isEnabled) return;
        if (isCursorLocked) UnlockCursor();
        else LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraMovementFPS.enabled = true;
        isCursorLocked = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cameraMovementFPS.enabled = false;
        isCursorLocked = false;
    }
    #endregion

    // ====================================================================================================
    //                     Player Functions
    // ====================================================================================================
    #region Player
    public void EnablePlayer()
    {
        isEnabled = true;
        LockCursor();
    }

    public void DisablePlayer() {
        isEnabled = false;
        UnlockCursor();
    }
    #endregion
}
