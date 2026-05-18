using System;
using UnityEngine;

/// <summary>
/// Camera component used for FPS camera movement.
/// </summary>

public class CameraViewFPS : MonoBehaviour
{
    [Header("Node References")]
    [SerializeField] private Transform xAxisRig;
    [SerializeField] private Transform yAxisRig;
    [Header("Camera Settings")]
    [SerializeField] private float sensitivity = 300.0f;
    [Range(0f, 90f)] public float verticalLookLimit = 90f;

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
        float mouseMovementX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseMovementY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        // Apply horizontal rotation
        xAxisRig.Rotate(Vector3.up * mouseMovementX);
        // Apply vertical rotation
        verticalRotation -= mouseMovementY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        yAxisRig.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    #endregion
}
