using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

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

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winScreenUI;

    [Header("Gameplay UI Elements")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
        winScreenUI.SetActive(false);
    }

    public void ShowGameplay()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        gameOverUI.SetActive(false);
        winScreenUI.SetActive(false);
    }

    public void ShowGameOver()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(true);
        winScreenUI.SetActive(false);
    }

    public void ShowWinScreen()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
        winScreenUI.SetActive(true);
    }

    public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }

        ShowGameplay();

        if (gameplayUI != null)
        {
            gameplayUI.SetActive(true);

            if (livesText != null)
            {
                livesText.gameObject.SetActive(true);
            }

            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
            }
        }
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
