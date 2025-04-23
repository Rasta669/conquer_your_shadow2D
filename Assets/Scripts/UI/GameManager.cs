//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
//using Thirdweb;
//using Thirdweb.Unity;

//public class GameManager : MonoBehaviour
//{
//	public static GameManager Instance { get; private set; }

//	private bool isPaused = false;
//	private bool isGameOver = false;
//	private bool isGameWon = false;
//	private UIDocument uiDocument;

//	// UI Elements
//	private ThirdwebManager thirdweb;
//	private VisualElement mainMenuUI;
//	private VisualElement optionsMenuUI;
//	private VisualElement pauseMenuUI;
//	private VisualElement resumeMenuUI;
//	private VisualElement gameOverUI;
//	private VisualElement audioSettingsUI;
//	private VisualElement videoSettingsUI;
//	private VisualElement settingsUI;
//	private VisualElement graphicsUI;
//	private VisualElement achievemntsUI;
//	private VisualElement controllerUI;
//	private VisualElement instructionsUI;
//	private VisualElement gameModePage;
//	private VisualElement youWinPage;
//	private Label AddressText;
//	private Label ConnectedText;
//	private Label EthBalanceText;
//	private Label CustomTokenBalanceText;
//	private Label ClaimedTokenBalanceText;
//	private Slider contrastSlider;
//	private Slider brightnessSlider;
//	private RadioButton fullscreenRadioButton;
//	private Button ConnectButton; 
//	private Button DisconnectButton; 


//	private Stack<VisualElement> menuHistory = new Stack<VisualElement>(); // Track menu navigation

//	public enum GameMode { Race, Survival }
//	public GameMode currentMode = GameMode.Race; // Default mode

//	public float shadowSpeed = 5f; // Public parameter to modify in Inspector

//	// Race mode variables
//	private float raceTime = 0f;
//	private int score = 0;
//	private bool isRacing = false;
//	private Label timeLabel;
//	private Label scoreLabel;
//	private Label winScoreLabel;

//	private UISpriteAnimation mainmenu_anim;
//	public GameObject main_menu_holder;

//	private IThirdwebWallet wallet;
//	private string walletAddress;
//	[field: SerializeField, Header("Wallet Options")]
//	private ulong ActiveChainId = 84532;


//	private void Awake()
//	{
//		thirdweb = FindObjectOfType<ThirdwebManager>();
//		if (Instance == null)
//		{
//			Instance = this;
//			DontDestroyOnLoad(gameObject);

//			SceneManager.sceneLoaded += OnSceneLoaded; // Ensure this is activ
//		}
//		else
//		{
//			Destroy(gameObject);
//		}

//		if (thirdweb == null)
//		{
//			Debug.LogError("ThirdwebManager not found in the scene! Please add the ThirdwebManager prefab.");
//		}
//		//if(thirdweb== null)
//		//{

//		//}
//	}

//	private void Update()
//	{
//		if (isRacing && currentMode == GameMode.Race)
//		{
//			raceTime += Time.deltaTime;
//			UpdateRaceUI();
//		}
//	}

//	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//	{

//		uiDocument = FindObjectOfType<UIDocument>();
//		if (uiDocument == null)
//		{
//			Debug.LogError("UIDocument not found on scene load!");
//			return;
//		}

//		var root = uiDocument.rootVisualElement;
//		mainMenuUI = root.Q<VisualElement>("CQYS_startPage");
//		optionsMenuUI = root.Q<VisualElement>("OptionsPage");
//		pauseMenuUI = root.Q<VisualElement>("PauseMENU");
//		resumeMenuUI = root.Q<VisualElement>("ResumeMenu");
//		gameOverUI = root.Q<VisualElement>("GameOverMenu");
//		audioSettingsUI = root.Q<VisualElement>("AudioPage");
//		videoSettingsUI = root.Q<VisualElement>("VideoPage");
//		settingsUI = root.Q<VisualElement>("VideoSettingsPage");
//		graphicsUI = root.Q<VisualElement>("GraphicsPage");
//		achievemntsUI = root.Q<VisualElement>("AchievementsPage");
//		controllerUI = root.Q<VisualElement>("ControllerPage");
//		instructionsUI = root.Q<VisualElement>("InstructionsPage");
//		gameModePage = root.Q<VisualElement>("GameModesPage");
//		youWinPage = root.Q<VisualElement>("YouWinMenu");

//		contrastSlider = settingsUI.Q<Slider>("Contrast");
//		brightnessSlider = settingsUI.Q<Slider>("Brightness");
//		fullscreenRadioButton = videoSettingsUI.Q<RadioButton>("Fullscreen");
//		ConnectButton = gameOverUI.Q<Button>("ConnectButton");
//		DisconnectButton = gameOverUI.Q<Button>("DisConnectButton");
//		AddressText = gameOverUI.Q<Label>("DisconnectButton");
//		ConnectedText = gameOverUI.Q<Label>("DisconnectButton");
//		EthBalanceText = gameOverUI.Q<Label>("EthBalanceTexct");
//		ClaimedTokenBalanceText = gameOverUI.Q<Label>("CysBalanceTexct");
//		CustomTokenBalanceText = gameOverUI.Q<Label>("CysBalanceTexct");

//		if (contrastSlider != null)
//			contrastSlider.RegisterValueChangedCallback(evt => AdjustContrast(evt.newValue));
//		if (brightnessSlider != null)
//			brightnessSlider.RegisterValueChangedCallback(evt => AdjustBrightness(evt.newValue));
//		if (fullscreenRadioButton != null)
//			fullscreenRadioButton.RegisterValueChangedCallback(evt => ToggleFullscreen(evt.newValue));

//		// Back button handling
//		Button backButton = settingsUI.Q<Button>("BackButton");
//		if (backButton != null)
//			backButton.clicked += Back;

//		// Race mode UI element (using a single label to display either time or score)
//		timeLabel = pauseMenuUI.Q<Label>("Scoretext");
//		scoreLabel = pauseMenuUI.Q<Label>("Scoretext");
//		winScoreLabel = youWinPage.Q<Label>("YourScoreText");
//		if (timeLabel == null)
//		{
//			Debug.LogError("ScoreText label not found in the UI Document!");
//		}
//		else
//		{
//			Debug.Log("ScoreText label found: " + timeLabel);
//			timeLabel.text = "Test Update";
//			timeLabel.MarkDirtyRepaint();
//		}
//		//mainmenu_anim = main_menu_holder.GetComponent<UISpriteAnimation>();
//		//mainmenu_anim.Func_PlayUIAnim();
//		AudioManager.Instance.PlayMenuMusic();

