using UnityEngine;

/// <summary>
/// Independent audio source player instance.
/// Used to do one shot audio play with higher control.
/// </summary>

public class AudioInstance : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private AudioSource audioSource;

    private bool isStarted = false;

    // ====================================================================================================
    //                     Virtual Methods
    // ====================================================================================================
    #region Virtual
    private void Update()
    {
        // Check has started
        if (!isStarted) return;
        // Check is still playing
        if (!audioSource.isPlaying) StopAudio();
    }
    #endregion

    // ====================================================================================================
    //                     Audio Methods
    // ====================================================================================================
    #region Audio
    public void SetAudio(AudioClip audioClip, float volume, float pitch)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
    }

    public void StartAudio()
    {
        audioSource.Play();
        isStarted = true;
    }

    public void StopAudio()
    {
        audioSource.Stop();
        Destroy(gameObject);
    }
    #endregion
}
