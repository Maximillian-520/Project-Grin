using UnityEngine;

/// <summary>
/// Camera component used for hand held swinging motion effect.
/// Failed, unused
/// </summary>

public class CameraSwing : MonoBehaviour
{
    const float TOLERATED_DISTANCE = 0.01f;

    [Header("Component and Object")]
    [SerializeField] private Transform pivot;
    [Header("Shake Settings")]
    [SerializeField] private bool is3D = false;
    public float speed = 0.1f;
    public float range = 0.1f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

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
        targetPosition = GetNewTargetPosition();
    }

    private void FixedUpdate()
    {
        // 2D swing
        if (!is3D)
        {
            float positionX = Mathf.MoveTowards(
                pivot.transform.localPosition.x, targetPosition.x, Time.fixedDeltaTime * speed
            );
            float positionY = Mathf.MoveTowards(
                pivot.transform.localPosition.y, targetPosition.y, Time.fixedDeltaTime * speed
            );
            pivot.transform.localPosition = new Vector3(
                positionX, positionY, pivot.transform.localPosition.z
            );
            if (IsTargetReached()) targetPosition = GetNewTargetPosition();
        }
        // 3D swing
        else
        {
            float positionX = Mathf.MoveTowards(
                pivot.transform.localPosition.x, targetPosition.x, Time.fixedDeltaTime * speed
            );
            float positionY = Mathf.MoveTowards(
                pivot.transform.localPosition.y, targetPosition.y, Time.fixedDeltaTime * speed
            );
            float positionZ = Mathf.MoveTowards(
                pivot.transform.localPosition.z, targetPosition.z, Time.fixedDeltaTime * speed
            );
            pivot.transform.localPosition = new Vector3(positionX, positionY, positionZ);
            if (IsTargetReached()) targetPosition = GetNewTargetPosition();
        }
    }

    private void OnEnable()
    {
        targetPosition = GetNewTargetPosition();
    }

    private void OnDisable()
    {
        pivot.transform.localPosition = initialPosition;
    }
    #endregion

    // ====================================================================================================
    //                     Helper Functions
    // ====================================================================================================
    #region Helper
    private bool IsTargetReached()
    {
        return MathUtility.GetDistance(pivot.transform.localPosition, targetPosition) < TOLERATED_DISTANCE;
    }

    private Vector3 GetNewTargetPosition()
    {
        float offsetX = Random.Range(-range, range);
        float offsetY = Random.Range(-range, range);
        Vector3 offset = pivot.transform.right.normalized * offsetX;
        offset += pivot.transform.up.normalized * offsetY;
        return initialPosition + offset;
    }
    #endregion
}