//		ResetUI();

//		Time.timeScale = 0f; // Start paused
//		isPaused = true;
//		isGameOver = false;
//		isGameWon = false;
//	}

//	public void ResetUI()
//	{
//		if (mainMenuUI != null)
//		{
//			mainMenuUI.style.display = DisplayStyle.Flex;


//		}
//		if (optionsMenuUI != null) optionsMenuUI.style.display = DisplayStyle.None;
//		if (pauseMenuUI != null) pauseMenuUI.style.display = DisplayStyle.None;
//		if (resumeMenuUI != null) resumeMenuUI.style.display = DisplayStyle.None;
//		if (gameOverUI != null) gameOverUI.style.display = DisplayStyle.None;
//		if (audioSettingsUI != null) audioSettingsUI.style.display = DisplayStyle.None;
//		if (videoSettingsUI != null) videoSettingsUI.style.display = DisplayStyle.None;
//		if (settingsUI != null) settingsUI.style.display = DisplayStyle.None;
//		if (graphicsUI != null) graphicsUI.style.display = DisplayStyle.None;
//		if (achievemntsUI != null) achievemntsUI.style.display = DisplayStyle.None;
//		if (controllerUI != null) controllerUI.style.display = DisplayStyle.None;
//		if (instructionsUI != null) instructionsUI.style.display = DisplayStyle.None;
//		if (gameModePage != null) gameModePage.style.display = DisplayStyle.None;

//		menuHistory.Clear();
//		menuHistory.Push(mainMenuUI); // Start from the main menu

//		// Reset race mode data
//		ResetGameState();
//	}

//	// Overridden ResetGameState to reset race time and score
//	private void ResetGameState()
//	{
//		Debug.Log("Game state reset before returning to Main Menu.");
//		raceTime = 0f;
//		score = 0;
//		isRacing = false;
//		UpdateRaceUI();
//	}

//	public void SetGameMode(int mode)
//	{
//		currentMode = (GameMode)mode; // Convert int to enum
//		StartGame();
//	}

//	public void StartGame()
//	{
//		Debug.Log("Game Started!");
//		Debug.Log("Starting game with mode: " + currentMode);
//		if (mainMenuUI != null)
//			mainMenuUI.style.display = DisplayStyle.None;
//		if (gameModePage != null)
//			gameModePage.style.display = DisplayStyle.None;

//		// Reset race mode values
//		raceTime = 0f;
//		score = 0;
//		isRacing = (currentMode == GameMode.Race);

//		Time.timeScale = 1f; // Resume time
//		AudioManager.Instance.PlayGameplayMusic();
//		Debug.Log("Gameplay music!");
//		AudioManager.Instance.PlayWaterSound();
//		isPaused = false;
//		isGameOver = false;
//		isGameWon = false;
//		UpdateRaceUI(); // Immediately update the label based on the mode
//		ShowMenu(pauseMenuUI, true); // Hide all menus                 
//	}

//	public void QuitGame()
//	{
//		Debug.Log("Quitting Game...");
//		Application.Quit();
//#if UNITY_EDITOR
//		UnityEditor.EditorApplication.isPlaying = false;
//#endif
//	}

//	public void GameOver()
//	{
//		Debug.Log("Game Over!");
//		isGameOver = true;
//		AudioManager.Instance.PlayGameOverMusic();
//		Time.timeScale = 0f;
//		ShowMenu(gameOverUI);
//		// Initialize UI elements on start
//		if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
//		if (DisconnectButton != null)
//		{
//			DisconnectButton.style.display = DisplayStyle.None;
//			// Ensure the button is interactable
//			//var buttonComponent = DisconnectButton.GetComponent<UnityEngine.UI.Button>();
//			//if (buttonComponent != null)
//			//{
//			//    buttonComponent.interactable = true;
//			//    Debug.Log("DisconnectButton is interactable: " + buttonComponent.interactable);
//			//}
//			//else
//			//{
//			//    Debug.LogError("DisconnectButton does not have a Button component!");
//			//}
//		}
//		if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
//		if (AddressText != null) AddressText.style.display = DisplayStyle.None;
//		if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
//		if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
//		if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
//	}

//	public void RestartGame()
//	{
//		Debug.Log("Restarting Game...");
//		Time.timeScale = 1f;
//		ResetUI();
//		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//	}

//	public void ShowMenu(VisualElement menu, bool isPauseMenu = false)
//	{
//		if (menuHistory.Count > 0)
//			menuHistory.Peek().style.display = DisplayStyle.None; // Hide current menu

//		if (menu != null)
//		{
//			menu.style.display = DisplayStyle.Flex;
//			menuHistory.Push(menu);

//			// Only pause the game if it's NOT the Pause menu
//			if (!isPauseMenu)
//				Time.timeScale = 0f;
//		}
//		else
//		{
//			menuHistory.Clear();
//			Time.timeScale = 1f;
//		}
//	}

//	public void ShowVideoSettings() => ShowMenu(videoSettingsUI);
//	public void ShowSettingsPage() => ShowMenu(settingsUI);
//	public void ShowGraphicsPage() => ShowMenu(graphicsUI);
//	public void ShowAudioSettings() => ShowMenu(audioSettingsUI);

//	public void ShowAchievementsUI() => ShowMenu(achievemntsUI);
//	public void ShowControllerUI() => ShowMenu(controllerUI);
//	public void ShowInstructionsUi() => ShowMenu(instructionsUI);

//	public void Back()
//	{
//		if (menuHistory.Count > 1)
//		{
//			VisualElement currentMenu = menuHistory.Pop();
//			currentMenu.style.display = DisplayStyle.None;
//			VisualElement previousMenu = menuHistory.Peek();
//			previousMenu.style.display = DisplayStyle.Flex;
//			Debug.Log("Returning to previous menu: " + previousMenu.name);
//		}
//		else
//		{
//			ShowMenu(mainMenuUI);
//		}
//	}

//	public void PauseGame()
//	{
//		Time.timeScale = 0f;
//		AudioManager.Instance.PlayPauseMusic();
//		ShowMenu(resumeMenuUI);
//		Debug.Log("Game Paused");
//	}

