using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            Debug.Log($"Player entered checkpoint trigger at {transform.position}");
            GameController.Instance.PlayerHitCheckPoint(transform);
        }
    }

    private void OnValidate()
    {
        if (!CompareTag("Checkpoint"))
        {
            Debug.Log("Setting Checkpoint tag");
            gameObject.tag = "Checkpoint";
        }

        // Make sure the collider is a trigger
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log("Setting collider to trigger");
        }
    }
}
