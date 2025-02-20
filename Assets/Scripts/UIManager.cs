using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winScreenUI;

    [Header("Gameplay UI Elements")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ValidateReferences();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ValidateReferences()
    {
        if (mainMenuUI == null) Debug.LogError("Main Menu UI reference is missing!");
        if (gameplayUI == null) Debug.LogError("Gameplay UI reference is missing!");
        if (gameOverUI == null) Debug.LogError("Game Over UI reference is missing!");
        if (winScreenUI == null) Debug.LogError("Win Screen UI reference is missing!");
    }

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (mainMenuUI == null) return;

        mainMenuUI.SetActive(true);
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (winScreenUI != null) winScreenUI.SetActive(false);

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    public void ShowGameplay()
    {
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(true);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (winScreenUI != null) winScreenUI.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(true);
        if (winScreenUI != null) winScreenUI.SetActive(false);
    }

    public void ShowWinScreen()
    {
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (winScreenUI != null) winScreenUI.SetActive(true);
    }

    public void StartGame()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is missing! Ensure GameManager exists in the scene.");
            return;
        }

        ShowGameplay();
        GameManager.Instance.StartGame();
    }

    public void ReturnToMainMenu()
    {
        ShowMainMenu();
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {lives}";
        }
    }

    public void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {time:F1}";
        }
    }

    public void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