//	public void AdjustContrast(float value) => RenderSettings.ambientIntensity = value;
//	public void AdjustBrightness(float value) => RenderSettings.ambientLight = new Color(value, value, value);

//	public void ToggleFullscreen(bool isFullscreen)
//	{
//		Debug.Log("ToggleFullscreen called! New State: " + isFullscreen);
//		if (isFullscreen)
//		{
//			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
//			Screen.SetResolution(1920, 1080, true);
//		}
//		else
//		{
//			Screen.fullScreenMode = FullScreenMode.Windowed;
//			Screen.SetResolution(1280, 720, false);
//		}
//		Debug.Log($"Fullscreen: {Screen.fullScreen}, Mode: {Screen.fullScreenMode}, Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}");
//	}

//	public void ReturnToMainMenu()
//	{
//		Time.timeScale = 0f;
//		ResetGameState();
//		SceneManager.LoadScene("GameScene");
//	}

//	public void ResumeGame()
//	{
//		Time.timeScale = 1f;
//		AudioManager.Instance.PlayGameplayMusic();
//		ShowMenu(pauseMenuUI, true);
//	}

//	public void ShowGameModemenu()
//	{
//		Debug.Log("Start Button Clicked! Showing Game Mode Menu...");
//		ShowMenu(gameModePage);
//	}

//	// Race mode specific methods
//	private void UpdateRaceUI()
//	{
//		// Determine what text to show based on the current mode
//		string displayText = "";
//		if (currentMode == GameMode.Race)
//		{
//			displayText = "Time: " + raceTime.ToString("F2");
//		}
//		else if (currentMode == GameMode.Survival)
//		{
//			displayText = "Score: " + score;
//		}

//		Debug.Log("Updating Race UI with text: " + displayText);

//		// Update the single UI element (both labels reference the same element)
//		if (timeLabel != null)
//			timeLabel.text = displayText;
//		if (scoreLabel != null && scoreLabel != timeLabel)
//			scoreLabel.text = displayText;
//	}

//	public void IncrementScore()
//	{
//		score++;
//		UpdateRaceUI();
//	}

//	public void EndGame()
//	{
//		isRacing = false;
//		Time.timeScale = 0f;
//	}
//	public void GameWin()
//	{
//		Debug.Log("Game Won!");
//		isGameWon = true;
//		if (AudioManager.Instance != null)
//		{
//			AudioManager.Instance.PlayGameWinMusic(); // This should only be called here when the game is actually won
//			Time.timeScale = 0f;
//		}
//		else
//		{
//			Debug.LogWarning("AudioManager instance is null! Make sure it's in the scene.");
//		}

//		ShowMenu(youWinPage);

//		string displayText = "";

//		if (currentMode == GameMode.Race)
//		{
//			displayText = "Time used: " + raceTime.ToString("F2");
//		}
//		else if (currentMode == GameMode.Survival)
//		{
//			displayText = "Your Score: " + score;
//		}

//		if (winScoreLabel != null)
//			winScoreLabel.text = displayText;

//	}

//	public async void Connect()
//	{
//		if (thirdweb == null)
//		{
//			Debug.LogError("Cannot connect: ThirdwebManager is not initialized.");
//			return;
//		}

//		try
//		{
//			var options = new WalletOptions(
//				provider: WalletProvider.WalletConnectWallet,
//				chainId: 84532
//			);

//			Debug.Log("Initiating wallet connection...");
//			wallet = await ThirdwebManager.Instance.ConnectWallet(options);
//			walletAddress = await wallet.GetAddress();
//			Debug.Log($"Wallet connected successfully! Address: {walletAddress}");

//			var balance = await wallet.GetBalance(chainId: ActiveChainId);
//			var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
//			Debug.Log($"Wallet balance: {balanceEth}");

//			// Update UI after successful connection
//			if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.None; // Hide connect button
//			if (DisconnectButton != null)
//			{
//				DisconnectButton.style.display = DisplayStyle.Flex; // Show disconnect button
//				// Ensure the button is interactable
//				//var buttonComponent = DisconnectButton;
//				//if (buttonComponent != null)
//				//{
//				//    buttonComponent.interactable = true;
//				//    Debug.Log("DisconnectButton is interactable after Connect: " + buttonComponent.interactable);
//				//}
//			}
//			if (ConnectedText != null)
//			{
//				ConnectedText.style.display = DisplayStyle.Flex; // Show connected text
//				ConnectedText.text = "Connected";
//			}
//			if (AddressText != null && !string.IsNullOrEmpty(walletAddress))
//			{
//				AddressText.style.display = DisplayStyle.Flex; // Show address text
//				// Show first 3 and last 3 characters of the address with dots in between
//				string shortAddress = $"{walletAddress.Substring(0, 3)}...{walletAddress.Substring(walletAddress.Length - 3)}";
//				AddressText.text = shortAddress;
//			}
//		}
//		catch (System.Exception ex)
//		{
//			Debug.LogError($"Failed to connect wallet: {ex.Message}");
//			wallet = null;
//			walletAddress = null;

//			// Reset UI on failure
//			if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
//			if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None;
//			if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
//			if (AddressText != null) AddressText.style.display = DisplayStyle.None;
//			if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
//			if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
//			if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
//		}
//	}

//	public async void Disconnect()
//	{
//		if (wallet == null)
//		{
//			Debug.LogWarning("No wallet to disconnect.");
//			return;
//		}

//		try
//		{
//			Debug.Log("Disconnect function called! Disconnecting wallet...");
//			await wallet.Disconnect();
//			wallet = null;
//			walletAddress = null;
//			Debug.Log("Wallet disconnected successfully.");

//			// Update UI after successful disconnection
//			if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex; // Show connect button
//			if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None; // Hide disconnect button
//			if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None; // Hide connected text
//			if (AddressText != null) AddressText.style.display = DisplayStyle.None; // Hide address text
//			if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
//			if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
//			if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
//		}
//		catch (System.Exception ex)
//		{
//			Debug.LogError($"Failed to disconnect wallet: {ex.Message}");
//		}
//	}
//}


//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
//using Thirdweb;
//using Thirdweb.Unity;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance { get; private set; }

