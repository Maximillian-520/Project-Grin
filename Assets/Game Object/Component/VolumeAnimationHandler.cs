using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeAnimationHandler : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private Volume volume;
    [Header("Effect Data")]
    [SerializeField] private float vignettePulsePeak = 0.4f;
    [SerializeField] private float vignettePulseDuration = 0.25f;

    private DG.Tweening.Sequence currentSequence;
    private float initialVignetteIntensity;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(volume, "volume is missing");
        // Initialize
        if (volume.profile.TryGet(out Vignette vignette))
        {
            initialVignetteIntensity = vignette.intensity.value;
        }
    }
    #endregion

    // ====================================================================================================
    //                     Effect Functions
    // ====================================================================================================
    #region Effect
    public void DoVignettePulse()
    {
        if (!currentSequence.IsUnityNull()) currentSequence.Kill();
        if (volume.profile.TryGet(out Vignette vignette))
        {
            currentSequence = DOTween.Sequence();
            currentSequence.Append(
                DOTween.To(
                    () => {return vignette.intensity.value;},
                    (float value) => {vignette.intensity.value = value;},
                    vignettePulsePeak,
                    vignettePulseDuration / 2
                )
            );
            currentSequence.Append(
                DOTween.To(
                    () => {return vignette.intensity.value;},
                    (float value) => {vignette.intensity.value = value;},
                    initialVignetteIntensity,
                    vignettePulseDuration / 2
                )
            );
        }
    }
    #endregion
}
