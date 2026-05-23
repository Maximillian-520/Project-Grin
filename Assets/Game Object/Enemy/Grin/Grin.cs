using UnityEngine;

public class Grin : MonoBehaviour, IDamageable
{
    [Header("Component and Object")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GrinAttack grinAttack;
    [Header("Behavior Data")]
    [SerializeField] private float idleTime = 3.0f;
    [Tooltip("Distance to player that determine the next attack")]
    [SerializeField] private float attackDistanceThreshold = 10f;

    private float idleTimer;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(rb, "rb is missing");
        Debug.Assert(grinAttack, "grinAttack is missing");
        // Connect event
        grinAttack.AttackFinished.AddListener(()=>{idleTimer = idleTime;});
        // Initialize
        idleTimer = idleTime;
    }

    private void Update()
    {
        // Update idle timer
        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0) DoAttack();
        }
    }

    private void FixedUpdate()
    {
        // Update idle physics
        if (idleTimer > 0)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
    #endregion

    // ====================================================================================================
    //                     Damageable Functions
    // ====================================================================================================
    #region Damageable
    public void ReceiveDamage(int damageAmount)
    {
        Debug.Log("enemy damaged");
    }
    #endregion

    // ====================================================================================================
    //                     Behavior Functions
    // ====================================================================================================
    #region Behavior
    private void DoAttack()
    {
        float distanceToPlayer = MathUtility.GetDistance(
            transform.position, Player.Instance.transform.position
        );
        if (distanceToPlayer < attackDistanceThreshold) grinAttack.ShootAttack();
        else grinAttack.SlashAttack();
    }
    #endregion
}
