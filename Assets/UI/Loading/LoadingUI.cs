using DG.Tweening;
using Nova;
using Unity.VisualScripting;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    const string LOADING_PREFIX = "Loading";
    private readonly string[] dotArray = {".", "..", "..."};

    [Header("Component and Object")]
    [SerializeField] private GameObject content;
    [SerializeField] private UIBlock2D loadingBackground;
    [SerializeField] private TextBlock loadingText;
    [Header("Loading Settings")]
    [SerializeField] private Color backgroundFadeOpenColor = new Color(0, 0, 0, 1);
    [SerializeField] private Color backgroundFadeCloseColor = new Color(0, 0, 0, 0);
    [SerializeField] private float backgroundFadeDuration = 0.5f;
    [SerializeField] private float dotUpdateTime = 0.333f;

    private bool isLoading = false;
    private Tween backgroundFadeTween;
    private int currentDotIndex;
    private float nextDotUpdateTimer;

    // ====================================================================================================
    //                     Virtual Methods
    // ====================================================================================================
    #region Virtual

    private void Start()
    {
        // Assertion check
        Debug.Assert(content, "content is missing");
        Debug.Assert(loadingBackground, "loadingBackground is missing");
        Debug.Assert(loadingText, "loadingText is missing");
        // Initialize
        content.gameObject.SetActive(true);
        loadingBackground.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(false);
        CloseLoadingUI();
    }

    private void Update()
    {
        // Check is loading or not
        if (!isLoading) return;
        // Update loading text
        if (loadingText)
        {
            if (Time.time >= nextDotUpdateTimer)
            {
                loadingText.Text = LOADING_PREFIX + dotArray[currentDotIndex];
                currentDotIndex = (currentDotIndex + 1) % dotArray.Length;
                nextDotUpdateTimer = Time.time + dotUpdateTime;
            }
        }
    }
    #endregion

    // ====================================================================================================
    //                     Loading Methods
    // ====================================================================================================
    #region Loading
    public void OpenLoadingUI()
    {
        // Setup object and variable
        content.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(false);
        isLoading = false;
        // Do background fade
        if (!backgroundFadeTween.IsUnityNull()) backgroundFadeTween.Kill();
        loadingBackground.Color = backgroundFadeCloseColor;
        backgroundFadeTween = DOTween.To(
            ()=>loadingBackground.Color,
            x=>loadingBackground.Color = x,
            backgroundFadeOpenColor,
            backgroundFadeDuration
        );
        backgroundFadeTween.OnComplete(()=>{
            isLoading = true;
            loadingText.gameObject.SetActive(true);
        });
        // Setup loading text
        loadingText.Text = LOADING_PREFIX + dotArray[currentDotIndex];
        currentDotIndex = 0;
        nextDotUpdateTimer = Time.time + dotUpdateTime;
    }

    public void CloseLoadingUI()
    {
        // Setup object and variable
        loadingText.gameObject.SetActive(false);
        isLoading = false;
        // Do background fade
        if (!backgroundFadeTween.IsUnityNull()) backgroundFadeTween.Kill();
        loadingBackground.Color = backgroundFadeOpenColor;
        backgroundFadeTween = DOTween.To(
            ()=>loadingBackground.Color,
            x=>loadingBackground.Color = x,
            backgroundFadeCloseColor,
            backgroundFadeDuration
        );
        backgroundFadeTween.OnComplete(()=>{
            content.gameObject.SetActive(false);
        });
        // Setup loading text
        loadingText.Text = LOADING_PREFIX + dotArray[currentDotIndex];
        currentDotIndex = 0;
        nextDotUpdateTimer = Time.time + dotUpdateTime;
    }
    #endregion
}
