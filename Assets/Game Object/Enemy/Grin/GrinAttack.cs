using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GrinAttack : MonoBehaviour
{
    public UnityEvent AttackFinished;

    [Header("Component and Object")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GrinVisual grinVisual;
    [SerializeField] private Transform slashHitPosition;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private GameObject muzzleFlashEffect;
    [Header("Slash Attack Data")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float maxDashDistanceToPlayer = 0.5f;
    [SerializeField] private float slashRange = 10f;
    [SerializeField] private int slashDamage = 40;
    [Header("Shoot Attack Data")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float beforeShootDelay = 0.5f;
    [SerializeField] private float shootDuration = 1.5f;
    [SerializeField] private float shootRate = 8;
    [SerializeField] private float shootMaxSpread = 9f;
    [SerializeField] private int shootDamage = 10;

    private Coroutine currentCoroutine;
    private bool isSlashAnimationFinished = false;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(rb, "rb is missing");
        Debug.Assert(grinVisual, "grinVisual is missing");
        Debug.Assert(slashHitPosition, "slashHitPosition is missing");
        Debug.Assert(bulletSpawnPosition, "bulletSpawnPosition is missing");
        Debug.Assert(muzzleFlashEffect, "muzzleFlashEffect is missing");
        Debug.Assert(bulletPrefab, "bulletPrefab is empty");
        // Connect event
        grinVisual.AnimationFinished.AddListener(OnAnimationFinished);
        grinVisual.SlashHitTriggered.AddListener(OnSlashHitTriggered);
        // Initialize
        muzzleFlashEffect.SetActive(false);
    }
    #endregion

    // ====================================================================================================
    //                     Attack Functions
    // ====================================================================================================
    #region Attack
    public void SlashAttack()
    {
        isSlashAnimationFinished = false;
        currentCoroutine = StartCoroutine(SlashSequence());
    }

    public void ShootAttack()
    {
        currentCoroutine = StartCoroutine(ShootSequence());
    }

    public void StopAttack()
    {
        if (!currentCoroutine.IsUnityNull())
        {
            StopCoroutine(currentCoroutine);
            rb.linearVelocity = Vector2.zero;
            grinVisual.DoIdle();
            muzzleFlashEffect.SetActive(false);
        }
    }

    private IEnumerator SlashSequence()
    {
        // Dash to player
        grinVisual.DoDash();
        float distanceToPlayer;
        yield return new WaitUntil(() =>
        {
            // Get direction to player
            Vector3 direction = MathUtility.GetDirection(
                transform.position, Player.Instance.transform.position
            );
            direction = new Vector3(direction.x, 0, direction.z);
            // Update speed and rotation
            rb.linearVelocity = direction * dashSpeed;
            transform.LookAt(Player.Instance.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            // Update distance
            Vector3 fromPosition = transform.position;
            fromPosition = new Vector3(fromPosition.x, 0, fromPosition.z);
            Vector3 toPosition = Player.Instance.transform.position;
            toPosition = new Vector3(toPosition.x, 0, toPosition.z);
            distanceToPlayer = MathUtility.GetDistance(fromPosition, toPosition);
            // Return condition
            return distanceToPlayer <= maxDashDistanceToPlayer;
        });
        rb.linearVelocity = Vector3.zero;
        // Slash player
        grinVisual.DoSlash();
        yield return new WaitUntil(() =>
        {
            rb.linearVelocity = Vector3.zero;
            return isSlashAnimationFinished;
        });
        // Attack finished
        grinVisual.DoIdle();
        currentCoroutine = null;
        AttackFinished?.Invoke();
    }

    private IEnumerator ShootSequence()
    {
        // Do shoot pose and wait
        grinVisual.DoShoot();
        float shootDelayTime = Time.time + beforeShootDelay;
        yield return new WaitUntil(() =>
        {
            // Update velocity and rotation
            Vector3 direction = MathUtility.GetDirection(
                transform.position, Player.Instance.transform.position
            );
            direction = new Vector3(direction.x, 0, direction.z);
            rb.linearVelocity = Vector3.zero;
            transform.LookAt(Player.Instance.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            // Return condition
            return Time.time >= shootDelayTime;
        });
        // Shoot at player
        int shootAmount = Mathf.RoundToInt(shootDuration * shootRate);
        int currentShootAmount = 0;
        float nextShootTime = -1.0f;
        muzzleFlashEffect.SetActive(true);
        yield return new WaitUntil(() =>
        {
            // Update shooting
            if (Time.time >= nextShootTime)
            {
                Bullet bulletInstance = Instantiate(
                    bulletPrefab,
                    bulletSpawnPosition.transform.position,
                    bulletSpawnPosition.transform.rotation
                );
                SetSpreadDirection(bulletInstance.transform, bulletSpawnPosition.transform);
                bulletInstance.damage = shootDamage;
                nextShootTime = Time.time + (1.0f / shootRate);
                currentShootAmount++;
            }
            // Update velocity and rotation
            Vector3 direction = MathUtility.GetDirection(
                transform.position, Player.Instance.transform.position
            );
            direction = new Vector3(direction.x, 0, direction.z);
            rb.linearVelocity = Vector3.zero;
            transform.LookAt(Player.Instance.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            // Return condition
            return currentShootAmount >= shootAmount;
        });
        muzzleFlashEffect.SetActive(false);
        // Attack finished
        grinVisual.DoIdle();
        currentCoroutine = null;
        AttackFinished?.Invoke();
    }
    #endregion

    // ====================================================================================================
    //                     Event Functions
    // ====================================================================================================
    #region Event
    private void OnAnimationFinished(string animationName)
    {
        if (animationName == grinVisual.slashAnimationName) isSlashAnimationFinished = true;
    }

    private void OnSlashHitTriggered()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(
            slashHitPosition.position, slashHitPosition.forward, out hit, slashRange
        );
        if (isHit)
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (!damageable.IsUnityNull())
            {
                damageable.ReceiveDamage(slashDamage);
            }
        }
    }
    #endregion

    // ====================================================================================================
    //                     Helper Functions
    // ====================================================================================================
    #region Helper
    private void SetSpreadDirection(Transform bulletTransform, Transform spawnTransform)
    {
        // Set constant
        const float SPREAD_LOOK_DISTANCE = 10f;
        // Calculate offset
        float offsetMagnitude = Random.Range(0f, shootMaxSpread * Mathf.Deg2Rad);
        Vector2 spreadDirection = Random.insideUnitCircle.normalized;
        Vector3 offsetDirectionX = spawnTransform.right * spreadDirection.x * offsetMagnitude;
        Vector3 offsetDirectionY = spawnTransform.up * spreadDirection.y * offsetMagnitude;
        Vector3 offset = offsetDirectionX + offsetDirectionY;
        // Get look position
        Vector3 lookPosition = spawnTransform.position;
        lookPosition += spawnTransform.forward.normalized * SPREAD_LOOK_DISTANCE;
        lookPosition += offset;
        // Set bullet rotation
        bulletTransform.LookAt(lookPosition);
    }
    #endregion
}
