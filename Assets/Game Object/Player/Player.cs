using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Node References")]
    [SerializeField] private CameraMovementFPS cameraMovementFPS;

    private bool isCursorLocked = true;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(cameraMovementFPS, "cameraMovementFPS is missing");
        // Connect events
        InputHandler.Instance.OnCursorTogglePressed.AddListener(ToggleCursorLock);
        // Initialize
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
