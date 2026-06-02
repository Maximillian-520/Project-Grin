using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private Player player;
    [SerializeField] private Grin grin;
    [SerializeField] private GunDropController gunDropController;
    [SerializeField] private GameoverUI gameoverUI;
    [SerializeField] private LoadingUI loadingUI;
    [Header("Game Settings")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string menuSceneName = "MenuScene";
    [SerializeField] private float gameoverDelayTime = 5.0f;
    [Header("Audio Settings")]
    [SerializeField] private string mainMusicAudioName = "Main Music";
    [SerializeField] private float musicFadeTime = 0.5f;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(player, "player is missing");
        Debug.Assert(grin, "grin is missing");
        Debug.Assert(gunDropController, "gunDropController is missing");
        Debug.Assert(gameoverUI, "gameoverUI is missing");
        // Connect event
        player.PlayerDied.AddListener(()=>
        {
            grin.DisableEnemy();
            gunDropController.enabled = false;
            Invoke("OpenLoseScreen", gameoverDelayTime);
        });
        grin.EnemyDied.AddListener(()=>
        {
            player.DisablePlayer();
            gunDropController.enabled = false;
            Invoke("OpenWinScreen", gameoverDelayTime);
        });
        // Initialize
        grin.EnableEnemy();
        AudioManager.Instance.PlayMusic(mainMusicAudioName);
    }
    #endregion

    // ====================================================================================================
    //                     Game Functions
    // ====================================================================================================
    #region Game
    public void RestartGame()
    {
        GameSceneManager.Instance.LoadScene(gameSceneName);
        loadingUI.OpenLoadingUI();
    }

    public void BackToMenu()
    {
        GameSceneManager.Instance.LoadScene(menuSceneName);
        loadingUI.OpenLoadingUI();
        AudioManager.Instance.StopMusic(musicFadeTime);
    }

    private void OpenWinScreen() {gameoverUI.OpenWinGameoverUI();}
    private void OpenLoseScreen() {gameoverUI.OpenLoseGameoverUI();}
    #endregion
}