//    private bool isPaused = false;
//    private bool isGameOver = false;
//    private bool isGameWon = false;
//    private UIDocument uiDocument;

//    // UI Elements
//    private ThirdwebManager thirdweb;
//    private VisualElement mainMenuUI;
//    private VisualElement optionsMenuUI;
//    private VisualElement pauseMenuUI;
//    private VisualElement resumeMenuUI;
//    private VisualElement gameOverUI;
//    private VisualElement audioSettingsUI;
//    private VisualElement videoSettingsUI;
//    private VisualElement settingsUI;
//    private VisualElement graphicsUI;
//    private VisualElement achievemntsUI;
//    private VisualElement controllerUI;
//    private VisualElement instructionsUI;
//    private VisualElement gameModePage;
//    private VisualElement youWinPage;
//    private Label AddressText;
//    private Label ConnectedText;
//    private Label EthBalanceText;
//    private Label CustomTokenBalanceText;
//    private Label ClaimedTokenBalanceText;
//    private Slider contrastSlider;
//    private Slider brightnessSlider;
//    private RadioButton fullscreenRadioButton;
//    private Button ConnectButton;
//    private Button DisconnectButton;

//    private Stack<VisualElement> menuHistory = new Stack<VisualElement>(); // Track menu navigation

//    public enum GameMode { Race, Survival }
//    public GameMode currentMode = GameMode.Race; // Default mode

//    public float shadowSpeed = 5f; // Public parameter to modify in Inspector

//    // Race mode variables
//    private float raceTime = 0f;
//    private int score = 0;
//    private bool isRacing = false;
//    private Label timeLabel;
//    private Label scoreLabel;
//    private Label winScoreLabel;

//    private UISpriteAnimation mainmenu_anim;
//    public GameObject main_menu_holder;

//    private IThirdwebWallet wallet;
//    private string walletAddress;
//    [field: SerializeField, Header("Wallet Options")]
//    private ulong ActiveChainId = 84532;

//    private void Awake()
//    {
//        thirdweb = FindObjectOfType<ThirdwebManager>();
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            SceneManager.sceneLoaded += OnSceneLoaded; // Ensure this is active
//        }
//        else
//        {
//            // If another instance exists, destroy this one but check AudioListeners first
//            CheckAndRemoveDuplicateAudioListeners();
//            Destroy(gameObject);
//            return;
//        }

//        if (thirdweb == null)
//        {
//            Debug.LogError("ThirdwebManager not found in the scene! Please add the ThirdwebManager prefab.");
//        }

//        // Ensure GameManager doesn’t have an AudioListener
//        AudioListener audioListener = GetComponent<AudioListener>();
//        if (audioListener != null)
//        {
//            Destroy(audioListener);
//            Debug.Log("Removed AudioListener from GameManager to avoid duplicates.");
//        }
//    }

//    private void Update()
//    {
//        if (isRacing && currentMode == GameMode.Race)
//        {
//            raceTime += Time.deltaTime;
//            UpdateRaceUI();
//        }
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        CheckAndRemoveDuplicateAudioListeners();

//        uiDocument = FindObjectOfType<UIDocument>();
//        if (uiDocument == null)
//        {
//            Debug.LogError("UIDocument not found on scene load!");
//            return;
//        }

//        var root = uiDocument.rootVisualElement;
//        mainMenuUI = root.Q<VisualElement>("CQYS_startPage");
//        optionsMenuUI = root.Q<VisualElement>("OptionsPage");
//        pauseMenuUI = root.Q<VisualElement>("PauseMENU");
//        resumeMenuUI = root.Q<VisualElement>("ResumeMenu");
//        gameOverUI = root.Q<VisualElement>("GameOverMenu");
//        audioSettingsUI = root.Q<VisualElement>("AudioPage");
//        videoSettingsUI = root.Q<VisualElement>("VideoPage");
//        settingsUI = root.Q<VisualElement>("VideoSettingsPage");
//        graphicsUI = root.Q<VisualElement>("GraphicsPage");
//        achievemntsUI = root.Q<VisualElement>("AchievementsPage");
//        controllerUI = root.Q<VisualElement>("ControllerPage");
//        instructionsUI = root.Q<VisualElement>("InstructionsPage");
//        gameModePage = root.Q<VisualElement>("GameModesPage");
//        youWinPage = root.Q<VisualElement>("YouWinMenu");

//        contrastSlider = settingsUI.Q<Slider>("Contrast");
//        brightnessSlider = settingsUI.Q<Slider>("Brightness");
//        fullscreenRadioButton = videoSettingsUI.Q<RadioButton>("Fullscreen");
//        ConnectButton = gameOverUI.Q<Button>("ConnectButton");
//        DisconnectButton = gameOverUI.Q<Button>("DisConnectButton");
//        AddressText = gameOverUI.Q<Label>("DisconnectButton");
//        ConnectedText = gameOverUI.Q<Label>("DisconnectButton");
//        EthBalanceText = gameOverUI.Q<Label>("EthBalanceTexct");
//        ClaimedTokenBalanceText = gameOverUI.Q<Label>("CysBalanceTexct");
//        CustomTokenBalanceText = gameOverUI.Q<Label>("CysBalanceTexct");

//        if (contrastSlider != null)
//            contrastSlider.RegisterValueChangedCallback(evt => AdjustContrast(evt.newValue));
//        if (brightnessSlider != null)
//            brightnessSlider.RegisterValueChangedCallback(evt => AdjustBrightness(evt.newValue));
//        if (fullscreenRadioButton != null)
//            fullscreenRadioButton.RegisterValueChangedCallback(evt => ToggleFullscreen(evt.newValue));

//        // Back button handling
//        Button backButton = settingsUI.Q<Button>("BackButton");
//        if (backButton != null)
//            backButton.clicked += Back;

//        // Race mode UI element
//        timeLabel = pauseMenuUI.Q<Label>("Scoretext");
//        scoreLabel = pauseMenuUI.Q<Label>("Scoretext");
//        winScoreLabel = youWinPage.Q<Label>("YourScoreText");
//        if (timeLabel == null)
//        {
//            Debug.LogError("ScoreText label not found in the UI Document!");
//        }
//        else
//        {
//            Debug.Log("ScoreText label found: " + timeLabel);
//            timeLabel.text = "Test Update";
//            timeLabel.MarkDirtyRepaint();
//        }

