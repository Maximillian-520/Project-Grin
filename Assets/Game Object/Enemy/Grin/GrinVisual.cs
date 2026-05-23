using UnityEngine;
using UnityEngine.Events;

public class GrinVisual : MonoBehaviour
{
    public UnityEvent<string> AnimationFinished;

    [Header("Component and Object")]
    [SerializeField] private Animator animator;
    [Header("Animation Data")]
    [SerializeField] public string idleAnimationName {private set; get;} = "Idle";
    [SerializeField] public string dashAnimationName {private set; get;} = "Dash";
    [SerializeField] public string slashAnimationName {private set; get;} = "Slash";
    [SerializeField] public string shootAnimationName {private set; get;} = "Shoot";

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(animator, "animator is missing");
    }
    #endregion

    // ====================================================================================================
    //                     Animation Functions
    // ====================================================================================================
    #region Animation
    public void DoIdle() {animator.Play(idleAnimationName);}

    public void DoDash() {animator.Play(dashAnimationName);}

    public void DoSlash() {animator.Play(slashAnimationName);}

    public void DoShoot() {animator.Play(shootAnimationName);}

    public void TriggerAnimationFinish(string animationName) {AnimationFinished?.Invoke(animationName);}
    #endregion
}
