using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private Rigidbody rb;
    [Header("Attack Data")]
    [SerializeField] private float speed = 10f;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(rb, "rb is missing");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = rb.transform.forward.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.transform.GetComponent<IDamageable>();
        if (!damageable.IsUnityNull()) damageable.ReceiveDamage(0);
        Destroy(gameObject);
    }
    #endregion
}
