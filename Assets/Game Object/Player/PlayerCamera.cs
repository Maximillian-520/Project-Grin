using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Death Settings")]
    [SerializeField] private float targetRotation = -90f;
    [SerializeField] private float duration = 1f;

    private Tween currentTween;

    // ====================================================================================================
    //                     Camera Functions
    // ====================================================================================================
    #region Camera
    public void DoCameraDeath()
    {
        if (!currentTween.IsUnityNull()) currentTween.Kill();
        currentTween = DOTween.To(
            ()=>transform.localEulerAngles,
            x=>transform.localEulerAngles = x,
            new Vector3(targetRotation, transform.localEulerAngles.y, transform.localEulerAngles.z),
            duration
        );
    }
    #endregion
}
