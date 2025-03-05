using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isGameWon = false;
    private UIDocument uiDocument;

    // UI Elements
    private VisualElement mainMenuUI;
    private VisualElement optionsMenuUI;
    private VisualElement pauseMenuUI;
    private VisualElement resumeMenuUI;
    private VisualElement gameOverUI;
    private VisualElement audioSettingsUI;
    private VisualElement videoSettingsUI;
    private VisualElement settingsUI;
    private VisualElement graphicsUI;
    private VisualElement achievemntsUI;
    private VisualElement controllerUI;
    private VisualElement instructionsUI;
    private VisualElement gameModePage;
    private VisualElement youWinPage;

    private Slider contrastSlider;
    private Slider brightnessSlider;
    private RadioButton fullscreenRadioButton;

    private Stack<VisualElement> menuHistory = new Stack<VisualElement>(); // Track menu navigation

    public enum GameMode { Race, Survival }
    public GameMode currentMode = GameMode.Race; // Default mode

    public float shadowSpeed = 5f; // Public parameter to modify in Inspector

    // Race mode variables
    private float raceTime = 0f;
    private int score = 0;
    private bool isRacing = false;
    private Label timeLabel;
    private Label scoreLabel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Ensure this is active
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isRacing && currentMode == GameMode.Race)
        {
            raceTime += Time.deltaTime;
            UpdateRaceUI();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found on scene load!");
            return;
        }

        var root = uiDocument.rootVisualElement;
        mainMenuUI = root.Q<VisualElement>("CQYS_startPage");
        optionsMenuUI = root.Q<VisualElement>("OptionsPage");
        pauseMenuUI = root.Q<VisualElement>("PauseMENU");
        resumeMenuUI = root.Q<VisualElement>("ResumeMenu");
        gameOverUI = root.Q<VisualElement>("GameOverMenu");
        audioSettingsUI = root.Q<VisualElement>("AudioPage");
        videoSettingsUI = root.Q<VisualElement>("VideoPage");
        settingsUI = root.Q<VisualElement>("VideoSettingsPage");
        graphicsUI = root.Q<VisualElement>("GraphicsPage");
        achievemntsUI = root.Q<VisualElement>("AchievementsPage");
        controllerUI = root.Q<VisualElement>("ControllerPage");
        instructionsUI = root.Q<VisualElement>("InstructionsPage");
        gameModePage = root.Q<VisualElement>("GameModesPage");
        youWinPage = root.Q<VisualElement>("YouWinMenu");

        contrastSlider = settingsUI.Q<Slider>("Contrast");
        brightnessSlider = settingsUI.Q<Slider>("Brightness");
        fullscreenRadioButton = videoSettingsUI.Q<RadioButton>("Fullscreen");

        if (contrastSlider != null)
            contrastSlider.RegisterValueChangedCallback(evt => AdjustContrast(evt.newValue));
        if (brightnessSlider != null)
            brightnessSlider.RegisterValueChangedCallback(evt => AdjustBrightness(evt.newValue));
        if (fullscreenRadioButton != null)
            fullscreenRadioButton.RegisterValueChangedCallback(evt => ToggleFullscreen(evt.newValue));

        // Back button handling
        Button backButton = settingsUI.Q<Button>("BackButton");
        if (backButton != null)
            backButton.clicked += Back;

        // Race mode UI element (using a single label to display either time or score)
        timeLabel = pauseMenuUI.Q<Label>("Scoretext");
        scoreLabel = pauseMenuUI.Q<Label>("Scoretext");
        if (timeLabel == null)
        {
            Debug.LogError("ScoreText label not found in the UI Document!");
        }
        else
        {
            Debug.Log("ScoreText label found: " + timeLabel);
            timeLabel.text = "Test Update";
            timeLabel.MarkDirtyRepaint();
        }


        ResetUI();

        Time.timeScale = 0f; // Start paused
        isPaused = true;
        isGameOver = false;
        isGameWon = false;
    }

    private void ResetUI()
    {
        if (mainMenuUI != null) mainMenuUI.style.display = DisplayStyle.Flex;
        if (optionsMenuUI != null) optionsMenuUI.style.display = DisplayStyle.None;
        if (pauseMenuUI != null) pauseMenuUI.style.display = DisplayStyle.None;
        if (resumeMenuUI != null) resumeMenuUI.style.display = DisplayStyle.None;
        if (gameOverUI != null) gameOverUI.style.display = DisplayStyle.None;
        if (audioSettingsUI != null) audioSettingsUI.style.display = DisplayStyle.None;
        if (videoSettingsUI != null) videoSettingsUI.style.display = DisplayStyle.None;
        if (settingsUI != null) settingsUI.style.display = DisplayStyle.None;
        if (graphicsUI != null) graphicsUI.style.display = DisplayStyle.None;
        if (achievemntsUI != null) achievemntsUI.style.display = DisplayStyle.None;
        if (controllerUI != null) controllerUI.style.display = DisplayStyle.None;
        if (instructionsUI != null) instructionsUI.style.display = DisplayStyle.None;
        if (gameModePage != null) gameModePage.style.display = DisplayStyle.None;

        menuHistory.Clear();
        menuHistory.Push(mainMenuUI); // Start from the main menu

        // Reset race mode data
        ResetGameState();
    }

    // Overridden ResetGameState to reset race time and score
    private void ResetGameState()
    {
        Debug.Log("Game state reset before returning to Main Menu.");
        raceTime = 0f;
        score = 0;
        isRacing = false;
        UpdateRaceUI();
    }

    public void SetGameMode(int mode)
    {
        currentMode = (GameMode)mode; // Convert int to enum
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        Debug.Log("Starting game with mode: " + currentMode);
        if (mainMenuUI != null)
            mainMenuUI.style.display = DisplayStyle.None;
        if (gameModePage != null)
            gameModePage.style.display = DisplayStyle.None;

        // Reset race mode values
        raceTime = 0f;
        score = 0;
        isRacing = (currentMode == GameMode.Race);

        Time.timeScale = 1f; // Resume time
        isPaused = false;
        isGameOver = false;
        isGameWon = false;
        UpdateRaceUI(); // Immediately update the label based on the mode
        ShowMenu(pauseMenuUI, true); // Hide all menus                 
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        isGameOver = true;
        Time.timeScale = 0f;
        ShowMenu(gameOverUI);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game...");
        Time.timeScale = 1f;
        ResetUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowMenu(VisualElement menu, bool isPauseMenu = false)
    {
        if (menuHistory.Count > 0)
            menuHistory.Peek().style.display = DisplayStyle.None; // Hide current menu

        if (menu != null)
        {
            menu.style.display = DisplayStyle.Flex;
            menuHistory.Push(menu);

            // Only pause the game if it's NOT the Pause menu
            if (!isPauseMenu)
                Time.timeScale = 0f;
        }
        else
        {
            menuHistory.Clear();
            Time.timeScale = 1f;
        }
    }

    public void ShowVideoSettings() => ShowMenu(videoSettingsUI);
    public void ShowSettingsPage() => ShowMenu(settingsUI);
    public void ShowGraphicsPage() => ShowMenu(graphicsUI);
    public void ShowAudioSettings() => ShowMenu(audioSettingsUI);

    public void ShowAchievementsUI() => ShowMenu(achievemntsUI);
    public void ShowControllerUI() => ShowMenu(controllerUI);
    public void ShowInstructionsUi() => ShowMenu(instructionsUI);

    public void Back()
    {
        if (menuHistory.Count > 1)
        {
            VisualElement currentMenu = menuHistory.Pop();
            currentMenu.style.display = DisplayStyle.None;
            VisualElement previousMenu = menuHistory.Peek();
            previousMenu.style.display = DisplayStyle.Flex;
            Debug.Log("Returning to previous menu: " + previousMenu.name);
        }
        else
        {
            ShowMenu(mainMenuUI);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        ShowMenu(resumeMenuUI);
        Debug.Log("Game Paused");
    }

    public void AdjustContrast(float value) => RenderSettings.ambientIntensity = value;
    public void AdjustBrightness(float value) => RenderSettings.ambientLight = new Color(value, value, value);

    public void ToggleFullscreen(bool isFullscreen)
    {
        Debug.Log("ToggleFullscreen called! New State: " + isFullscreen);
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.SetResolution(1920, 1080, true);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, false);
        }
        Debug.Log($"Fullscreen: {Screen.fullScreen}, Mode: {Screen.fullScreenMode}, Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 0f;
        ResetGameState();
        SceneManager.LoadScene("GameScene");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        ShowMenu(pauseMenuUI, true);
    }

    public void ShowGameModemenu()
    {
        Debug.Log("Start Button Clicked! Showing Game Mode Menu...");
        ShowMenu(gameModePage);
    }

    // Race mode specific methods
    private void UpdateRaceUI()
    {
        // Determine what text to show based on the current mode
        string displayText = "";
        if (currentMode == GameMode.Race)
        {
            displayText = "Time: " + raceTime.ToString("F2");
        }
        else if (currentMode == GameMode.Survival)
        {
            displayText = "Score: " + score;
        }

        Debug.Log("Updating Race UI with text: " + displayText);

        // Update the single UI element (both labels reference the same element)
        if (timeLabel != null)
            timeLabel.text = displayText;
        if (scoreLabel != null && scoreLabel != timeLabel)
            scoreLabel.text = displayText;
    }

    public void IncrementScore()
    {
        score++;
        UpdateRaceUI();
    }

    public void EndGame()
    {
        isRacing = false;
        Time.timeScale = 0f;
    }
    public void GameWin()
    {
        Debug.Log("Game Won!");
        isGameWon = true;
        Time.timeScale = 0f;
        ShowMenu(youWinPage);
    }
}
