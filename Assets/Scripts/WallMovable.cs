using UnityEngine;
using System.Collections;

public class WallMovable : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private bool isDown = true;
    [SerializeField] private bool isRandom = true;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector3 moveDirection = Vector3.up;

    private float distance;
    private Vector3 startPos;
    private bool isWaiting = false;
    private bool canChange = true;

    void Awake()
    {
        distance = transform.localScale.y;
        startPos = transform.position;
        if (!isDown)
        {
            transform.position = startPos - moveDirection * distance;
        }
    }

    void Update()
    {
        if (isDown)
        {
            if (Vector3.Distance(transform.position, startPos + moveDirection * distance) > 0.01f)
            {
                transform.position += moveDirection * Time.deltaTime * speed;
            }
            else if (!isWaiting)
            {
                StartCoroutine(WaitToChange(0.25f));
            }
        }
        else
        {
            if (!canChange) return;

            if (Vector3.Distance(transform.position, startPos) > 0.01f)
            {
                transform.position -= moveDirection * Time.deltaTime * speed;
            }
            else if (!isWaiting)
            {
                StartCoroutine(WaitToChange(0.25f));
            }
        }
    }

    IEnumerator WaitToChange(float time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        isWaiting = false;
        isDown = !isDown;

        if (isRandom && !isDown)
        {
            if (Random.value > 0.5f)
            {
                StartCoroutine(Retry(1.5f));
            }
        }
    }

    IEnumerator Retry(float time)
    {
        canChange = false;
        yield return new WaitForSeconds(time);

        if (Random.value > 0.5f)
        {
            StartCoroutine(Retry(1.25f));
        }
        else
        {
            canChange = true;
        }
    }
}
