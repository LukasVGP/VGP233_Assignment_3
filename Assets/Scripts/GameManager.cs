using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float gameTime = 180f; // 3 minutes

    private int currentLives;
    private float currentTime;
    private bool isGameActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UIManager.Instance.ShowMainMenu();
    }

    private void Update()
    {
        if (!isGameActive) return;

        currentTime -= Time.deltaTime;
        UIManager.Instance.UpdateTimerText(currentTime);

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        if (GameController.Instance == null)
        {
            Debug.LogError("GameController instance is missing!");
            return;
        }

        currentLives = maxLives;
        currentTime = gameTime;
        isGameActive = true;
        UIManager.Instance.UpdateLives(currentLives);

        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer);
        }

        GameController.Instance.SpawnPlayer();
    }

    public void PlayerDied()
    {
        currentLives--;
        UIManager.Instance.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            GameController.Instance.PlayerDied(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    private void GameOver()
    {
        isGameActive = false;
        UIManager.Instance.ShowGameOver();
    }

    public void PlayerWon()
    {
        isGameActive = false;
        UIManager.Instance.ShowWinScreen();
    }
}
