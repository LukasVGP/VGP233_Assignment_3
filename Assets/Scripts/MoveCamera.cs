using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("References")]
    public Transform targetToLookAt;
    public Transform orientation;

    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0, 2f, -3.5f);
    public float targetHeight = 1.5f;

    [Header("Camera Behavior")]
    public float rotationThreshold = 15f;
    public float positionSmoothTime = 0.95f;
    public float rotationSmoothTime = 0.85f;
    public float followDelay = 0.2f;
    public float minDistance = 2f; // Minimum distance to maintain from player

    private float currentAngleDifference;
    private Vector3 smoothVelocity;
    private float targetAngle;
    private float currentAngle;
    private float lastPlayerRotationTime;
    private float lastPlayerAngle;

    private void Start()
    {
        if (targetToLookAt == null)
        {
            targetToLookAt = GameObject.FindGameObjectWithTag("Player").transform;
        }
        transform.position = targetToLookAt.position + offset;
        targetAngle = targetToLookAt.eulerAngles.y;
        currentAngle = targetAngle;
        lastPlayerAngle = targetAngle;
    }

    private void LateUpdate()
    {
        // Always keep player in focus by updating look position first
        Vector3 targetLookPosition = targetToLookAt.position + Vector3.up * targetHeight;

        if (Mathf.Abs(Mathf.DeltaAngle(lastPlayerAngle, targetToLookAt.eulerAngles.y)) > 0.1f)
        {
            lastPlayerRotationTime = Time.time;
            lastPlayerAngle = targetToLookAt.eulerAngles.y;
        }

        currentAngleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetToLookAt.eulerAngles.y));

        if (currentAngleDifference > rotationThreshold && Time.time > lastPlayerRotationTime + followDelay)
        {
            targetAngle = targetToLookAt.eulerAngles.y;
        }

        // Smooth rotation while maintaining focus
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime / rotationSmoothTime);
        Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);

        // Calculate and adjust desired position
        Vector3 desiredPosition = targetToLookAt.position + (rotation * offset);
        Vector3 directionToTarget = targetLookPosition - desiredPosition;
        float distanceToTarget = directionToTarget.magnitude;

        // Ensure minimum distance is maintained
        if (distanceToTarget < minDistance)
        {
            desiredPosition = targetLookPosition - directionToTarget.normalized * minDistance;
        }

        // Update position with maintained focus
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, positionSmoothTime);

        // Always look at player
        transform.LookAt(targetLookPosition);

        if (orientation != null)
        {
            orientation.rotation = Quaternion.Euler(0, targetToLookAt.eulerAngles.y, 0);
        }
    }
}
