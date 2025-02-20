using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Camera Settings")]
    [SerializeField] private GameObject playerCamera;

    private Transform currentCheckpoint;
    private int checkpointIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (SaveManager.Instance == null)
            {
                GameObject saveManagerObject = new GameObject("SaveManager");
                saveManagerObject.AddComponent<SaveManager>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        if (playerPrefab != null)
        {
            GameObject player = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            if (playerCamera != null)
            {
                playerCamera.SetActive(true);
                MoveCamera moveCamera = playerCamera.GetComponent<MoveCamera>();
                if (moveCamera != null)
                {
                    moveCamera.SetTarget(player.transform);
                }
            }
        }
        else
        {
            Debug.LogError("Player Prefab is not assigned in GameController!");
        }
    }

    public void PlayerHitCheckPoint(Transform checkpoint)
    {
        if (SaveManager.Instance != null)
        {
            currentCheckpoint = checkpoint;
            checkpointIndex++;
            SaveManager.Instance.SetLastCheckpointReached(checkpointIndex);
            SaveManager.Instance.Save();
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        if (currentCheckpoint != null)
        {
            player.transform.position = currentCheckpoint.position;
            player.transform.rotation = currentCheckpoint.rotation;
        }
        else if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }

        if (playerCamera != null)
        {
            playerCamera.SetActive(true);
            MoveCamera moveCamera = playerCamera.GetComponent<MoveCamera>();
            if (moveCamera != null)
            {
                moveCamera.SetTarget(player.transform);
            }
        }
    }
}
