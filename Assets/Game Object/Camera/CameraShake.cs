using UnityEngine;

/// <summary>
/// Camera component used for shaking effect.
/// </summary>

public class CameraShake : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private Transform pivot;
    [Header("Shake Settings")]
    [SerializeField] private bool is3D = false;
    public float range = 0.1f;

    private Vector3 initialPosition;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(pivot, "pivot is missing");
        // Initialize
        initialPosition = pivot.transform.localPosition;
    }

    private void FixedUpdate()
    {
        // 2D shake
        if (!is3D)
        {
            float offsetX = Random.Range(-range, range);
            float offsetY = Random.Range(-range, range);
            pivot.transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);
        }
        // 3D shake
        else
        {
            float offsetX = Random.Range(-range, range);
            float offsetY = Random.Range(-range, range);
            Vector3 offset = pivot.transform.right.normalized * offsetX;
            offset += pivot.transform.up.normalized * offsetY;
            pivot.transform.localPosition = initialPosition + offset;
        }
    }

    private void OnDisable()
    {
        pivot.transform.localPosition = initialPosition;
    }
    #endregion
}
