using UnityEngine;

// By B0N3head 
// All yours, use this script however you see fit, feel free to give credit if you want
public class PlayerMovement : MonoBehaviour
{
    [Header("Node References")]
    [SerializeField] private Rigidbody rb;
    // ====================================================================================================
    [Header("Movement Settings")]
    [Tooltip("Max walk speed")]
    public float walkMoveSpeed = 7.5f;
    [Tooltip("Max sprint speed")]
    public float sprintMoveSpeed = 11f;
    [Tooltip("Max jump speed")]
    public float jumpMoveSpeed = 6f;
    [Tooltip("Max crouch speed")]
    public float crouchMoveSpeed = 4f;
    // ====================================================================================================
    [Header("Crouch Settings")]
    [Tooltip("How long it takes to crouch")]
    public float crouchDownSpeed = 0.2f;
    [Tooltip("How tall the character is when they crouch")]
    public float crouchHeight = 0.68f; //change for how large you want when crouching
    [Tooltip("How tall the character is when they stand")]
    public float standingHeight = 1f;
    [Tooltip("Lerp between crouching and standing")]
    public bool smoothCrouch = true;
    [Tooltip("Can you crouch while in the air")]
    public bool jumpCrouching = true;
    // ====================================================================================================
    [Header("Jump Settings")]
    [Tooltip("Initial jump force")]
    public float jumpForce = 110f;
    [Tooltip("Continuous jump force")]
    public float jumpAcceleration = 10f;
    [Tooltip("Max jump up time")]
    public float jumpTime = 0.4f;
    [Tooltip("How long you have to jump after leaving a ledge (seconds)")]
    public float coyoteTime = 0.2f;
    [Tooltip("How long I should buffer your jump input for (seconds)")]
    public float jumpBuffer = 0.1f;
    [Tooltip("How long do I have to wait before I can jump again")]
    public float jumpCooldown = 0.6f;
    [Tooltip("Fall quicker")]
    public float extraGravity = 0.1f;
    [Tooltip("The tag that will be considered the ground")]
    public string groundTag = "Ground";
    // ====================================================================================================
    [Header("Debug Info")]
    [Tooltip("Are we on the ground?")]
    public bool isGrounded = true;
    [Tooltip("Are we crouching?")]
    public bool isCrouching = false;
    [Tooltip("The current speed I should be moving at")]
    public float currentSpeed;

