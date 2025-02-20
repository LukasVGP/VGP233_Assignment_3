using UnityEngine;

public class MovableObs : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float offset = 0f;

    [Header("Movement Direction")]
    [SerializeField] private Vector3 moveDirection = Vector3.right;

    private bool isForward = true;
    private Vector3 startPos;
    private Vector3 targetPos;

    void Awake()
    {
        startPos = transform.position;
        moveDirection = moveDirection.normalized;
        transform.position += moveDirection * offset;
        targetPos = startPos + moveDirection * distance;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = isForward ? targetPos : startPos;

        // Calculate the movement for this frame
        float moveAmount = speed * Time.deltaTime;
        Vector3 moveVector = moveDirection * moveAmount;

        // Calculate the distance to target
        float distanceToTarget = Vector3.Distance(currentPosition, targetPosition);

        if (distanceToTarget <= moveAmount)
        {
            // If we're closer than our movement amount, snap to target
            transform.position = targetPosition;
            isForward = !isForward;
        }
        else
        {
            // Move towards target
            transform.position += isForward ? moveVector : -moveVector;
        }
    }
}
