using UnityEngine;

public class Grin : MonoBehaviour, IDamageable
{
    [Header("Component and Object")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider colliderBody;
    [SerializeField] private GrinAttack grinAttack;
    [SerializeField] private GrinVisual grinVisual;
    [Header("Enemy Data")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float idleTime = 3.0f;
    [Tooltip("Distance to player that determine the next attack")]
    [SerializeField] private float attackDistanceThreshold = 10f;

    public float currentHealth {private set; get;}
    private bool isAlive = true;
    private float idleTimer;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(rb, "rb is missing");
        Debug.Assert(colliderBody, "colliderBody is missing");
        Debug.Assert(grinAttack, "grinAttack is missing");
        Debug.Assert(grinVisual, "grinVisual is missing");
        // Connect event
        grinAttack.AttackFinished.AddListener(()=>{idleTimer = idleTime;});
        // Initialize
        currentHealth = maxHealth;
        idleTimer = idleTime;
    }

    private void Update()
    {
        // Check is alive
        if (!isAlive) return;
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
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        if (currentHealth <= 0) DoDie();
    }
    #endregion

    // ====================================================================================================
    //                     Enemy Functions
    // ====================================================================================================
    #region Enemy
    private void DoAttack()
    {
        float distanceToPlayer = MathUtility.GetDistance(
            transform.position, Player.Instance.transform.position
        );
        if (distanceToPlayer < attackDistanceThreshold) grinAttack.ShootAttack();
        else grinAttack.SlashAttack();
    }

    private void DoDie()
    {
        colliderBody.enabled = false;
        grinAttack.StopAttack();
        grinVisual.DoDie();
        isAlive = false;
    }
    #endregion
}
