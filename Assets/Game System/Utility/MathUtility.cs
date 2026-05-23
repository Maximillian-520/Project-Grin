using UnityEngine;

/// <summary>
/// Utility script for other math functions that does not exist in system or unity
/// </summary>

public static class MathUtility
{
    // ====================================================================================================
    //                     Get Distance
    // ====================================================================================================
    public static float GetDistance(Vector3 fromPosition, Vector3 toPosition)
    {
        Vector3 distanceVector = toPosition - fromPosition;
        return distanceVector.magnitude;
    }
    public static float GetDistance(Vector2 fromPosition, Vector2 toPosition)
    {
        Vector2 distanceVector = toPosition - fromPosition;
        return distanceVector.magnitude;
    }

    // ====================================================================================================
    //                     Get Direction
    // ====================================================================================================
    public static Vector3 GetDirection(Vector3 fromPosition, Vector3 toPosition)
    {
        Vector3 distanceVector = toPosition - fromPosition;
        return distanceVector.normalized;
    }
    public static Vector2 GetDirection(Vector2 fromPosition, Vector2 toPosition)
    {
        Vector3 distanceVector = toPosition - fromPosition;
        return distanceVector.normalized;
    }

    // ====================================================================================================
    //                     Get Angle
    // ====================================================================================================
    public static float GetAngle(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    // ====================================================================================================
    //                     Rotate Vector
    // ====================================================================================================
    public static Vector2 RotateVector(Vector2 vector, float angleDegree)
    {
        float angleRad = angleDegree * Mathf.Deg2Rad;
        float angleCos = Mathf.Cos(angleRad);
        float angleSin = Mathf.Sin(angleRad);
        return new Vector2(
            vector.x * angleCos - vector.y * angleSin,
            vector.x * angleSin + vector.y * angleCos
        );
    }
}
