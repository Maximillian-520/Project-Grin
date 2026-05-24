using Nova;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private UIBlock2D fillUIBlock;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(fillUIBlock, "fillUIBlock is missing");
    }
    #endregion

    // ====================================================================================================
    //                     Bar Functions
    // ====================================================================================================
    #region Bar
    public void UpdateBar(float normalizedValue)
    {
        fillUIBlock.Size.Percent = new Vector3(
            normalizedValue,
            fillUIBlock.Size.Percent.y,
            fillUIBlock.Size.Percent.z
        );
    }
    #endregion
}
