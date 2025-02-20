using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Screens")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;

    [Header("HUD Elements")]
    [SerializeField] private Image heartIcon;
    [SerializeField] private TMP_Text livesCountText;
    [SerializeField] private TMP_Text timerText;

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
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuScreen.SetActive(true);
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void ShowGameOver()
    {
        mainMenuScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        winScreen.SetActive(false);
    }

    public void ShowWinScreen()
    {
        mainMenuScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        winScreen.SetActive(true);
    }

    public void UpdateLives(int currentLives)
    {
        livesCountText.text = currentLives.ToString();
    }

    public void UpdateTimer(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartGame();
    }

    public void BackToMainMenu()
    {
        ShowMainMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
