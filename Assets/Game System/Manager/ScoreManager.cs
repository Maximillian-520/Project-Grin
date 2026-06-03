using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {private set; get;}

    [Header("Score Settings")]
    [SerializeField] private float countingTickTime = 1.0f;

    public int score;
    private bool isCounting = false;
    private float nextCountingTick;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Awake() {Instance = this;}

    private void OnDestroy() {Instance = null;}

    private void Update() {if (Time.time >= nextCountingTick) AddScore();}
    #endregion

    // ====================================================================================================
    //                     Score Functions
    // ====================================================================================================
    #region Score
    public void StartScoreCounting()
    {
        score = 0;
        isCounting = true;
        nextCountingTick = Time.time + countingTickTime;
    }

    public void StopScoreCounting() {isCounting = true;}

    private void AddScore()
    {
        score++;
        nextCountingTick = Time.time + countingTickTime;
    }
    #endregion
}
