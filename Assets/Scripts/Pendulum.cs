using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [Header("Pendulum Settings")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float limit = 75f;
    [SerializeField] private bool randomStart = false;

    [Header("Swing Direction")]
    [SerializeField] private Vector3 swingAxis = Vector3.forward; // Z-axis by default

    private float random = 0;

    void Awake()
    {
        if (randomStart)
            random = Random.Range(0f, 1f);
    }

    void Update()
    {
        float angle = limit * Mathf.Sin(Time.time + random * speed);
        transform.localRotation = Quaternion.Euler(
            swingAxis.x * angle,
            swingAxis.y * angle,
            swingAxis.z * angle
        );
    }
}
