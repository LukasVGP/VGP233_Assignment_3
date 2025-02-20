using UnityEngine;
using System.Collections.Generic;

public class SpinningObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 spinSpeed = Vector3.zero;
    [SerializeField] private Transform spinningObject;

    private struct TrackerData
    {
        public Vector3 previousPosition;
        public Vector3 velocity;
    }
    private List<TrackerData> trackerData;

    private void Awake()
    {
        trackerData = new List<TrackerData>();
        InitializeTrackerData();
    }

    private void InitializeTrackerData()
    {
        trackerData.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            TrackerData data = new TrackerData
            {
                previousPosition = transform.GetChild(i).position,
                velocity = Vector3.zero
            };
            trackerData.Add(data);
        }
    }

    private void Update()
    {
        if (spinningObject != null)
        {
            Vector3 spinAmount = spinSpeed * Time.deltaTime;
            spinningObject.Rotate(spinAmount.x, spinAmount.y, spinAmount.z);
        }

        for (int i = 0; i < trackerData.Count && i < transform.childCount; i++)
        {
            TrackerData data = trackerData[i];
            data.velocity = (transform.GetChild(i).position - data.previousPosition) / Time.deltaTime;
            data.previousPosition = transform.GetChild(i).position;
            trackerData[i] = data;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int closestTracker = GetClosestTrackerIndex(collision.transform.position);
            if (closestTracker >= 0 && closestTracker < trackerData.Count)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(trackerData[closestTracker].velocity * 500.0f);
                }
            }
        }
    }

    private int GetClosestTrackerIndex(Vector3 position)
    {
        float closestDistanceSq = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < transform.childCount && i < trackerData.Count; i++)
        {
            float distSq = Vector3.SqrMagnitude(position - transform.GetChild(i).position);
            if (distSq < closestDistanceSq)
            {
                closestDistanceSq = distSq;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