//        AudioManager.Instance.PlayMenuMusic();
//        ResetUI();

//        Time.timeScale = 0f; // Start paused
//        isPaused = true;
//        isGameOver = false;
//        isGameWon = false;
//    }

//    private void CheckAndRemoveDuplicateAudioListeners()
//    {
//        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
//        if (listeners.Length > 1)
//        {
//            Debug.LogWarning($"Found {listeners.Length} AudioListeners in the scene. Ensuring only one remains.");
//            // Keep the first AudioListener (likely on the main camera) and disable/remove others
//            for (int i = 1; i < listeners.Length; i++)
//            {
//                if (listeners[i].gameObject != Camera.main.gameObject)
//                {
//                    Destroy(listeners[i]);
//                }
//                else if (listeners[i].gameObject == gameObject)
//                {
//                    Destroy(listeners[i]); // Remove from GameManager if it’s not the main camera
//                }
//            }
//        }
//        else if (listeners.Length == 0)
//        {
//            Debug.LogError("No AudioListener found in the scene! Adding one to the main camera.");
//            if (Camera.main != null)
//            {
//                Camera.main.gameObject.AddComponent<AudioListener>();
//            }
//        }
//    }

//    public void ResetUI()
//    {
//        if (mainMenuUI != null)
//        {
//            mainMenuUI.style.display = DisplayStyle.Flex;
//        }
//        if (optionsMenuUI != null) optionsMenuUI.style.display = DisplayStyle.None;
//        if (pauseMenuUI != null) pauseMenuUI.style.display = DisplayStyle.None;
//        if (resumeMenuUI != null) resumeMenuUI.style.display = DisplayStyle.None;
//        if (gameOverUI != null) gameOverUI.style.display = DisplayStyle.None;
//        if (audioSettingsUI != null) audioSettingsUI.style.display = DisplayStyle.None;
//        if (videoSettingsUI != null) videoSettingsUI.style.display = DisplayStyle.None;
//        if (settingsUI != null) settingsUI.style.display = DisplayStyle.None;
//        if (graphicsUI != null) graphicsUI.style.display = DisplayStyle.None;
//        if (achievemntsUI != null) achievemntsUI.style.display = DisplayStyle.None;
//        if (controllerUI != null) controllerUI.style.display = DisplayStyle.None;
//        if (instructionsUI != null) instructionsUI.style.display = DisplayStyle.None;
//        if (gameModePage != null) gameModePage.style.display = DisplayStyle.None;

//        menuHistory.Clear();
//        menuHistory.Push(mainMenuUI); // Start from the main menu

//        ResetGameState();
//    }

//    private void ResetGameState()
//    {
//        Debug.Log("Game state reset before returning to Main Menu.");
//        raceTime = 0f;
//        score = 0;
//        isRacing = false;
//        UpdateRaceUI();
//    }

//    public void SetGameMode(int mode)
//    {
//        currentMode = (GameMode)mode;
//        StartGame();
//    }

//    public void StartGame()
//    {
//        Debug.Log("Game Started!");
//        Debug.Log("Starting game with mode: " + currentMode);
//        if (mainMenuUI != null)
//            mainMenuUI.style.display = DisplayStyle.None;
//        if (gameModePage != null)
//            gameModePage.style.display = DisplayStyle.None;

//        raceTime = 0f;
//        score = 0;
//        isRacing = (currentMode == GameMode.Race);

//        Time.timeScale = 1f;
//        AudioManager.Instance.PlayGameplayMusic();
//        Debug.Log("Gameplay music!");
//        AudioManager.Instance.PlayWaterSound();
//        isPaused = false;
//        isGameOver = false;
//        isGameWon = false;
//        UpdateRaceUI();
//        ShowMenu(pauseMenuUI, true);
//    }

//    public void QuitGame()
//    {
//        Debug.Log("Quitting Game...");
//        Application.Quit();
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
//    }

//    public void GameOver()
//    {
//        Debug.Log("Game Over!");
//        isGameOver = true;
//        AudioManager.Instance.PlayGameOverMusic();
//        Time.timeScale = 0f;
//        ShowMenu(gameOverUI);
//        if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
//        if (DisconnectButton != null)
//        {
//            DisconnectButton.style.display = DisplayStyle.None;
//        }
//        if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
//        if (AddressText != null) AddressText.style.display = DisplayStyle.None;
//        if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
//        if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
//        if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
//    }

//    public void RestartGame()
//    {
//        Debug.Log("Restarting Game...");
//        Time.timeScale = 1f;
//        ResetUI();
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void ShowMenu(VisualElement menu, bool isPauseMenu = false)
//    {
//        if (menuHistory.Count > 0)
//            menuHistory.Peek().style.display = DisplayStyle.None;

//        if (menu != null)
//        {
//            menu.style.display = DisplayStyle.Flex;
//            menuHistory.Push(menu);
//            if (!isPauseMenu)
//                Time.timeScale = 0f;
//        }
//        else
//        {
//            menuHistory.Clear();
//            Time.timeScale = 1f;
//        }
//    }

//    public void ShowVideoSettings() => ShowMenu(videoSettingsUI);
//    public void ShowSettingsPage() => ShowMenu(settingsUI);
//    public void ShowGraphicsPage() => ShowMenu(graphicsUI);
//    public void ShowAudioSettings() => ShowMenu(audioSettingsUI);
//    public void ShowAchievementsUI() => ShowMenu(achievemntsUI);
//    public void ShowControllerUI() => ShowMenu(controllerUI);
//    public void ShowInstructionsUi() => ShowMenu(instructionsUI);

//    public void Back()
//    {
//        if (menuHistory.Count > 1)
//        {
//            VisualElement currentMenu = menuHistory.Pop();
//            currentMenu.style.display = DisplayStyle.None;
//            VisualElement previousMenu = menuHistory.Peek();
//            previousMenu.style.display = DisplayStyle.Flex;
//            Debug.Log("Returning to previous menu: " + previousMenu.name);
//        }
//        else
//        {
//            ShowMenu(mainMenuUI);
//        }
//    }

//    public void PauseGame()
//    {
//        Time.timeScale = 0f;
//        AudioManager.Instance.PlayPauseMusic();
//        ShowMenu(resumeMenuUI);
//        Debug.Log("Game Paused");
//    }

