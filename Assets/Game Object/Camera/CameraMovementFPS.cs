using System;
using UnityEngine;

/// <summary>
/// Camera component used for FPS camera movement.
/// </summary>

public class CameraMovementFPS : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private Transform xAxisRig;
    [SerializeField] private Transform yAxisRig;
    [Header("Camera Settings")]
    [SerializeField] private float baseSensitivity = 300.0f;
    public float sensitivityScale = 1.0f;
    public float smoothingSpeed = 10f;
    [Range(0f, 90f)] public float verticalLookLimit = 90f;
    public bool invertXAxis = false;
    public bool invertYAxis = false;

    private float currentMouseX;
    private float currentMouseY;
    private float verticalRotation;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(xAxisRig, "xAxisRig is missing");
        Debug.Assert(yAxisRig, "yAxisRig is missing");
        // Initialize
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        // Get mouse movement
        float sensitivity = baseSensitivity * sensitivityScale;
        float mouseMovementX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseMovementY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        if (invertXAxis) mouseMovementX *= -1;
        if (invertYAxis) mouseMovementY *= -1;
        // Apply smoothing
        currentMouseX = Mathf.Lerp(currentMouseX, mouseMovementX, smoothingSpeed * Time.deltaTime);
        currentMouseY = Mathf.Lerp(currentMouseY, mouseMovementY, smoothingSpeed * Time.deltaTime);
        // Apply horizontal rotation
        xAxisRig.Rotate(Vector3.up * currentMouseX);
        // Apply vertical rotation
        verticalRotation -= currentMouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        yAxisRig.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    #endregion
}
