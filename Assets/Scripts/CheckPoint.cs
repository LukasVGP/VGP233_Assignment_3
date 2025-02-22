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
    }
}
