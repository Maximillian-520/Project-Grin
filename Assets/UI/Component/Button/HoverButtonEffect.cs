using Nova;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Events;

public class HoverButtonEffect : BaseButtonEffect
{
    const float FULL_CIRCLE_DEGREES = 360;

    public UnityEvent buttonPressed;

    [Header("Button Settings")]
    [SerializeField] private UIBlock2D fillBlock;
    [Tooltip("Hovering time needed to fire button pressed event")]
    [SerializeField] private float fillHoverTime = 1f;
    [SerializeField] private bool isClockwise = true;
    // ====================================================================================================
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
    [Header("Fill Color Effect")]
    [SerializeField] private Color normalFillColor = Color.white;
    [SerializeField] private Color hoverFillColor = Color.gray;
    [SerializeField] private Color pressedFillColor = Color.black;
    [Tooltip("Fill color animation duration")]
    [SerializeField] private float fillColorDuration = 0.2f;
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
    private Tween fillColorTween;
    private Tween textColorTween;
    private bool isHovering;
    private float FillHoverTimer
    {
        set
        {
            fillHoverTimer = value;
            fillBlock.RadialFill.FillAngle = (value / fillHoverTime) * -FULL_CIRCLE_DEGREES;
        }
        get {return fillHoverTimer;}
    }
    private float fillHoverTimer;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Update()
    {
        // Check fill block and hovering
        if (!fillBlock || !isHovering) return;
        // Update fill
        if (FillHoverTimer < fillHoverTime)
        {
            FillHoverTimer += Time.deltaTime;
            if (FillHoverTimer >= fillHoverTime) buttonPressed?.Invoke();
        }
    }
    #endregion

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
        DoFillColorEffect(normalFillColor);
        DoTextColorEffect(normalTextColor);
        isHovering = false;
    }

    override public void ButtonHover(Gesture.OnHover evt)
    {
        DoScaleEffect(hoverScale);
        DoColorEffect(hoverColor);
        DoFillColorEffect(hoverFillColor);
        DoTextColorEffect(hoverTextColor);
        isHovering = true;
    }

    override public void ButtonPressed(Gesture.OnPress evt)
    {
        // DoScaleEffect(pressedScale);
        // DoColorEffect(pressedColor);
        // DoFillColorEffect(pressedFillColor);
        // DoTextColorEffect(pressedTextColor);
        isHovering = true;
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

    private void DoFillColorEffect(Color targetValue)
    {
        if (!fillBlock) return;
        if (!fillColorTween.IsUnityNull()) fillColorTween.Kill();
        fillColorTween = DOTween.To(
            ()=>fillBlock.Color,
            x=>fillBlock.Color = x,
            targetValue,
            fillColorDuration
        );
        if (targetValue == normalFillColor)
        {
            fillColorTween.OnComplete(()=>{FillHoverTimer = 0;});
        }
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
