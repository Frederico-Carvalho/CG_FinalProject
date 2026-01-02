using UnityEditor.Build;
using UnityEngine;

public class animateStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isBoostingHash;

    // Ground Check
    [SerializeField] float groundCheckDistance = 0.3f;
    [SerializeField] LayerMask groundLayer;
    int isGroundedHash;
    int isJumpingHash;

    Rigidbody rb;
    [SerializeField] float maxSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        isJumpingHash = Animator.StringToHash("IsJumping");
        isBoostingHash = Animator.StringToHash("IsBoosting");
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isBoosting = animator.GetBool(isBoostingHash);
        bool forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // defines the current speed based on the rigidbody's velocity
        float currentSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        bool isRunningFullSpeed  = currentSpeed >= 10f;
        bool isBoostingFullSpeed = currentSpeed >= maxSpeed * 0.8f;

        // RUN
        animator.SetBool(isRunningHash, isRunningFullSpeed && isGrounded);

        //BOOST
        animator.SetBool(isBoostingHash, isBoostingFullSpeed && isGrounded);

        // WALK (true if any movement key pressed)
        animator.SetBool(isWalkingHash, forwardPressed && isGrounded);

        // GROUND
        animator.SetBool(isGroundedHash, isGrounded);

        // JUMP
        animator.SetBool(isJumpingHash, !isGrounded);

    }
}
