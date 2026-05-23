using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component and Object")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundCheck groundCheck;
    [Header("Movement Data")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 6f;
    public float acceleration = 24f;
    public float jumpForce = 7f;

    private InputHandler inputHandler;
    private Vector3 currentDirection;
    private float currentSpeed;

    // ====================================================================================================
    //                     Virtual Functions
    // ====================================================================================================
    #region Virtual
    private void Start()
    {
        // Assertion check
        Debug.Assert(rb, "rb is missing");
        // Initialize
        inputHandler = InputHandler.Instance;
    }

    void FixedUpdate()
    {
        HandleMove();
        HandleJump();
        HandleSpeedChange();
    }
    #endregion

    // ====================================================================================================
    //                     Movement Functions
    // ====================================================================================================
    #region Movement
    private void HandleMove()
    {
        if (inputHandler.moveInput != Vector2.zero)
        {
            Vector3 newDirection = transform.right * inputHandler.moveInput.x;
            newDirection += transform.forward * inputHandler.moveInput.y;
            currentDirection = newDirection.normalized;
        }
        Vector3 velocity = currentDirection * currentSpeed;
        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    private void HandleJump()
    {
        if (Cursor.lockState != CursorLockMode.Locked) {}
        else if (inputHandler.jumpInput && groundCheck.isOnGround)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    private void HandleSpeedChange()
    {
        float targetSpeed;
        if (Cursor.lockState != CursorLockMode.Locked) targetSpeed = 0;
        else if (inputHandler.moveInput == Vector2.zero) targetSpeed = 0;
        else if (inputHandler.sprintInput) targetSpeed = sprintSpeed;
        else targetSpeed = walkSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
    }
    #endregion
}
