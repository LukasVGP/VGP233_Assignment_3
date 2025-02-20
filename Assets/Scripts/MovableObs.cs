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
        if (isForward)
        {
            if (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position += moveDirection * Time.deltaTime * speed;
            }
            else
            {
                isForward = false;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) > 0.01f)
            {
                transform.position -= moveDirection * Time.deltaTime * speed;
            }
            else
            {
                isForward = true;
            }
        }
    }
}
