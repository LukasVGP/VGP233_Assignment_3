using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    private Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool shouldJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.angularDamping = 10f;
        GameController.Instance.SetPlayerStartPosition(gameObject);
    }

    void Update()
    {
        GetPlayerInput();
        HandleRotation();
        UpdateAnimator();
        HandleJump();
    }

    void FixedUpdate()
    {
        if (shouldJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce * 2f, ForceMode.Impulse);
            Debug.Log($"Jump force applied: {jumpForce}");
            shouldJump = false;
        }

        MovePlayer();
        ControlSpeed();
        ApplyDrag();
    }

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void HandleRotation()
    {
        if (horizontalInput != 0)
        {
            float rotationTorque = horizontalInput * rotateSpeed;
            rb.AddTorque(Vector3.up * rotationTorque, ForceMode.Acceleration);

            if (orientation != null)
            {
                orientation.rotation = transform.rotation;
            }
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Walk", verticalInput);
        animator.SetFloat("Strafe", horizontalInput);
        animator.SetBool("IsMoving", (Mathf.Abs(horizontalInput) > 0.0f || Mathf.Abs(verticalInput) > 0.0f));
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            shouldJump = true;
            Debug.Log("Jump requested!");
        }
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
    }

    private void ControlSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void ApplyDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Ground Contact Made");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Left Ground");
        }
    }
}
