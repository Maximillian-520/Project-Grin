using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is a template for main menu controller.
/// Used for managing main menu flow and events.
/// Please rewrite this script as needed!
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private GameObject mainContent;
    [SerializeField] private GameObject settingsContent;
    [SerializeField] private GameObject creditsContent;
    [Header("Main Menu")]
    [SerializeField] private string playLoadSceneName = "GameScene";
    [SerializeField] private string mainMenuMusicName = "MainMenu";

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(mainContent, "mainContent is missing");
        Debug.Assert(settingsContent, "settingsContent is missing");
        Debug.Assert(creditsContent, "creditsContent is missing");
        // Initialize
        mainContent.SetActive(true);
        settingsContent.SetActive(false);
        creditsContent.SetActive(false);
    }
    #endregion

    // ====================================================================================================
    //                     Button Functions
    // ====================================================================================================
    #region Button
    // Button pressed event function, used to load game scene
    public void OnPlay()
    {
        SceneManager.LoadScene(playLoadSceneName);
    }

    // Button pressed event function, used to open settings content
    public void OnSettings()
    {
        mainContent.SetActive(false);
        settingsContent.SetActive(true);
        creditsContent.SetActive(false);
    }

    // Button pressed event function, used to open credits content
    public void OnCredits()
    {
        mainContent.SetActive(false);
        settingsContent.SetActive(false);
        creditsContent.SetActive(true);
    }

    // Button pressed event function, used to quit game
    public void OnQuit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }

    // Button pressed event function, used to close all opened content
    public void OnBack()
    {
        mainContent.SetActive(true);
        settingsContent.SetActive(false);
        creditsContent.SetActive(false);
    }
    #endregion
}
