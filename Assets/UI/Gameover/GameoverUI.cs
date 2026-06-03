using Nova;
using UnityEngine;

public class GameoverUI : MonoBehaviour
{
    const string SCORE_TEXT_PREFIX = "Score = ";

    [Header("Component and Object")]
    [SerializeField] private GameObject content;
    [SerializeField] private UIBlock2D background;
    [SerializeField] private TextBlock winText;
    [SerializeField] private TextBlock loseText;
    [SerializeField] private TextBlock scoreText;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(content, "content is missing");
        Debug.Assert(background, "background is missing");
        Debug.Assert(winText, "winText is missing");
        Debug.Assert(loseText, "loseText is missing");
        Debug.Assert(scoreText, "scoreText is missing");
        // Initialize
        CLoseLoseGameoverUI();
    }
    #endregion

    // ====================================================================================================
    //                     Gameover Methods
    // ====================================================================================================
    #region Gameover
    public void OpenWinGameoverUI()
    {
        content.gameObject.SetActive(true);
        winText.gameObject.SetActive(true);
        loseText.gameObject.SetActive(false);
        scoreText.Text = SCORE_TEXT_PREFIX + (ScoreManager.Instance.score * 2).ToString();
    }

    public void OpenLoseGameoverUI()
    {
        content.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(true);
        scoreText.Text = SCORE_TEXT_PREFIX + ScoreManager.Instance.score.ToString();
    }

    public void CLoseLoseGameoverUI()
    {
        content.gameObject.SetActive(false);
    }
    #endregion
}
