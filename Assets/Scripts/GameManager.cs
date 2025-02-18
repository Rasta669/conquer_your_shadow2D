using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool isPaused = true; // Ensure game is paused at the start
    public GameObject gameOverUI;
    public GameObject mainMenuUI; // Reference to Main Menu UI

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance created");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 0f; // Pause the game until StartGame is called
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(true); // Show main menu at the start
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        Time.timeScale = 1f; // Resume time to start the game
        isPaused = false;

        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(false); // Hide the Main Menu
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // Stop the game
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Closes the application (only works in a built game)

        // If running in the editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
