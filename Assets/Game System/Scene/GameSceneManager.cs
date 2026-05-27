using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public UnityEvent LoadSceneFinsihed;

    public static GameSceneManager Instance {private set; get;}

    [Header("Loading Settings")]
    [SerializeField] private float loadingStartDelay = 0.5f;
    [SerializeField] private float loadingFinishDelay = 0.5f;
    public float loadingProgress;

    private string nextLoadSceneName;
    private AsyncOperation loadingOperation;

    // ====================================================================================================
    //                     Virtual Methods
    // ====================================================================================================
    #region Virtual
    private void Awake()
    {
        if (Instance) Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    // ====================================================================================================
    //                     Scene Methods
    // ====================================================================================================
    #region Scene
    public void LoadScene(string sceneName)
    {
        loadingProgress = 0;
        nextLoadSceneName = sceneName;
        Invoke("StartLoadNextScene", loadingStartDelay);
    }
    
    private void StartLoadNextScene() {StartCoroutine(LoadSceneSequence(nextLoadSceneName));}

    private void FinishLoadNextScene()
    {
        LoadSceneFinsihed?.Invoke();
        loadingOperation.allowSceneActivation = true;
    }

    private IEnumerator LoadSceneSequence(string sceneName)
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;
        yield return new WaitWhile(() =>
        {
            loadingProgress = loadingOperation.progress / 0.9f;
            return loadingProgress < 1.0f;
        });
        loadingProgress = 1.0f;
        Invoke("FinishLoadNextScene", loadingFinishDelay);
    }
    #endregion
}
