using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private VisualElement mainMenu;
    private VisualElement optionsMenu;
    private VisualElement pauseMenu;
    private VisualElement resumeMenu;
    private VisualElement gameOverMenu;
    private VisualElement audioSettingsUI;
    private VisualElement videoSettingsUI;
    private VisualElement settingsUI;
    private VisualElement graphicsUI;
    private VisualElement achievemntsUI;
    private VisualElement controllerUI;
    private VisualElement instructionsUI;
    private VisualElement gameModePage; 
    private VisualElement youWinPage; 

    private UIDocument uiDocument;


    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        mainMenu = root.Q<VisualElement>("CQYS_startPage");
        optionsMenu = root.Q<VisualElement>("OptionsPage");
        pauseMenu = root.Q<VisualElement>("PauseMENU");
        resumeMenu = root.Q<VisualElement>("ResumeMenu");
        gameOverMenu = root.Q<VisualElement>("GameOverMenu");
        audioSettingsUI = root.Q<VisualElement>("AudioPage");
        videoSettingsUI = root.Q<VisualElement>("VideoPage");
        settingsUI = root.Q<VisualElement>("VideoSettingsPage");
        graphicsUI = root.Q<VisualElement>("GraphicsPage");
        achievemntsUI = root.Q<VisualElement>("AchievementsPage");
        controllerUI = root.Q<VisualElement>("ControllerPage");
        instructionsUI = root.Q<VisualElement>("InstructionsPage");
        gameModePage = root.Q<VisualElement>("GameModesPage");
        youWinPage = root.Q<VisualElement>("YouWinMenu");


        Button startButton = mainMenu.Q<Button>("StartButton");
        Button raceButton  = gameModePage.Q<Button>("RaceButton");
        Button survivalButton = gameModePage.Q<Button>("SurvivalButton");
        Button backButtonGM = gameModePage.Q<Button>("BackButton");
        Button pauseButton = pauseMenu.Q<Button>("PauseButton");
        Button resumeButton = resumeMenu.Q<Button>("ResumeButton");
        Button restarteButtonPs = resumeMenu.Q<Button>("RestartButton");
        Button restarteButtonYW = youWinPage.Q<Button>("RestartButton");
        Button optionsButton = mainMenu.Q<Button>("OptionsButton");
        Button achievemntsButton = mainMenu.Q<Button>("AchievemntsButton");
        Button audioButton = optionsMenu.Q<Button>("AudioButton");
        Button videoButton = optionsMenu.Q<Button>("VideoButton");
        Button controllerButton = optionsMenu.Q<Button>("ControllerButton");
        Button instructionsButton = optionsMenu.Q<Button>("InstructionsButton");
        Button settingsButton = videoSettingsUI.Q<Button>("SettingslButton");
        Button graphicsButton = videoSettingsUI.Q<Button>("GraphicsButton");
        Button quitButton = mainMenu.Q<Button>("QuitButton");
        Button backButtonGO = gameOverMenu.Q<Button>("RmainMenuButton"); // Back button to go back to main menu
        Button restartButtonGO = gameOverMenu.Q<Button>("RestartButton");
        Button backButtonOp =  optionsMenu.Q<Button>("BackButton");
        Button backButtonAud = audioSettingsUI.Q<Button>("BackButton");
        Button backButtonVS = videoSettingsUI.Q<Button>("BackButton");
        Button backButtonGs = graphicsUI.Q<Button>("BackButton");
        Button backButtonS = settingsUI.Q<Button>("BackButton");
        Button backButtonAch = achievemntsUI.Q<Button>("BackButton");
        Button backButtonInstr = instructionsUI.Q<Button>("BackButton");
        Button backButtonContrll = controllerUI.Q<Button>("BackButton");
        Button RMMGO = gameOverMenu.Q<Button>("RmainMenuButton"); 
        Button connectBtn = gameOverMenu.Q<Button>("ConnectButton");
        Button RMMAud = audioSettingsUI.Q<Button>("RmainMenuButton");
        Button RMMVS = videoSettingsUI.Q<Button>("RmainMenuButton");
        Button RMMS = settingsUI.Q<Button>("RmainMenuButton");
        Button RMMPs = resumeMenu.Q<Button>("RmainMenuButton");
        Button RMMGs = graphicsUI.Q<Button>("RmainMenuButton");
        Button RMMYw = youWinPage.Q<Button>("RmainMenuButton");
        //Button RMMAch = achievemntsUI.Q<Button>("RmainMenuButton");
        Button RMMInstr = instructionsUI.Q<Button>("RmainMenuButton");
        Button RMMContrll = controllerUI.Q<Button>("RmainMenuButton");
        RadioButton fullscreenRadioButton = videoSettingsUI.Q<RadioButton>("FullOrWindowed");
        Button ConnectButton = gameOverMenu.Q<Button>("ConnectButton");
        Button DisconnectButton = gameOverMenu.Q<Button>("DisConnectButton");

        startButton.clicked += ShowGameModemenu;
        raceButton.clicked += SetRaceMode;
        survivalButton.clicked += SetSurvivalMode;
        pauseButton.clicked += PauseGame; 
        resumeButton.clicked += Resume;
        restarteButtonPs.clicked += Restart;
        optionsButton.clicked += ShowOptions;
        achievemntsButton.clicked += ShowAchievemntsUI;
        videoButton.clicked += OpenVideoSettings;
        instructionsButton.clicked += ShowInstructionsUI;
        controllerButton.clicked += ShowControllerUI;
        restartButtonGO.clicked += Restart;
        restarteButtonYW.clicked += Restart;
        settingsButton.clicked += OpenSettingsPage;
        graphicsButton.clicked += OpenGraphicsPage;
        quitButton.clicked += QuitGame;
        audioButton.clicked += OpenAudioSettings;
        backButtonGM.clicked += GoBack;
        backButtonGO.clicked += GoBack;
        backButtonOp.clicked += GoBack;
        backButtonAud.clicked += GoBack;
        backButtonVS.clicked += GoBack;
        backButtonS.clicked += GoBack;
        backButtonGs.clicked += GoBack;
        backButtonAch.clicked += GoBack;
        backButtonInstr.clicked += GoBack;
        backButtonContrll.clicked += GoBack;
        RMMAud.clicked += ReturnToMainMenu;
        RMMS.clicked += ReturnToMainMenu;
        RMMGO.clicked += ReturnToMainMenuGO;
        RMMVS.clicked += ReturnToMainMenu;
        RMMVS.clicked += ReturnToMainMenu;
        RMMPs.clicked += ReturnToMainMenu;
        RMMGs.clicked += ReturnToMainMenu;
        RMMInstr.clicked += ReturnToMainMenu;
        RMMContrll.clicked += ReturnToMainMenu;
        RMMYw.clicked += ReturnToMainMenu;
        ConnectButton.clicked += ConnectWeb3;
        DisconnectButton.clicked += DisconnectWeb3;
        //fullscreenRadioButton.clicked += () => ToggleFullscreen(!Screen.fullScreen);

        // Back button for the main menu to return to main page
        //if (backButtonGO != null)
        //{
        //    backButtonGO.clicked += ReturnToMainMenu;
        //}

        if (fullscreenRadioButton != null)
        {
            fullscreenRadioButton.RegisterValueChangedCallback(evt =>
            {
                Debug.Log("RadioButton Callback Triggered! New Value: " + evt.newValue);
                ToggleFullscreen(evt.newValue);
            });
        }
        else
        {
            Debug.LogError("Fullscreen RadioButton is NULL! Check UI Query.");
        }

        
        //ResetUI();

    }

    private void ResetUI()
    {
        //mainMenu.style.display = DisplayStyle.Flex;
        //optionsMenu.style.display = DisplayStyle.None;
        //videoSettingsUI.style.display = DisplayStyle.None;
        //settingsUI.style.display = DisplayStyle.None;
        //graphicsUI.style.display = DisplayStyle.None;
        GameManager.Instance.ResetUI();

        Time.timeScale = 0f; // Ensure game is paused when in the menu
    }

    //private void StartGame()
    //{
    //    Debug.Log("Starting Game...");
    //    mainMenu.style.display = DisplayStyle.None;
    //    Time.timeScale = 1f;
    //    GameManager.Instance.StartGame();
    //}

    private void ShowOptions() => GameManager.Instance.ShowMenu(optionsMenu);
    private void OpenVideoSettings() => GameManager.Instance.ShowVideoSettings();

    private void GoBack() => GameManager.Instance.Back(); // Handle back navigation
    private void OpenSettingsPage() => GameManager.Instance.ShowSettingsPage();

    private void OpenAudioSettings() => GameManager.Instance.ShowMenu(audioSettingsUI); // Open audio settings
    private void OpenGraphicsPage() => GameManager.Instance.ShowGraphicsPage();
    private void QuitGame() => GameManager.Instance.QuitGame();
    private void ToggleFullscreen(bool isFullscreen) => GameManager.Instance.ToggleFullscreen(isFullscreen);

    // New method to handle back button from the main menu
    private void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        GameManager.Instance.ShowMenu(mainMenu);
    }

    private void ShowAchievemntsUI()
    {
        GameManager.Instance.ShowAchievementsUI();
    }

    void ShowInstructionsUI()
    {
        GameManager.Instance.ShowInstructionsUi();
    }

    void ShowControllerUI()
    {
        GameManager.Instance.ShowControllerUI();
    }

    void Restart()
    {
        GameManager.Instance.RestartGame();
    }

    void ReturnToMainMenuGO()
    {
        GameManager.Instance.ReturnToMainMenu();
    }

    void PauseGame()
    {
        GameManager.Instance.PauseGame();   
    }

    void Resume()
    {
        GameManager.Instance.ResumeGame();
    }

    void SetRaceMode()
    {
        GameManager.Instance.SetGameMode(0);
    }

    void SetSurvivalMode()
    {
        GameManager.Instance.SetGameMode(1);
    }

    void ShowGameModemenu()
    {
        GameManager.Instance.ShowGameModemenu();
    }

    void ConnectWeb3()
    {
        GameManager.Instance.Connect();
    }

    void DisconnectWeb3()
    {
        GameManager.Instance.Disconnect();  
    }
}
