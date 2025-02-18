using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("References")]
    public Transform targetToLookAt;
    public Transform orientation;

    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0, 5, -10);

    [Header("Camera Controls")]
    public float mouseSensitivity = 2f;
    public float returnSpeed = 2f;
    public float maxHorizontalAngle = 45f;
    public float maxVerticalAngle = 30f;
    public float smoothTime = 0.1f;

    private float xRotation;
    private float yRotation;
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private bool isMouseMoving;
    private Vector2 lastMousePosition;

    private void Start()
    {
        if (targetToLookAt == null)
        {
            targetToLookAt = GameObject.FindGameObjectWithTag("Player").transform;
        }
        lastMousePosition = Input.mousePosition;
        transform.position = targetToLookAt.position + offset;
    }

    private void Update()
    {
        HandleMouseInput();
        UpdateCameraPosition();
        ApplyRotation();
    }

    private void HandleMouseInput()
    {
        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 mouseDelta = currentMousePosition - lastMousePosition;

        isMouseMoving = mouseDelta.magnitude > 0.1f;

        if (isMouseMoving)
        {
            float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;

            yRotation = Mathf.Clamp(yRotation, -maxHorizontalAngle, maxHorizontalAngle);
            xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);
        }
        else
        {
            yRotation = Mathf.Lerp(yRotation, 0, Time.deltaTime * returnSpeed);
            xRotation = Mathf.Lerp(xRotation, 0, Time.deltaTime * returnSpeed);
        }

        lastMousePosition = currentMousePosition;
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentRotation);
        Vector3 targetPosition = targetToLookAt.position + rotation * offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref smoothVelocity,
            smoothTime
        );
    }

    private void ApplyRotation()
    {
        currentRotation = Vector3.Lerp(
            currentRotation,
            new Vector3(xRotation, yRotation, 0),
            Time.deltaTime * returnSpeed
        );

        transform.rotation = Quaternion.Euler(currentRotation);
        orientation.rotation = Quaternion.Euler(0, currentRotation.y, 0);
        transform.LookAt(targetToLookAt);
    }

    public void ResetCamera()
    {
        xRotation = 0f;
        yRotation = 0f;
        currentRotation = Vector3.zero;
        smoothVelocity = Vector3.zero;
    }
}

