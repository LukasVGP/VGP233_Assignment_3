using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientation;
    public Transform orientation => _orientation;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    private Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.3f;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool shouldJump = false;
    private bool isRunning = false;
    private float currentMoveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.angularDamping = 10f;
        currentMoveSpeed = walkSpeed;
    }

    void Update()
    {
        CheckGround();
        GetPlayerInput();
        HandleRotation();
        UpdateAnimator();
        HandleJump();
        HandleRunning();
    }

    void FixedUpdate()
    {
        if (shouldJump)
        {
            PerformJump();
        }
        MovePlayer();
        ControlSpeed();
        ApplyDrag();
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void HandleRunning()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift) && verticalInput > 0;
        currentMoveSpeed = isRunning ? runSpeed : walkSpeed;
    }

    private void HandleRotation()
    {
        if (horizontalInput != 0)
        {
            float rotationAmount = horizontalInput * rotateSpeed;
            transform.Rotate(Vector3.up * rotationAmount);
            rb.angularVelocity = Vector3.zero;
            if (_orientation != null)
            {
                _orientation.rotation = transform.rotation;
            }
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Walk", verticalInput);
        animator.SetFloat("Strafe", horizontalInput);
        animator.SetBool("IsMoving", (Mathf.Abs(horizontalInput) > 0.0f || Mathf.Abs(verticalInput) > 0.0f));
        animator.SetBool("Run", isRunning);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            shouldJump = true;
            animator.SetTrigger("Jump");
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        shouldJump = false;
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput;
        rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10, ForceMode.Force);
    }

    private void ControlSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVelocity.magnitude > currentMoveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * currentMoveSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void ApplyDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : 0;
    }
}
