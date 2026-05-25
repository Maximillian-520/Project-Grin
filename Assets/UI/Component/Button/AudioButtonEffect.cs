using Nova;
using UnityEngine;

public class AudioButtonEffect : BaseButtonEffect
{
    [Header("Audio Effect")]
    [SerializeField] private string normalSFXName = "";
    [SerializeField] private string hoverSFXName = "";
    [SerializeField] private string pressedSFXName = "";

    // ====================================================================================================
    //                     Interactable Functions
    // ====================================================================================================
    #region Interactable
    override public void ButtonNormal(Gesture.OnUnhover evt)
    {
        if (normalSFXName != ""){AudioManager.Instance.PlaySFX(normalSFXName);}
    }

    override public void ButtonHover(Gesture.OnHover evt)
    {
        if (hoverSFXName != ""){AudioManager.Instance.PlaySFX(hoverSFXName);}
    }

    override public void ButtonPressed(Gesture.OnPress evt)
    {
        if (pressedSFXName != ""){AudioManager.Instance.PlaySFX(pressedSFXName);}
    }
    #endregion
}
