using UnityEngine;
using NUnit.Framework;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    [Header("References")]
    public Transform startPoint;
    public Transform[] checkpoints = new Transform[5];
    public GameObject playerPrefab;
    public Camera mainCamera;

    private int lastCheckPointIndex = -1;
    private SaveManager saveManager;
    private GameObject currentPlayer;
    private bool isFirstStart = true;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (FindAnyObjectByType<SaveManager>() == null)
        {
            GameObject saveManagerObj = new GameObject("SaveManager");
            saveManager = saveManagerObj.AddComponent<SaveManager>();
            Debug.Log("Created new SaveManager");
        }
        else
        {
            saveManager = FindAnyObjectByType<SaveManager>();
            Debug.Log("Found existing SaveManager");
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void Initialize()
    {
        if (startPoint == null)
        {
            Debug.LogError("Start point not assigned!");
            return;
        }

        if (!isFirstStart && saveManager != null)
        {
            saveManager.Load();
            GameState gameState = saveManager.GetGameState();
            lastCheckPointIndex = gameState.LastCheckpoint;
            Debug.Log($"Loaded checkpoint index: {lastCheckPointIndex}");
        }
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null) return;

        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        currentPlayer = Instantiate(playerPrefab);

        if (isFirstStart)
        {
            SetPlayerPosition(currentPlayer, startPoint.position);
            isFirstStart = false;
            lastCheckPointIndex = -1;
            if (saveManager != null)
            {
                saveManager.SetLastCheckpointReached(-1);
                saveManager.Save();
            }
        }
        else
        {
            SetPlayerStartPosition(currentPlayer);
        }

        SetupCamera();
    }

    public void RespawnPlayer(GameObject player)
    {
        if (player != null)
        {
            PlayerDied(player);
            SetupCamera();
            Debug.Log($"Player {player.name} respawned at checkpoint {lastCheckPointIndex}");
        }
    }

    public void SetPlayerStartPosition(GameObject player)
    {
        if (player == null) return;

        Vector3 position;
        if (lastCheckPointIndex >= 0 && lastCheckPointIndex < checkpoints.Length && checkpoints[lastCheckPointIndex] != null)
        {
            position = checkpoints[lastCheckPointIndex].position;
            Debug.Log($"Spawning at checkpoint {lastCheckPointIndex}");
        }
        else
        {
            position = startPoint.position;
            Debug.Log("Spawning at start point");
        }

        SetPlayerPosition(player, position);
    }

    public void PlayerDied(GameObject player)
    {
        if (player == null) return;

        Vector3 respawnPosition;
        if (lastCheckPointIndex >= 0 && lastCheckPointIndex < checkpoints.Length && checkpoints[lastCheckPointIndex] != null)
        {
            respawnPosition = checkpoints[lastCheckPointIndex].position;
            Debug.Log($"Respawning at checkpoint {lastCheckPointIndex}");
        }
        else
        {
            respawnPosition = startPoint.position;
            Debug.Log("Respawning at start point");
        }

        SetPlayerPosition(player, respawnPosition);
    }

    public void PlayerHitCheckPoint(Transform checkpoint)
    {
        if (checkpoint == null) return;

        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] == checkpoint)
            {
                lastCheckPointIndex = i;
                Debug.Log($"Checkpoint {i} activated at position {checkpoint.position}");

                if (saveManager != null)
                {
                    saveManager.SetLastCheckpointReached(i);
                    saveManager.Save();
                }
                break;
            }
        }
    }

    private void SetPlayerPosition(GameObject player, Vector3 position)
    {
        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = position;
        }
        else
        {
            player.transform.position = position;
        }
    }

    private void SetupCamera()
    {
        if (currentPlayer == null || mainCamera == null) return;

        MoveCamera cameraController = mainCamera.GetComponent<MoveCamera>();
        if (cameraController != null)
        {
            cameraController.targetToLookAt = currentPlayer.transform;
            cameraController.enabled = true;
        }
    }
}
