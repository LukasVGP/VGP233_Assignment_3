using UnityEngine;
using NUnit.Framework;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    [Header("References")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform checkPointsContainer;

    private int lastCheckPointIndex = -1;
    private SaveManager saveManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Ensure SaveManager exists
        if (FindAnyObjectByType<SaveManager>() == null)
        {
            GameObject saveManagerObj = new GameObject("SaveManager");
            saveManager = saveManagerObj.AddComponent<SaveManager>();
        }
        else
        {
            saveManager = FindAnyObjectByType<SaveManager>();
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void OnDestroy()
    {
        if (saveManager != null)
        {
            saveManager.Save();
        }
    }

    private void Initialize()
    {
        if (startPoint == null || checkPointsContainer == null)
        {
            Debug.LogWarning("Please assign StartPoint and CheckpointsContainer in the inspector");
            return;
        }

        if (saveManager != null)
        {
            saveManager.Load();
            GameState gameState = saveManager.GetGameState();
            lastCheckPointIndex = gameState.LastCheckpoint;
        }
    }

    public void SetPlayerStartPosition(GameObject player)
    {
        if (player == null) return;

        Vector3 position = (lastCheckPointIndex >= 0 && checkPointsContainer != null)
            ? checkPointsContainer.GetChild(lastCheckPointIndex).position
            : (startPoint != null ? startPoint.position : Vector3.zero);

        SetPlayerPosition(player, position);
    }

    public void PlayerDied(GameObject player)
    {
        if (player == null) return;

        Vector3 respawnPosition;

        if (lastCheckPointIndex >= 0 && checkPointsContainer != null)
        {
            respawnPosition = checkPointsContainer.GetChild(lastCheckPointIndex).position;
        }
        else if (startPoint != null)
        {
            respawnPosition = startPoint.position;
        }
        else
        {
            respawnPosition = Vector3.zero;
        }

        SetPlayerPosition(player, respawnPosition);
    }

    public void PlayerHitCheckPoint(Transform checkpoint)
    {
        if (checkpoint == null || checkPointsContainer == null) return;

        for (int i = 0; i < checkPointsContainer.childCount; ++i)
        {
            if (checkpoint == checkPointsContainer.GetChild(i))
            {
                if (lastCheckPointIndex < i)
                {
                    lastCheckPointIndex = i;
                    if (saveManager != null)
                    {
                        saveManager.SetLastCheckpointReached(i);
                    }
                }
                break;
            }
        }
    }

    private void SetPlayerPosition(GameObject player, Vector3 position)
    {
        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.position = position;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            player.transform.position = position;
        }
    }
}
