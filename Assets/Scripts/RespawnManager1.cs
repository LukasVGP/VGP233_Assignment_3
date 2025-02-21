using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] checkpointPositions;
    [SerializeField] private Transform player;

    private void Start()
    {
        SaveManager.Instance.Load();
        RespawnAtLastCheckpoint();
    }

    public void RespawnAtLastCheckpoint()
    {
        int lastCheckpoint = SaveManager.Instance.GetGameState().LastCheckpoint;
        if (lastCheckpoint >= 0 && lastCheckpoint < checkpointPositions.Length)
        {
            player.position = checkpointPositions[lastCheckpoint].position;
        }
    }

    // Call this when player dies
    public void OnPlayerDeath()
    {
        RespawnAtLastCheckpoint();
    }
}
