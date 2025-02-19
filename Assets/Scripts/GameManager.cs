//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance { get; private set; }

//    private bool isPaused = false;
//    private UIDocument uiDocument;
//    private VisualElement pauseMenuUI;
//    private VisualElement resumeMenuUI;
//    private VisualElement mainMenuUI;

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void OnEnable()
//    {
//        uiDocument = FindObjectOfType<UIDocument>();
//        if (uiDocument == null)
//        {
//            Debug.LogError("UIDocument not found!");
//            return;
//        }

//        var root = uiDocument.rootVisualElement;

//        // Assign UI Elements
//        mainMenuUI = root.Q<VisualElement>("CQYS_startPage"); // Main Menu
//        pauseMenuUI = root.Q<VisualElement>("PauseMENU"); // Pause button is inside this
//        resumeMenuUI = root.Q<VisualElement>("ResumeMenu"); // This should pop up when paused

//        // Ensure correct visibility at start
//        mainMenuUI.style.display = DisplayStyle.Flex; // Show Main Menu at start
//        pauseMenuUI.style.display = DisplayStyle.None; // Hide Pause button
//        resumeMenuUI.style.display = DisplayStyle.None; // Hide Resume Menu

//        Time.timeScale = 0f; // Game starts paused
//    }

//    public void StartGame()
//    {
//        Debug.Log("Game Started!");
//        Time.timeScale = 1f;
//        isPaused = false;

//        // Hide Main Menu, show Pause button
//        mainMenuUI.style.display = DisplayStyle.None;
//        pauseMenuUI.style.display = DisplayStyle.Flex;
//        resumeMenuUI.style.display = DisplayStyle.None;
//    }

//    public void PauseGame()
//    {
//        if (!isPaused)
//        {
//            Time.timeScale = 0f;
//            isPaused = true;

//            // Hide Pause Button, Show Resume Menu
//            pauseMenuUI.style.display = DisplayStyle.None;
//            resumeMenuUI.style.display = DisplayStyle.Flex;
//        }
//    }

//    public void ResumeGame()
//    {
//        if (isPaused)
//        {
//            Time.timeScale = 1f;
//            isPaused = false;

//            // Show Pause Button, Hide Resume Menu
//            pauseMenuUI.style.display = DisplayStyle.Flex;
//            resumeMenuUI.style.display = DisplayStyle.None;
//        }
//    }

//    public void RestartGame()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void QuitGame()
//    {
//        Application.Quit();

//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isPaused = false;
    private UIDocument uiDocument;
    private VisualElement pauseMenuUI;
    private VisualElement resumeMenuUI;
    private VisualElement mainMenuUI;
    private VisualElement gameOverUI;

    void Awake()
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

    private void OnEnable()
    {
        uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found!");
            return;
        }

        var root = uiDocument.rootVisualElement;

        // Assign UI Elements
        mainMenuUI = root.Q<VisualElement>("CQYS_startPage"); // Main Menu
        pauseMenuUI = root.Q<VisualElement>("PauseMENU"); // Pause button UI
        resumeMenuUI = root.Q<VisualElement>("ResumeMenu"); // Resume Menu
        gameOverUI = root.Q<VisualElement>("GameOverMenu"); // Game Over UI

        // Ensure correct visibility at start
        mainMenuUI.style.display = DisplayStyle.Flex; // Show Main Menu
        pauseMenuUI.style.display = DisplayStyle.None; // Hide Pause button
        resumeMenuUI.style.display = DisplayStyle.None; // Hide Resume Menu
        gameOverUI.style.display = DisplayStyle.None; // Hide Game Over UI

        Time.timeScale = 0f; // Game starts paused
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        Time.timeScale = 1f;
        isPaused = false;

        // Hide Main Menu, Show Pause Button
        mainMenuUI.style.display = DisplayStyle.None;
        pauseMenuUI.style.display = DisplayStyle.Flex;
        resumeMenuUI.style.display = DisplayStyle.None;
        gameOverUI.style.display = DisplayStyle.None;
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;

            // Hide Pause Button, Show Resume Menu
            pauseMenuUI.style.display = DisplayStyle.None;
            resumeMenuUI.style.display = DisplayStyle.Flex;
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;

            // Show Pause Button, Hide Resume Menu
            pauseMenuUI.style.display = DisplayStyle.Flex;
            resumeMenuUI.style.display = DisplayStyle.None;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;

        // Show Game Over Screen, Hide Other Menus
        gameOverUI.style.display = DisplayStyle.Flex;
        pauseMenuUI.style.display = DisplayStyle.None;
        resumeMenuUI.style.display = DisplayStyle.None;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameOverUI.style.display = DisplayStyle.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
