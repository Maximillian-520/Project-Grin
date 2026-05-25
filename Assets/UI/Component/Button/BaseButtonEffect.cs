using Nova;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class BaseButtonEffect : MonoBehaviour
{
    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        UIBlock uiBlock = GetComponent<Interactable>().UIBlock;
        uiBlock.AddGestureHandler<Gesture.OnUnhover>(ButtonNormal);
        uiBlock.AddGestureHandler<Gesture.OnHover>(ButtonHover);
        uiBlock.AddGestureHandler<Gesture.OnPress>(ButtonPressed);
        ResetButton();
    }

    private void OnEnable() {ResetButton();}

    private void OnDisable() {ResetButton();}
    #endregion
    // ====================================================================================================
    //                     Interactable Functions
    // ====================================================================================================
    #region Interactable
    virtual public void ResetButton(){}

    virtual public void ButtonNormal(Gesture.OnUnhover evt) {}

    virtual public void ButtonHover(Gesture.OnHover evt) {}

    virtual public void ButtonPressed(Gesture.OnPress evt) {}
    #endregion
}
