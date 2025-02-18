using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform orientation;
    [SerializeField] private float smoothFactor = 10.0f;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Quaternion lookOffset = Quaternion.identity;

    private void Awake()
    {
        ValidateReferences();
    }

    private void ValidateReferences()
    {
        if (orientation == null)
        {
            Debug.LogError("Orientation reference is required for ThirdPersonCamera!");
        }
    }

    private void Update()
    {
        if (orientation == null) return;

        UpdateCameraPosition();
        UpdateCameraRotation();
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition = orientation.position +
            (orientation.right * offset.x) +
            (orientation.up * offset.y) +
            (orientation.forward * offset.z);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothFactor * Time.deltaTime
        );
    }

    private void UpdateCameraRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(orientation.forward) * lookOffset;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            smoothFactor * Time.deltaTime
        );
    }
}
