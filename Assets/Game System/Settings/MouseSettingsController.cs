using UnityEngine;
using NovaSamples.UIControls;

public class MouseSettingsController : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Toggle mouseInverseToggle;

    static public float mouseSensitivity = 1.0f;
    static public bool isMouseInverse = false;

    // ====================================================================================================
    //                     Virtual Methods
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Connect UI events
        if (mouseSensitivitySlider) mouseSensitivitySlider.OnValueChanged.AddListener(
            value =>
            {
                mouseSensitivity = value;
                SaveSettings();
            }
        );
        if (mouseInverseToggle) mouseInverseToggle.OnToggled.AddListener(
            value =>
            {
                isMouseInverse = value;
                SaveSettings();
            }
        );
        // Initialize
        LoadSettings();
    }
    #endregion

    // ====================================================================================================
    //                     Settings Methods
    // ====================================================================================================
    #region Settings
    private void ApplySettings() {}

    public void SaveSettings()
    {
        // Save settings
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetInt("MouseInverse", isMouseInverse ? 1 : 0);
        PlayerPrefs.Save();
        // Apply new saved changes
        ApplySettings();
    }

    public void LoadSettings()
    {
        // Load settings
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        isMouseInverse = PlayerPrefs.GetInt("MouseInverse", 0) == 1;
        // Apply settings
        ApplySettings();
        // Set UI values
        if(mouseSensitivitySlider) mouseSensitivitySlider.Value = mouseSensitivity;
        if(mouseInverseToggle) mouseInverseToggle.ToggledOn = isMouseInverse;
    }
    #endregion
}
