using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isPaused = false;
    private bool isGameOver = false;
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

    private Slider contrastSlider;
    private Slider brightnessSlider;
    private RadioButton fullscreenRadioButton;

    private Stack<VisualElement> menuHistory = new Stack<VisualElement>(); // Track menu navigation

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        contrastSlider = settingsUI.Q<Slider>("Contrast");
        brightnessSlider = settingsUI.Q<Slider>("Brightness");
        fullscreenRadioButton = videoSettingsUI.Q<RadioButton>("Fullscreen");



        if (contrastSlider != null) contrastSlider.RegisterValueChangedCallback(evt => AdjustContrast(evt.newValue));
        if (brightnessSlider != null) brightnessSlider.RegisterValueChangedCallback(evt => AdjustBrightness(evt.newValue));
        if (fullscreenRadioButton != null) fullscreenRadioButton.RegisterValueChangedCallback(evt => ToggleFullscreen(evt.newValue));

        // Back button handling
        Button backButton = settingsUI.Q<Button>("BackButton");
        if (backButton != null)
            backButton.clicked += Back;

        ResetUI();

        Time.timeScale = 0f; // Start paused
        isPaused = true;
        isGameOver = false;
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

        menuHistory.Clear();
        menuHistory.Push(mainMenuUI); // Start from the main menu
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
        ShowMenu(null); // Hide all menus
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

    public void ShowMenu(VisualElement menu)
    {
        if (menuHistory.Count > 0)
            menuHistory.Peek().style.display = DisplayStyle.None; // Hide current menu

        if (menu != null)
        {
            menu.style.display = DisplayStyle.Flex;
            menuHistory.Push(menu);
            Time.timeScale = 0f; // Pause while in menus
        }
        else
        {
            menuHistory.Clear();
            Time.timeScale = 1f; // Resume game when no menus are open
        }
    }

    public void ShowVideoSettings() => ShowMenu(videoSettingsUI);
    public void ShowSettingsPage() => ShowMenu(settingsUI);
    public void ShowGraphicsPage() => ShowMenu(graphicsUI);
    public void ShowAudioSettings() => ShowMenu(audioSettingsUI); // Add this method

    public void ShowAchievementsUI()
    {
        ShowMenu(achievemntsUI);
    }

    public void ShowControllerUI()
    {
        ShowMenu(controllerUI);
    }

    public void ShowInstructionsUi()
    {
        ShowMenu(instructionsUI);
    }

    public void Back() // Add back functionality
    {
        if (menuHistory.Count > 1) // Ensure there's a previous menu
        {
            VisualElement currentMenu = menuHistory.Pop();
            currentMenu.style.display = DisplayStyle.None; // Hide current menu
            VisualElement previousMenu = menuHistory.Peek();
            previousMenu.style.display = DisplayStyle.Flex; // Show previous menu
            Debug.Log("Returning to previous menu: " + previousMenu.name);
        }
        else
        {
            // If there's no previous menu, return to the main menu
            ShowMenu(mainMenuUI);
        }
    }

    public void AdjustContrast(float value) => RenderSettings.ambientIntensity = value;
    public void AdjustBrightness(float value) => RenderSettings.ambientLight = new Color(value, value, value);
    public void ToggleFullscreen(bool isFullscreen)
    {
        Debug.Log("ToggleFullscreen called! New State: " + isFullscreen); // Confirm method is called

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




}
