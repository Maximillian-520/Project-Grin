using Nova;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class BasicButtonEffect : BaseButtonEffect
{
    [Header("Scale Effect")]
    [SerializeField] private float normalScale = 1.0f;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float pressedScale = 0.9f;
    [Tooltip("Scale animation duration")]
    [SerializeField] private float scaleDuration = 0.2f;
    // ====================================================================================================
    [Header("Color Effect")]
    [SerializeField] private UIBlock2D colorBlock;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.gray;
    [SerializeField] private Color pressedColor = Color.black;
    [Tooltip("Color animation duration")]
    [SerializeField] private float colorDuration = 0.2f;
    // ====================================================================================================
    [Header("Text Color Effect")]
    [SerializeField] private TextBlock textBlock;
    [SerializeField] private Color normalTextColor = Color.white;
    [SerializeField] private Color hoverTextColor = Color.gray;
    [SerializeField] private Color pressedTextColor = Color.black;
    [Tooltip("Text color animation duration")]
    [SerializeField] private float textColorDuration = 0.2f;

    private Tween scaleTween;
    private Tween colorTween;
    private Tween textColorTween;

    // ====================================================================================================
    //                     Interactable Functions
    // ====================================================================================================
    #region Interactable
    override public void ResetButton()
    {
        if (!scaleTween.IsUnityNull()) scaleTween.Kill();
        if (!colorTween.IsUnityNull()) colorTween.Kill();
        if (!textColorTween.IsUnityNull()) textColorTween.Kill();
        transform.localScale = Vector3.one * normalScale;
        colorBlock.Color = normalColor;
        textBlock.Color = normalTextColor;
    }

    override public void ButtonNormal(Gesture.OnUnhover evt)
    {
        DoScaleEffect(normalScale);
        DoColorEffect(normalColor);
        DoTextColorEffect(normalTextColor);
    }

    override public void ButtonHover(Gesture.OnHover evt)
    {
        DoScaleEffect(hoverScale);
        DoColorEffect(hoverColor);
        DoTextColorEffect(hoverTextColor);
    }

    override public void ButtonPressed(Gesture.OnPress evt)
    {
        DoScaleEffect(pressedScale);
        DoColorEffect(pressedColor);
        DoTextColorEffect(pressedTextColor);
    }
    #endregion

    // ====================================================================================================
    //                     Effect Functions
    // ====================================================================================================
    #region Effect
    private void DoScaleEffect(float targetValue)
    {
        if (!scaleTween.IsUnityNull()) scaleTween.Kill();
        scaleTween = DOTween.To(
            ()=>transform.localScale,
            x=>transform.localScale = x,
            Vector3.one * targetValue,
            scaleDuration
        );
    }

    private void DoColorEffect(Color targetValue)
    {
        if (!colorBlock) return;
        if (!colorTween.IsUnityNull()) colorTween.Kill();
        colorTween = DOTween.To(
            ()=>colorBlock.Color,
            x=>colorBlock.Color = x,
            targetValue,
            colorDuration
        );
    }

    private void DoTextColorEffect(Color targetValue)
    {
        if (!textBlock) return;
        if (!textColorTween.IsUnityNull()) textColorTween.Kill();
        textColorTween = DOTween.To(
            ()=>textBlock.Color,
            x=>textBlock.Color = x,
            targetValue,
            textColorDuration
        );
    }
    #endregion
}
