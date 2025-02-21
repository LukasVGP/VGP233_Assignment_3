using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientation;
    public Transform orientation => _orientation;
    [SerializeField] private Animator animator;
    private Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundMask;

    [Header("Gravity Settings")]
    [SerializeField] private float fallMultiplier = 2.5f;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool shouldJump = false;
    private bool isRunning = false;
    private float currentMoveSpeed;
    private bool isStunned = false;
    private float stunTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.angularDamping = 10f;
        currentMoveSpeed = walkSpeed;
    }

    void Update()
    {
        if (isStunned) return;

        CheckGround();
        GetPlayerInput();
        HandleRotation();
        UpdateAnimator();
        HandleJump();
        HandleRunning();
    }

    void FixedUpdate()
    {
        if (isStunned) return;

        if (shouldJump)
        {
            PerformJump();
        }
        MovePlayer();
        ControlSpeed();
        ApplyDrag();
        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.down * fallMultiplier, ForceMode.Acceleration);
        }
    }

    public void HitPlayer(Vector3 force, float stunDuration)
    {
        if (!isStunned)
        {
            rb.AddForce(force, ForceMode.Impulse);
            isStunned = true;
            stunTimer = stunDuration;
            StartCoroutine(StunRoutine());
        }
    }

    private IEnumerator StunRoutine()
    {
        while (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            yield return null;
        }
        isStunned = false;
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundMask);
        Debug.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckRadius, isGrounded ? Color.green : Color.red);
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
        if (animator != null)
        {
            animator.SetFloat("Walk", verticalInput);
            animator.SetFloat("Strafe", horizontalInput);
            animator.SetBool("IsMoving", (Mathf.Abs(horizontalInput) > 0.0f || Mathf.Abs(verticalInput) > 0.0f));
            animator.SetBool("Run", isRunning);
            animator.SetBool("IsGrounded", isGrounded);
        }
    }



    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            shouldJump = true;
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce * rb.mass, ForceMode.Impulse);
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
