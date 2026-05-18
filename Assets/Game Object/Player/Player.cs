using UnityEngine;

public class Player : MonoBehaviour
{
    // [Header("Node References")]
    // [SerializeField] private Rigidbody rb;
    // [Header("Movement Data")]
    // public float movementSpeed = 5.0f;

    // private float verticalRotation;

    // // ====================================================================================================
    // //                     Virtual Functions
    // // ====================================================================================================
    // #region Virtual
    // private void Start()
    // {
    //     // Assertion check
    //     Assert.IsNotNull(rb, "rb is missing");
    //     // Initialize
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }

    // private void FixedUpdate()
    // {
    //     // Get variables
    //     Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
    //     Vector3 rightDirection = transform.TransformDirection(Vector3.right);
    //     Vector3 movementInput = GetMovementInput();
    //     // Calculate velocity
    //     Vector3 velocity = forwardDirection * movementInput.z;
    //     velocity += rightDirection * movementInput.x;
    //     velocity *= movementSpeed;
    //     // Apply movement
    //     rb.linearVelocity = velocity;
    // }
    // #endregion

    // private Vector3 GetMovementInput()
    // {
    //     Vector3 movementInput = Vector3.zero;
    //     if (Input.GetKey(KeyCode.W)) movementInput.z += 1;
    //     if (Input.GetKey(KeyCode.A)) movementInput.x -= 1;
    //     if (Input.GetKey(KeyCode.S)) movementInput.z -= 1;
    //     if (Input.GetKey(KeyCode.D)) movementInput.x += 1;
    //     return movementInput;
    // }
}
