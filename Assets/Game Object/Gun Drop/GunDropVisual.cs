using UnityEngine;

public class GunDropVisual : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private GameObject gunDisplay;
    [Header("Visual Settings")]
    [SerializeField] private float spinSpeed = 45f;
    [SerializeField] private bool isReversed = false;
    [SerializeField] private float flickerTick = 0.2f;

    public bool isFlickering = false;
    private float nextFlickerTime;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(gunDisplay, "gunDisplay is missing");
        // Initialize
        gunDisplay.transform.localEulerAngles = new Vector3(
            gunDisplay.transform.localEulerAngles.x,
            Random.Range(0, 360),
            gunDisplay.transform.localEulerAngles.z
        );
    }

    private void FixedUpdate()
    {
        // Update gun display spin
        float spinVelocity = spinSpeed * Time.fixedDeltaTime;
        if (isReversed) spinVelocity *= -1;
        gunDisplay.transform.localEulerAngles = new Vector3(
            gunDisplay.transform.localEulerAngles.x,
            gunDisplay.transform.localEulerAngles.y + spinVelocity,
            gunDisplay.transform.localEulerAngles.z
        );
        // Update flicker
        if (isFlickering && Time.time >= nextFlickerTime)
        {
            gunDisplay.SetActive(!gunDisplay.activeSelf);
            nextFlickerTime = Time.time + flickerTick;
        }
    }
    #endregion
}
