using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Checkpoint {checkpointIndex} reached!"); // Add this debug line
            SaveManager.Instance.SetLastCheckpointReached(checkpointIndex);
            SaveManager.Instance.Save();
        }
    }
}
