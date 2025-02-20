using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    [Header("Game Setup")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform checkPointsContainer;

    private GameObject currentPlayer;
    private int lastCheckPointIndex = -1;
    private SaveManager saveManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeSaveManager();
    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab != null && startPoint != null)
        {
            currentPlayer = Instantiate(playerPrefab, startPoint.position, startPoint.rotation);

            MoveCamera cameraScript = FindObjectOfType<MoveCamera>();
            if (cameraScript != null)
            {
                cameraScript.SetTarget(currentPlayer.transform);

                PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();
                if (playerMovement != null && playerMovement.orientation != null)
                {
                    cameraScript.orientation = playerMovement.orientation;
                }
            }
        }
    }

    private void InitializeSaveManager()
    {
        if (FindObjectOfType<SaveManager>() == null)
        {
            GameObject saveManagerObj = new GameObject("SaveManager");
            saveManager = saveManagerObj.AddComponent<SaveManager>();
        }
        else
        {
            saveManager = FindObjectOfType<SaveManager>();
        }

        if (saveManager != null)
        {
            saveManager.Load();
            GameState gameState = saveManager.GetGameState();
            lastCheckPointIndex = gameState.LastCheckpoint;
        }
    }

    public void PlayerDied(GameObject player)
    {
        Vector3 respawnPosition = (lastCheckPointIndex >= 0 && checkPointsContainer != null)
            ? checkPointsContainer.GetChild(lastCheckPointIndex).position
            : startPoint.position;

        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = respawnPosition;
        }
    }

    public void PlayerHitCheckPoint(Transform checkpoint)
    {
        if (checkpoint == null || checkPointsContainer == null) return;

        for (int i = 0; i < checkPointsContainer.childCount; i++)
        {
            if (checkpoint == checkPointsContainer.GetChild(i))
            {
                if (lastCheckPointIndex < i)
                {
                    lastCheckPointIndex = i;
                    saveManager?.SetLastCheckpointReached(i);
                }
                break;
            }
        }
    }
}