    private InputHandler inputHandler;
    private Vector3 movementInput;
    private float coyoteTimeCounter, jumpBufferCounter, startJumpTime, endJumpTime;
    private bool wantingToJump = false, wantingToCrouch = false, wantingToSprint = false;
    private bool jumpCooldownOver = true;


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
        currentSpeed = walkMoveSpeed; // Set currentSpeed to walking as no keys should be pressed yet
    }

    private void Update()
    {
        // Move all input to Update(), then use given input on FixedUpdate()
        movementInput = new Vector3(inputHandler.moveInput.x, 0, inputHandler.moveInput.y); // WSAD key
        wantingToJump = InputHandler.Instance.jumpInput; // Jump key
        wantingToCrouch = InputHandler.Instance.crouchInput; // Crouch key
        wantingToSprint = InputHandler.Instance.sprintInput; // Sprint key
    }

    void FixedUpdate()
    {
        // Double check if we are on the ground or not (Changes current speed if true)
        // --- QUICK EXPLINATION --- 
        // transform.position.y - transform.localScale.y + 0.1f.
        // This puts the start of the ray 0.1f above the bottom of the player.
        // We then shoot a ray 0.15f down, this exists the player with 0.5f to hit objects.
        // Removing this +- of 0.1f and having it shoot directly under the player can skip the ground as
        // sometimes the capsules bottom clips through the ground.
        bool isHitGround = Physics.Raycast(
            new Vector3(
                transform.position.x,
                transform.position.y - transform.localScale.y + 0.1f,
                transform.position.z
            ),
            Vector3.down, 0.15f
        );
        if (isHitGround) HandleHitGround();
        // Sprinting
        if (wantingToSprint && isGrounded && !isCrouching) currentSpeed = sprintMoveSpeed;
        else if (!isCrouching && isGrounded) currentSpeed = walkMoveSpeed;
        // Crouching 
        // Can be simplified to Crouch((wantingToCrouch && jumpCrouching)).
        // Though the bellow is more readable.
        if (wantingToCrouch && jumpCrouching) Crouch(true);
        else Crouch(false);
        // Coyote timer
        // When the player leaves the ground, start counting down from the set value coyoteTime.
        // This allows players to jump late. After they have left.
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;
        // Jump buffer timer
        // When the player leaves the ground, start counting down from the set value jumpBuffer.
        // This will "buffer" the input and allow for early jump presses to be valid and no longer ignored.
        if (wantingToJump) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter -= Time.deltaTime;
        // If the coyote timer has not run out, our jump buffer has not run out, we our cool down (canJump)
        // is now over.
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && jumpCooldownOver)
        {
            // Apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            // Reset
            jumpCooldownOver = false;
            isGrounded = false;
            jumpBufferCounter = 0f;
            currentSpeed = jumpMoveSpeed;
            endJumpTime = Time.time + jumpTime;
            // Wait jumpCooldown (1f = 1 second) then run the jumpCoolDownCountdown() void
            Invoke(nameof(JumpCoolDownCountdown), jumpCooldown);
        }
        else if (wantingToJump && !isGrounded && endJumpTime > Time.time)
        {
            // Hold down space for a further jump (until the timer runs out)
            rb.AddForce(Vector3.up * jumpAcceleration, ForceMode.Acceleration);
        }
        // WSAD movement
        movementInput = movementInput.normalized;
        Vector3 forwardVel = transform.forward * currentSpeed * movementInput.z;
        Vector3 horizontalVel = transform.right * currentSpeed * movementInput.x;
        rb.linearVelocity = horizontalVel + forwardVel + new Vector3(0, rb.linearVelocity.y, 0);
        //Extra gravity for more nicer jumping
        rb.AddForce(new Vector3(0, -extraGravity, 0), ForceMode.Impulse);
    }
    #endregion

    // ====================================================================================================
    //                     Movement Functions
    // ====================================================================================================
    #region Movement
    private void JumpCoolDownCountdown() {jumpCooldownOver = true;}

    // Crouch handling
    private void Crouch(bool crouch)
    {
        isCrouching = crouch;
        if (crouch)
        {
            // If the player is crouching
            currentSpeed = crouchMoveSpeed;
            Vector3 crouchScale = new Vector3(
                transform.localScale.x, crouchHeight, transform.localScale.z
            );
            if (smoothCrouch)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    Mathf.Lerp(transform.localScale.y, crouchHeight, crouchDownSpeed),
                    transform.localScale.z
                );
                transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(
                        transform.position.x, transform.position.y - crouchHeight, transform.position.z
                    ),
                    crouchDownSpeed);
            }
            else if (transform.localScale != crouchScale)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x, crouchHeight, transform.localScale.z
                );
                transform.position = new Vector3(
                    transform.position.x, transform.position.y - crouchHeight / 2, transform.position.z
                );
            }
        }
        else
        {
            // If the player is standing
            Vector3 standScale = new Vector3(
                transform.localScale.x, standingHeight, transform.localScale.z
            );
            if (smoothCrouch)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    Mathf.Lerp(transform.localScale.y, standingHeight, crouchDownSpeed),
                    transform.localScale.z
                );
                transform.position = Vector3.Lerp(
                    transform.position, new Vector3(transform.position.x,
                    transform.position.y - standingHeight / 2, transform.position.z),
                    crouchDownSpeed
                );
            }
            else if (transform.localScale != standScale)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x, standingHeight, transform.localScale.z
                );
                transform.position = new Vector3(
                    transform.position.x, transform.position.y + standingHeight / 2, transform.position.z
                );
            }
        }
    }

    // Ground check
    //****** make sure whatever you want to be the ground in your game matches the tag set in the script
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == groundTag) HandleHitGround();
    }

    // This is separated in its own void as this code needs to be run on two separate occasions.
    // Saves copy pasting code.
    // Just double checking if we are crouching and setting the speed accordingly.
    public void HandleHitGround()
    {
        if (isCrouching) currentSpeed = crouchMoveSpeed;
        else currentSpeed = walkMoveSpeed;
        isGrounded = true;
    }

    // // Dw about understanding this, it's just the code for setting up the player character 
    // public void setupCharacter()
    // {
    //     gameObject.tag = "Player";
    //     if (!gameObject.GetComponent<Rigidbody>())
    //     {
    //         Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
    //         rb.mass = 10;
    //     }
    //     else Debug.Log("Rigidbody already exists");

    //     if (!gameObject.transform.Find("Camera"))
    //     {
    //         Vector3 old = transform.position;
    //         gameObject.transform.position = new Vector3(0, -0.8f, 0);
    //         GameObject go = new GameObject("Camera");
    //         go.AddComponent<Camera>();
    //         go.AddComponent<AudioListener>();
    //         go.transform.rotation = new Quaternion(0, 0, 0, 0);
    //         go.transform.localScale = new Vector3(1, 1, 1);
    //         go.transform.parent = transform;
    //         gameObject.transform.position = old;
    //         Debug.Log("Camera created");
    //     }
    //     else Debug.Log("Camera already exists");
    // }
    #endregion
}
