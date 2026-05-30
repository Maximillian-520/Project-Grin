using Unity.VisualScripting;
using UnityEngine;

public class GunDropController : MonoBehaviour
{
    [Header("Object and Component")]
    [SerializeField] private Transform topLeftPosition;
    [SerializeField] private Transform bottomRightPosition;
    [Header("Gun Drop Settings")]
    [SerializeField] private GunDrop gunDropPrefab;
    [SerializeField] private float spawnTime = 5f;

    private Vector2 rectPosition;
    private Vector2 rectSize;
    private float spawnTimer;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(topLeftPosition, "topLeftPosition is missing");
        Debug.Assert(bottomRightPosition, "bottomRightPosition is missing");
        Debug.Assert(gunDropPrefab, "gunDropPrefab is missing");
        // Initialize
        CalculateRect();
        spawnTimer = spawnTime;
    }

    private void Update()
    {
        // Update spawn timer
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnGunDrop();
            spawnTimer = spawnTime;
        }
    }
    #endregion

    // ====================================================================================================
    //                     Gun Drop Functions
    // ====================================================================================================
    #region Gun Drop
    private void SpawnGunDrop()
    {
        GunDrop gunDropInstance = Instantiate(gunDropPrefab);
        Vector2 randomPosition = GetRandomPosition();
        gunDropInstance.transform.position = new Vector3(
            randomPosition.x,
            transform.position.y,
            randomPosition.y
        );
    }
    #endregion

    // ====================================================================================================
    //                     Rect Functions
    // ====================================================================================================
    #region Rect
    private Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(rectPosition.x, rectPosition.x + rectSize.x),
            Random.Range(rectPosition.y, rectPosition.y + rectSize.y)
        );
    }

    private void CalculateRect()
    {
        // Calculate corners
        Vector2 topLeftCorner = new Vector2(
            topLeftPosition.position.x, topLeftPosition.position.z
        );
        Vector2 bottomRightCorner = new Vector2(
            bottomRightPosition.position.x, bottomRightPosition.position.z
        );
        // Calculate rect size (rect position is alwasy top left of the square)
        rectPosition = new Vector2(
            Mathf.Min(topLeftCorner.x, bottomRightCorner.x),
            Mathf.Min(topLeftCorner.y, bottomRightCorner.y)
        );
        // Calculate rect size
        rectSize = new Vector2(
            Mathf.Abs(topLeftCorner.x - bottomRightCorner.x),
            Mathf.Abs(topLeftCorner.y - bottomRightCorner.y)
        );
    }
    #endregion
}