//    public void AdjustContrast(float value) => RenderSettings.ambientIntensity = value;
//    public void AdjustBrightness(float value) => RenderSettings.ambientLight = new Color(value, value, value);

//    public void ToggleFullscreen(bool isFullscreen)
//    {
//        Debug.Log("ToggleFullscreen called! New State: " + isFullscreen);
//        if (isFullscreen)
//        {
//            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
//            Screen.SetResolution(1920, 1080, true);
//        }
//        else
//        {
//            Screen.fullScreenMode = FullScreenMode.Windowed;
//            Screen.SetResolution(1280, 720, false);
//        }
//        Debug.Log($"Fullscreen: {Screen.fullScreen}, Mode: {Screen.fullScreenMode}, Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}");
//    }

//    public void ReturnToMainMenu()
//    {
//        Time.timeScale = 0f;
//        ResetGameState();
//        SceneManager.LoadScene("GameScene");
//    }

//    public void ResumeGame()
//    {
//        Time.timeScale = 1f;
//        AudioManager.Instance.PlayGameplayMusic();
//        ShowMenu(pauseMenuUI, true);
//    }

//    public void ShowGameModemenu()
//    {
//        Debug.Log("Start Button Clicked! Showing Game Mode Menu...");
//        ShowMenu(gameModePage);
//    }

//    private void UpdateRaceUI()
//    {
//        string displayText = "";
//        if (currentMode == GameMode.Race)
//        {
//            displayText = "Time: " + raceTime.ToString("F2");
//        }
//        else if (currentMode == GameMode.Survival)
//        {
//            displayText = "Score: " + score;
//        }

//        Debug.Log("Updating Race UI with text: " + displayText);

//        if (timeLabel != null)
//            timeLabel.text = displayText;
//        if (scoreLabel != null && scoreLabel != timeLabel)
//            scoreLabel.text = displayText;
//    }

//    public void IncrementScore()
//    {
//        score++;
//        UpdateRaceUI();
//    }

//    public void EndGame()
//    {
//        isRacing = false;
//        Time.timeScale = 0f;
//    }

//    public void GameWin()
//    {
//        Debug.Log("Game Won!");
//        isGameWon = true;
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayGameWinMusic();
//            Time.timeScale = 0f;
//        }
//        else
//        {
//            Debug.LogWarning("AudioManager instance is null! Make sure it's in the scene.");
//        }

//        ShowMenu(youWinPage);

//        string displayText = "";
//        if (currentMode == GameMode.Race)
//        {
//            displayText = "Time used: " + raceTime.ToString("F2");
//        }
//        else if (currentMode == GameMode.Survival)
//        {
//            displayText = "Your Score: " + score;
//        }

//        if (winScoreLabel != null)
//            winScoreLabel.text = displayText;
//    }

//    public async void Connect()
//    {
//        if (thirdweb == null)
//        {
//            Debug.LogError("Cannot connect: ThirdwebManager is not initialized.");
//            return;
//        }

//        try
//        {
//            var options = new WalletOptions(
//                provider: WalletProvider.WalletConnectWallet,
//                chainId: 84532
//            );

//            Debug.Log("Initiating wallet connection...");
//            wallet = await ThirdwebManager.Instance.ConnectWallet(options);
//            walletAddress = await wallet.GetAddress();
//            Debug.Log($"Wallet connected successfully! Address: {walletAddress}");

//            var balance = await wallet.GetBalance(chainId: ActiveChainId);
//            var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
//            Debug.Log($"Wallet balance: {balanceEth}");

//            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.None;
//            if (DisconnectButton != null)
//            {
//                DisconnectButton.style.display = DisplayStyle.Flex;
//            }
//            if (ConnectedText != null)
//            {
//                ConnectedText.style.display = DisplayStyle.Flex;
//                ConnectedText.text = "Connected";
//            }
//            if (AddressText != null && !string.IsNullOrEmpty(walletAddress))
//            {
//                AddressText.style.display = DisplayStyle.Flex;
//                string shortAddress = $"{walletAddress.Substring(0, 3)}...{walletAddress.Substring(walletAddress.Length - 3)}";
//                AddressText.text = shortAddress;
//            }
//        }
//        catch (System.Exception ex)
//        {
//            Debug.LogError($"Failed to connect wallet: {ex.Message}");
//            wallet = null;
//            walletAddress = null;

//            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
//            if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None;
//            if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
//            if (AddressText != null) AddressText.style.display = DisplayStyle.None;
//            if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
//            if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
//            if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
//        }
//    }

//    public async void Disconnect()
//    {
//        if (wallet == null)
//        {
//            Debug.LogWarning("No wallet to disconnect.");
//            return;
//        }

//        try
//        {
//            Debug.Log("Disconnect function called! Disconnecting wallet...");
//            await wallet.Disconnect();
//            wallet = null;
//            walletAddress = null;
//            Debug.Log("Wallet disconnected successfully.");

//            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
//            if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None;
//            if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
//            if (AddressText != null) AddressText.style.display = DisplayStyle.None;
//            if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
//            if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
//            if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
//        }
//        catch (System.Exception ex)
//        {
//            Debug.LogError($"Failed to disconnect wallet: {ex.Message}");
//        }
//    }

