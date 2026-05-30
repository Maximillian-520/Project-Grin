using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    // ====================================================================================================
    //                     Weapon Functions
    // ====================================================================================================
    #region Weapon
    public abstract void Trigger();
    #endregion

    // ====================================================================================================
    //                     Helper Functions
    // ====================================================================================================
    #region Helper
    protected Vector3 GetSpreadOffset(Transform fpsTransform, float maxSpread)
    {
        float offsetMagnitude = Random.Range(0f, maxSpread * Mathf.Deg2Rad);
        Vector2 spreadDirection = Random.insideUnitCircle.normalized;
        Vector3 offsetDirectionX = fpsTransform.right * spreadDirection.x * offsetMagnitude;
        Vector3 offsetDirectionY = fpsTransform.up * spreadDirection.y * offsetMagnitude;
        return offsetDirectionX + offsetDirectionY;
    }
    #endregion
}
