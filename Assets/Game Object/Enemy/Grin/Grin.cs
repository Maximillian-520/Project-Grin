using UnityEngine;
using UnityEngine.Events;

public class Grin : MonoBehaviour, IDamageable
{
    public UnityEvent EnemyDied;

    [Header("Component and Object")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider colliderBody;
    [SerializeField] private GrinAttack grinAttack;
    [SerializeField] private GrinVisual grinVisual;
    [SerializeField] private BarUI healthBarUI;
    [Header("Enemy Data")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float idleTime = 3.0f;
    [Tooltip("Distance to player that determine the next attack")]
    [SerializeField] private float attackDistanceThreshold = 10f;

    private bool isEnabled = false;
    public float currentHealth {private set; get;}
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
        Debug.Assert(healthBarUI, "healthBarUI is missing");
        // Connect event
        grinAttack.AttackFinished.AddListener(()=>{idleTimer = idleTime;});
        // Initialize
        currentHealth = maxHealth;
        idleTimer = idleTime;
        healthBarUI.UpdateBar(1.0f);
    }

    private void Update()
    {
        // Check is alive
        if (!isEnabled) return;
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
        healthBarUI.UpdateBar((float)currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            EnemyDied?.Invoke();
            DoDie();
        }
    }
    #endregion

    // ====================================================================================================
    //                     Enemy Functions
    // ====================================================================================================
    #region Enemy
    public void EnableEnemy() {isEnabled = true;}

    public void DisableEnemy() {isEnabled = false;}

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
        isEnabled = false;
    }
    #endregion
}