//    private void OnDestroy()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded; // Clean up delegate
//    }
//}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Thirdweb;
using Thirdweb.Unity;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isGameWon = false;
    private UIDocument uiDocument;

    // UI Elements
    private ThirdwebManager thirdweb;
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
    private VisualElement walletSelectionUI; // New wallet selection UI
    private Label AddressText;
    private Label ConnectedText;
    private Label EthBalanceText;
    private Label CustomTokenBalanceText;
    private Label ClaimedTokenBalanceText;
    private Slider contrastSlider;
    private Slider brightnessSlider;
    private RadioButton fullscreenRadioButton;
    private Button ConnectButton;
    private Button DisconnectButton;

    private Stack<VisualElement> menuHistory = new Stack<VisualElement>();

    public enum GameMode { Race, Survival }
    public GameMode currentMode = GameMode.Race;

    public float shadowSpeed = 5f;

    private float raceTime = 0f;
    private int score = 0;
    private bool isRacing = false;
    private Label timeLabel;
    private Label scoreLabel;
    private Label winScoreLabel;

    private IThirdwebWallet wallet;
    private string walletAddress;
    [field: SerializeField, Header("Wallet Options")]
    private ulong ActiveChainId = 84532;

    private void Awake()
    {
        thirdweb = FindObjectOfType<ThirdwebManager>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            CheckAndRemoveDuplicateAudioListeners();
            Destroy(gameObject);
            return;
        }

        if (thirdweb == null)
        {
            Debug.LogError("ThirdwebManager not found in the scene!");
        }

        AudioListener audioListener = GetComponent<AudioListener>();
        if (audioListener != null)
        {
            Destroy(audioListener);
            Debug.Log("Removed AudioListener from GameManager.");
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
        CheckAndRemoveDuplicateAudioListeners();

        uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found!");
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
        walletSelectionUI = root.Q<VisualElement>("WalletSelectionUI");

        contrastSlider = settingsUI.Q<Slider>("Contrast");
        brightnessSlider = settingsUI.Q<Slider>("Brightness");
        fullscreenRadioButton = videoSettingsUI.Q<RadioButton>("Fullscreen");
        ConnectButton = gameOverUI.Q<Button>("ConnectButton");
        DisconnectButton = gameOverUI.Q<Button>("DisConnectButton");
        AddressText = gameOverUI.Q<Label>("DisconnectButton");
        ConnectedText = gameOverUI.Q<Label>("DisconnectButton");
        EthBalanceText = gameOverUI.Q<Label>("EthBalanceTexct");
        ClaimedTokenBalanceText = gameOverUI.Q<Label>("CysBalanceTexct");
        CustomTokenBalanceText = gameOverUI.Q<Label>("CysBalanceTexct");

        if (contrastSlider != null)
            contrastSlider.RegisterValueChangedCallback(evt => AdjustContrast(evt.newValue));
        if (brightnessSlider != null)
            brightnessSlider.RegisterValueChangedCallback(evt => AdjustBrightness(evt.newValue));
        if (fullscreenRadioButton != null)
            fullscreenRadioButton.RegisterValueChangedCallback(evt => ToggleFullscreen(evt.newValue));

        Button backButton = settingsUI.Q<Button>("BackButton");
        if (backButton != null)
            backButton.clicked += Back;

        timeLabel = pauseMenuUI.Q<Label>("Scoretext");
        scoreLabel = pauseMenuUI.Q<Label>("Scoretext");
        winScoreLabel = youWinPage.Q<Label>("YourScoreText");

        // Setup wallet selection UI
        //SetupWalletSelectionUI();

        AudioManager.Instance.PlayMenuMusic();
        ResetUI();

        Time.timeScale = 0f;
        isPaused = true;
        isGameOver = false;
        isGameWon = false;
    }

    private void SetupWalletSelectionUI()
    {
        if (walletSelectionUI == null)
        {
            Debug.LogError("WalletSelectionUI not found in UI Document! Please add it.");
            return;
        }

        walletSelectionUI.style.display = DisplayStyle.None;

        // Buttons for all supported wallet providers
        Button privateKeyButton = new Button(() => ConnectWithProvider(WalletProvider.PrivateKeyWallet)) { text = "Private Key" };
        Button inAppButton = new Button(() => ConnectWithProvider(WalletProvider.InAppWallet)) { text = "In-App Wallet" };
        Button walletConnectButton = new Button(() => ConnectWithProvider(WalletProvider.WalletConnectWallet)) { text = "WalletConnect" };
        Button metaMaskButton = new Button(() => ConnectWithProvider(WalletProvider.MetaMaskWallet)) { text = "MetaMask" };
        Button ecosystemButton = new Button(() => ConnectWithProvider(WalletProvider.EcosystemWallet)) { text = "Ecosystem Wallet" };

        walletSelectionUI.Add(privateKeyButton);
        walletSelectionUI.Add(inAppButton);
        walletSelectionUI.Add(walletConnectButton);
        walletSelectionUI.Add(metaMaskButton);
        walletSelectionUI.Add(ecosystemButton);

        // Optional styling
        privateKeyButton.style.width = 200;
        inAppButton.style.width = 200;
        walletConnectButton.style.width = 200;
        metaMaskButton.style.width = 200;
        ecosystemButton.style.width = 200;

        // ConnectButton triggers wallet selection
        if (ConnectButton != null)
        {
            ConnectButton.clicked += Connect; // Add new behavior
            // Note: Original Connect() is preserved below and can still be called separately
        }
    }

    private void ShowWalletSelection()
    {
        ShowMenu(walletSelectionUI);
    }

    private async void ConnectWithProvider(WalletProvider provider)
    {
        if (thirdweb == null)
        {
            Debug.LogError("ThirdwebManager not initialized.");
            return;
        }

        var options = new WalletOptions(
            provider: provider,
            chainId: ActiveChainId
        );

        // Special handling for providers requiring additional options
        if (provider == WalletProvider.InAppWallet)
        {
            options.InAppWalletOptions = new InAppWalletOptions(email: "user@example.com"); // Example email, adjust as needed
        }
        else if (provider == WalletProvider.EcosystemWallet)
        {
            options.EcosystemWalletOptions = new EcosystemWalletOptions(ecosystemId: "your-ecosystem-id"); // Replace with actual ID
        }

        try
        {
            Debug.Log($"Connecting with {provider}...");
            wallet = await ThirdwebManager.Instance.ConnectWallet(options);
            walletAddress = await wallet.GetAddress();
            Debug.Log($"Wallet connected: {walletAddress}");

            var balance = await wallet.GetBalance(chainId: ActiveChainId);
            var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
            Debug.Log($"Wallet balance: {balanceEth}");

            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.None;
            if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.Flex;
            if (ConnectedText != null)
            {
                ConnectedText.style.display = DisplayStyle.Flex;
                ConnectedText.text = "Connected";
            }
            if (AddressText != null && !string.IsNullOrEmpty(walletAddress))
            {
                AddressText.style.display = DisplayStyle.Flex;
                string shortAddress = $"{walletAddress.Substring(0, 3)}...{walletAddress.Substring(walletAddress.Length - 3)}";
                AddressText.text = shortAddress;
            }

            if (walletSelectionUI != null)
                walletSelectionUI.style.display = DisplayStyle.None;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to connect with {provider}: {ex.Message}");
            wallet = null;
            walletAddress = null;

            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
            if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None;
            if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
            if (AddressText != null) AddressText.style.display = DisplayStyle.None;
            if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
            if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
            if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
        }
    }

    // Original Connect function preserved
    public async void Connect()
    {
        if (thirdweb == null)
        {
            Debug.LogError("Cannot connect: ThirdwebManager is not initialized.");
            return;
        }

        try
        {
            var options = new WalletOptions(
                provider: WalletProvider.WalletConnectWallet,
                chainId: 84532
            );

            Debug.Log("Initiating wallet connection...");
            wallet = await ThirdwebManager.Instance.ConnectWallet(options);
            walletAddress = await wallet.GetAddress();
            Debug.Log($"Wallet connected successfully! Address: {walletAddress}");

            var balance = await wallet.GetBalance(chainId: ActiveChainId);
            var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
            Debug.Log($"Wallet balance: {balanceEth}");

            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.None;
            if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.Flex;
            if (ConnectedText != null)
            {
                ConnectedText.style.display = DisplayStyle.Flex;
                ConnectedText.text = "Connected";
            }
            if (AddressText != null && !string.IsNullOrEmpty(walletAddress))
            {
                AddressText.style.display = DisplayStyle.Flex;
                string shortAddress = $"{walletAddress.Substring(0, 3)}...{walletAddress.Substring(walletAddress.Length - 3)}";
                AddressText.text = shortAddress;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to connect wallet: {ex.Message}");
            wallet = null;
            walletAddress = null;

            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
            if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None;
            if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
            if (AddressText != null) AddressText.style.display = DisplayStyle.None;
            if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
            if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
            if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
        }
    }

    private void CheckAndRemoveDuplicateAudioListeners()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            Debug.LogWarning($"Found {listeners.Length} AudioListeners. Ensuring only one remains.");
            for (int i = 1; i < listeners.Length; i++)
            {
                if (listeners[i].gameObject != Camera.main.gameObject)
                {
                    Destroy(listeners[i]);
                }
                else if (listeners[i].gameObject == gameObject)
                {
                    Destroy(listeners[i]);
                }
            }
        }
        else if (listeners.Length == 0)
        {
            Debug.LogError("No AudioListener found! Adding to main camera.");
            if (Camera.main != null)
            {
                Camera.main.gameObject.AddComponent<AudioListener>();
            }
        }
    }

    public void ResetUI()
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
        if (walletSelectionUI != null) walletSelectionUI.style.display = DisplayStyle.None;

        menuHistory.Clear();
        menuHistory.Push(mainMenuUI);

        ResetGameState();
    }

    private void ResetGameState()
    {
        Debug.Log("Game state reset.");
        raceTime = 0f;
        score = 0;
        isRacing = false;
        UpdateRaceUI();
    }

    public void SetGameMode(int mode)
    {
        currentMode = (GameMode)mode;
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("Game Started! Mode: " + currentMode);
        if (mainMenuUI != null) mainMenuUI.style.display = DisplayStyle.None;
        if (gameModePage != null) gameModePage.style.display = DisplayStyle.None;

        raceTime = 0f;
        score = 0;
        isRacing = (currentMode == GameMode.Race);

        Time.timeScale = 1f;
        AudioManager.Instance.PlayGameplayMusic();
        AudioManager.Instance.PlayWaterSound();
        isPaused = false;
        isGameOver = false;
        isGameWon = false;
        UpdateRaceUI();
        ShowMenu(pauseMenuUI, true);
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
        AudioManager.Instance.PlayGameOverMusic();
        Time.timeScale = 0f;
        ShowMenu(gameOverUI);
        if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
        if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None;
        if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
        if (AddressText != null) AddressText.style.display = DisplayStyle.None;
        if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
        if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
        if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
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
            menuHistory.Peek().style.display = DisplayStyle.None;

        if (menu != null)
        {
            menu.style.display = DisplayStyle.Flex;
            menuHistory.Push(menu);
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
            Debug.Log("Returning to: " + previousMenu.name);
        }
        else
        {
            ShowMenu(mainMenuUI);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.PlayPauseMusic();
        ShowMenu(resumeMenuUI);
        Debug.Log("Game Paused");
    }

    public void AdjustContrast(float value) => RenderSettings.ambientIntensity = value;
    public void AdjustBrightness(float value) => RenderSettings.ambientLight = new Color(value, value, value);

    public void ToggleFullscreen(bool isFullscreen)
    {
        Debug.Log("ToggleFullscreen: " + isFullscreen);
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
        AudioManager.Instance.PlayGameplayMusic();
        ShowMenu(pauseMenuUI, true);
    }

    public void ShowGameModemenu()
    {
        Debug.Log("Showing Game Mode Menu...");
        ShowMenu(gameModePage);
    }

    private void UpdateRaceUI()
    {
        string displayText = currentMode == GameMode.Race ? "Time: " + raceTime.ToString("F2") : "Score: " + score;
        Debug.Log("Updating Race UI: " + displayText);

        if (timeLabel != null) timeLabel.text = displayText;
        if (scoreLabel != null && scoreLabel != timeLabel) scoreLabel.text = displayText;
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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameWinMusic();
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning("AudioManager is null!");
        }

        ShowMenu(youWinPage);

        string displayText = currentMode == GameMode.Race ? "Time used: " + raceTime.ToString("F2") : "Your Score: " + score;
        if (winScoreLabel != null) winScoreLabel.text = displayText;
    }

    public async void Disconnect()
    {
        if (wallet == null)
        {
            Debug.LogWarning("No wallet to disconnect.");
            return;
        }

        try
        {
            Debug.Log("Disconnecting wallet...");
            await wallet.Disconnect();
            wallet = null;
            walletAddress = null;
            Debug.Log("Wallet disconnected.");

            if (ConnectButton != null) ConnectButton.style.display = DisplayStyle.Flex;
            if (DisconnectButton != null) DisconnectButton.style.display = DisplayStyle.None;
            if (ConnectedText != null) ConnectedText.style.display = DisplayStyle.None;
            if (AddressText != null) AddressText.style.display = DisplayStyle.None;
            if (EthBalanceText != null) EthBalanceText.style.display = DisplayStyle.None;
            if (CustomTokenBalanceText != null) CustomTokenBalanceText.style.display = DisplayStyle.None;
            if (ClaimedTokenBalanceText != null) ClaimedTokenBalanceText.style.display = DisplayStyle.None;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to disconnect: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}