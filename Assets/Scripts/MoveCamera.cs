using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("References")]
    public Transform targetToLookAt;
    public Transform orientation;

    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0, 2f, -3.5f); // Closer zoom for 50% bigger view
    public float targetHeight = 1.5f;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.95f; // Much slower position following
    public float rotationSmoothTime = 0.85f; // Much slower rotation

    private Vector3 smoothVelocity = Vector3.zero;
    private Quaternion currentRotation;

    private void Start()
    {
        if (targetToLookAt == null)
        {
            targetToLookAt = GameObject.FindGameObjectWithTag("Player").transform;
        }
        transform.position = targetToLookAt.position + offset;
        currentRotation = targetToLookAt.rotation;
    }

    private void LateUpdate()
    {
        // Calculate target positions
        Vector3 targetLookPosition = targetToLookAt.position + Vector3.up * targetHeight;
        Vector3 desiredPosition = targetToLookAt.position + targetToLookAt.rotation * offset;

        // Smooth position following
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref smoothVelocity,
            positionSmoothTime
        );

        // Smooth rotation following
        Vector3 lookDirection = targetLookPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime / rotationSmoothTime
        );

        // Update orientation
        if (orientation != null)
        {
            orientation.rotation = Quaternion.Euler(0, targetToLookAt.rotation.eulerAngles.y, 0);
        }
    }
}
