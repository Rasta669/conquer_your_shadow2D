//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using TMPro;
//using Thirdweb;
//using Thirdweb.Unity;
//using System.Threading.Tasks;
//using System;

//public class NewGameManager : MonoBehaviour
//{
//    public static NewGameManager Instance { get; private set; }

//    private bool isPaused = false;
//    private bool isGameOver = false;
//    private bool isGameWon = false;

//    // UI Canvases
//    [SerializeField] private GameObject mainMenuCanvas;
//    [SerializeField] private GameObject gameModeMenuCanvas;
//    [SerializeField] private GameObject gameplayCanvas;
//    [SerializeField] private GameObject pauseMenuCanvas;
//    [SerializeField] private GameObject gameOverCanvas;

//    // Main Menu Buttons
//    [SerializeField] private Button startButton;

//    // Game Mode Menu Buttons
//    [SerializeField] private Button raceModeButton;
//    [SerializeField] private Button survivalModeButton;
//    //[SerializeField] private Button gameModeQuitButton;
//    [SerializeField] private Button gameModeBackButton;

//    // Gameplay Canvas Elements
//    [SerializeField] private TextMeshProUGUI gameplayScoreOrTimeLabel;
//    [SerializeField] private Button pauseButton;

//    // Pause Menu Buttons
//    [SerializeField] private Button resumeButton;
//    [SerializeField] private Button pauseRestartButton;
//    [SerializeField] private Button pauseBackButton;

//    // Game Over Menu Buttons
//    [SerializeField] private Button gameOverRestartButton;
//    [SerializeField] private Button gameOverBackButton;
//    [SerializeField] private Button connectButton;
//    [SerializeField] private Button disconnectButton;

//    // Shared Labels (Game Over)
//    [SerializeField] private TextMeshProUGUI gameOverScoreOrTimeLabel;
//    //[SerializeField] private TextMeshProUGUI ClaimTokenContractAddress;
//    [SerializeField] private TextMeshProUGUI connectedLabel;
//    [SerializeField] private TextMeshProUGUI addressLabel;
//    [SerializeField] private TextMeshProUGUI ethBalanceLabel;
//    [field: SerializeField]
//    public string ClaimTokenAmount { get; set; } // Amount of tokens to claim (e.g., "100")
//    [field: SerializeField, Header("Claim Token Options")]
//    public string ClaimTokenContractAddress { get; set; } // Contract address of the claim token (Drop contract)
//    [field: SerializeField]
//    public TextMeshProUGUI ClaimedTokenBalanceText { get; set; } // The claimed token balance text (TextMeshProUGUI)

//    private ThirdwebManager thirdweb;
//    private IThirdwebWallet wallet;
//    private string walletAddress;
//    [SerializeField, Header("Wallet Options")]
//    private ulong ActiveChainId = 84532;

//    public enum GameMode { Race, Survival }
//    public GameMode currentMode = GameMode.Race;

//    private float raceTime = 0f;
//    private int score = 0;
//    private bool isRacing = false;

//    private void Awake()
//    {
//        thirdweb = FindObjectOfType<ThirdwebManager>();
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            SceneManager.sceneLoaded += OnSceneLoaded;
//        }
//        else
//        {
//            CheckAndRemoveDuplicateAudioListeners();
//            Destroy(gameObject);
//            return;
//        }

//        if (thirdweb == null)
//        {
//            Debug.LogError("ThirdwebManager not found in the scene!");
//        }

//        AudioListener audioListener = GetComponent<AudioListener>();
//        if (audioListener != null)
//        {
//            Destroy(audioListener);
//            Debug.Log("Removed AudioListener from GameManager.");
//        }
//    }

//    private void Start()
//    {
//        // Assign button listeners
//        startButton.onClick.AddListener(ShowGameModeMenu);

//        raceModeButton.onClick.AddListener(() => StartGame(GameMode.Race));
//        survivalModeButton.onClick.AddListener(() => StartGame(GameMode.Survival));
//        //gameModeQuitButton.onClick.AddListener(QuitGame);
//        gameModeBackButton.onClick.AddListener(ReturnToMainMenu);

//        pauseButton.onClick.AddListener(PauseGame);
//        resumeButton.onClick.AddListener(ResumeGame);
//        pauseRestartButton.onClick.AddListener(RestartGame);
//        pauseBackButton.onClick.AddListener(ReturnToMainMenu);

//        gameOverRestartButton.onClick.AddListener(RestartGame);
//        gameOverBackButton.onClick.AddListener(ReturnToMainMenu);
//        //connectButton.onClick.AddListener(Connect);
//        //disconnectButton.onClick.AddListener(Disconnect);

//        ResetUI();
//    }

//    private void Update()
//    {
//        if (isRacing && currentMode == GameMode.Race && !isPaused && !isGameOver)
//        {
//            raceTime += Time.deltaTime;
//            UpdateUI();
//        }

//        // Pause toggle with Escape key
//        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && gameplayCanvas.activeSelf)
//        {
//            if (isPaused) ResumeGame();
//            else PauseGame();
//        }
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        CheckAndRemoveDuplicateAudioListeners();
//        ResetUI();
//    }

//    private void CheckAndRemoveDuplicateAudioListeners()
//    {
//        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
//        if (listeners.Length > 1)
//        {
//            Debug.LogWarning($"Found {listeners.Length} AudioListeners. Ensuring only one remains.");
//            for (int i = 1; i < listeners.Length; i++)
//            {
//                if (listeners[i].gameObject != Camera.main.gameObject)
//                {
//                    Destroy(listeners[i]);
//                }
//            }
//        }
//        else if (listeners.Length == 0)
//        {
//            Debug.LogError("No AudioListener found! Adding to main camera.");
//            if (Camera.main != null)
//            {
//                Camera.main.gameObject.AddComponent<AudioListener>();
//            }
//        }
//    }

//    private void ResetUI()
//    {
//        mainMenuCanvas.SetActive(true);
//        gameModeMenuCanvas.SetActive(false);
//        gameplayCanvas.SetActive(false);
//        pauseMenuCanvas.SetActive(false);
//        gameOverCanvas.SetActive(false);

//        connectButton.gameObject.SetActive(true);
//        disconnectButton.gameObject.SetActive(false);
//        connectedLabel.gameObject.SetActive(false);
//        addressLabel.gameObject.SetActive(false);
//        ethBalanceLabel.gameObject.SetActive(false);
//        ClaimedTokenBalanceText.gameObject.SetActive(false);

//        ResetGameState();
//        Time.timeScale = 0f;
//    }

//    private void ResetGameState()
//    {
//        raceTime = 0f;
//        score = 0;
//        isRacing = false;
//        isPaused = false;
//        isGameOver = false;
//        isGameWon = false;
//        UpdateUI();
//    }

//    private void UpdateUI()
//    {
//        string displayText = currentMode == GameMode.Race ? $"Time: {raceTime:F2}" : $"Score: {score}";
//        if (gameplayScoreOrTimeLabel != null) gameplayScoreOrTimeLabel.text = displayText;
//        if (gameOverScoreOrTimeLabel != null) gameOverScoreOrTimeLabel.text = displayText;
//    }

//    public void ShowGameModeMenu()
//    {
//        mainMenuCanvas.SetActive(false);
//        gameModeMenuCanvas.SetActive(true);
//    }

//    private void StartGame(GameMode mode)
//    {
//        currentMode = mode;
//        Debug.Log("Game Started! Mode: " + currentMode);
//        gameModeMenuCanvas.SetActive(false);
//        gameplayCanvas.SetActive(true);

//        raceTime = 0f;
//        score = 0;
//        isRacing = (currentMode == GameMode.Race);
//        Time.timeScale = 1f;
//        AudioManager.Instance.PlayGameplayMusic();
//        AudioManager.Instance.PlayWaterSound();
//        UpdateUI();
//    }

//    private void PauseGame()
//    {
//        isPaused = true;
//        Time.timeScale = 0f;
//        AudioManager.Instance.PlayPauseMusic();
//        gameplayCanvas.SetActive(false);
//        pauseMenuCanvas.SetActive(true);
//        Debug.Log("Game Paused");
//    }

//    private void ResumeGame()
//    {
//        isPaused = false;
//        Time.timeScale = 1f;
//        AudioManager.Instance.PlayGameplayMusic();
//        pauseMenuCanvas.SetActive(false);
//        gameplayCanvas.SetActive(true);
//        Debug.Log("Game Resumed");
//    }

//    public void GameOver()
//    {
//        Debug.Log("Game Over!");
//        isGameOver = true;
//        Time.timeScale = 0f;
//        AudioManager.Instance.PlayGameOverMusic();
//        gameplayCanvas.SetActive(false);
//        pauseMenuCanvas.SetActive(false);
//        gameOverCanvas.SetActive(true);
//        UpdateUI();
//    }

//    public void GameWin()
//    {
//        Debug.Log("Game Won!");
//        isGameWon = true;
//        Time.timeScale = 0f;
//        AudioManager.Instance.PlayGameWinMusic();
//        gameplayCanvas.SetActive(false);
//        pauseMenuCanvas.SetActive(false);
//        gameOverCanvas.SetActive(true);
//        UpdateUI();
//    }

//    public void ReturnToMainMenu()
//    {
//        Debug.Log("Returning to Main Menu...");
//        ResetGameState();
//        mainMenuCanvas.SetActive(true);
//        gameModeMenuCanvas.SetActive(false);
//        gameplayCanvas.SetActive(false);
//        pauseMenuCanvas.SetActive(false);
//        gameOverCanvas.SetActive(false);
//    }

//    public void RestartGame()
//    {
//        Debug.Log("Restarting Game...");
//        ResetGameState();
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void QuitGame()
//    {
//        Debug.Log("Quitting Game...");
//        Application.Quit();
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
//    }

//    public void IncrementScore()
//    {
//        score++;
//        UpdateUI();
//    }

//    public async void Connect()
//    {
//        if (thirdweb == null || !thirdweb.isActiveAndEnabled)
//        {
//            Debug.LogError("Cannot connect: ThirdwebManager is not initialized.");
//            return;
//        }

//        try
//        {
//            var options = new WalletOptions(
//                provider: WalletProvider.WalletConnectWallet,
//                chainId: ActiveChainId
//            );

//            Debug.Log("Initiating wallet connection...");
//            wallet = await ThirdwebManager.Instance.ConnectWallet(options);

//            // If wallet is null, retry once
//            if (wallet == null)
//            {
//                Debug.LogWarning("First connection attempt failed, retrying...");
//                wallet = await ThirdwebManager.Instance.ConnectWallet(options);
//            }

//            walletAddress = await wallet.GetAddress();
//            Debug.Log($"Wallet connected successfully! Address: {walletAddress}");

//            var balance = await wallet.GetBalance(chainId: ActiveChainId);
//            var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
//            Debug.Log($"Wallet balance: {balanceEth}");

//            // Get the updated token balance and display it
//            var contract = await ThirdwebManager.Instance.GetContract(ClaimTokenContractAddress, ActiveChainId);
//            var tokenBalance = await contract.ERC20_BalanceOf(walletAddress);
//            var decimals = await contract.ERC20_Decimals();
//            var tokenBalanceFormatted = Utils.ToEth(tokenBalance.ToString(), (int)decimals, addCommas: true);
//            Debug.Log($"Updated token balance for {walletAddress}: {tokenBalanceFormatted}");

//            connectButton.gameObject.SetActive(false);
//            disconnectButton.gameObject.SetActive(true);
//            connectedLabel.gameObject.SetActive(true);
//            connectedLabel.text = "Connected";
//            addressLabel.gameObject.SetActive(true);
//            addressLabel.text = $"{walletAddress.Substring(0, 3)}...{walletAddress.Substring(walletAddress.Length - 3)}";
//            ethBalanceLabel.gameObject.SetActive(true);
//            ethBalanceLabel.text = $"ETH: {balanceEth}";
//            ClaimedTokenBalanceText.gameObject.SetActive(true);
//            ClaimedTokenBalanceText.text = $"CYS: {tokenBalanceFormatted}";
//        }
//        catch (System.Exception ex)
//        {
//            Debug.LogError($"Failed to connect wallet: {ex.Message}");
//            wallet = null;
//            walletAddress = null;

//            connectButton.gameObject.SetActive(true);
//            disconnectButton.gameObject.SetActive(false);
//            connectedLabel.gameObject.SetActive(false);
//            addressLabel.gameObject.SetActive(false);
//            ethBalanceLabel.gameObject.SetActive(false);
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
//            Debug.Log("Disconnecting wallet...");
//            await wallet.Disconnect();
//            wallet = null;
//            walletAddress = null;
//            Debug.Log("Wallet disconnected.");

//            connectButton.gameObject.SetActive(true);
//            disconnectButton.gameObject.SetActive(false);
//            connectedLabel.gameObject.SetActive(false);
//            addressLabel.gameObject.SetActive(false);
//            ethBalanceLabel.gameObject.SetActive(false);
//        }
//        catch (System.Exception ex)
//        {
//            Debug.LogError($"Failed to disconnect: {ex.Message}");
//        }
//    }


//    public async void ClaimToken()
//    {
//        if (thirdweb == null)
//        {
//            Debug.LogError("Cannot claim token: ThirdwebManager is not initialized.");
//            return;
//        }

//        if (wallet == null)
//        {
//            Debug.LogError("Cannot claim token: Wallet is not connected. Please connect the wallet first.");
//            return;
//        }

//        if (string.IsNullOrEmpty(ClaimTokenContractAddress) || !ClaimTokenContractAddress.StartsWith("0x") || ClaimTokenContractAddress.Length != 42)
//        {
//            Debug.LogError("Invalid claim token contract address. Please provide a valid Ethereum address (e.g., 0x...)");
//            return;
//        }

//        if (string.IsNullOrEmpty(ClaimTokenAmount) || !float.TryParse(ClaimTokenAmount, out float claimAmount) || claimAmount <= 0)
//        {
//            Debug.LogError("Invalid claim amount. Please provide a valid amount (e.g., 100)");
//            return;
//        }

//        try
//        {
//            Debug.Log($"Claiming {ClaimTokenAmount} tokens to {walletAddress} from contract {ClaimTokenContractAddress}...");

//            // Ensure the wallet is on the correct chain before claiming
//            Debug.Log($"Ensuring wallet is on chain {ActiveChainId}...");
//            if (wallet is WalletConnectWallet walletConnect)
//            {
//                await walletConnect.EnsureCorrectNetwork(ActiveChainId);
//            }
//            else
//            {
//                Debug.LogWarning("Wallet is not a WalletConnectWallet; cannot ensure correct network.");
//            }

//            // Add a delay to ensure the chain switch takes effect
//            Debug.Log("Waiting for chain switch to take effect...");
//            await Task.Delay(10000); // Wait 10 seconds

//            // Create a contract instance for the Drop contract
//            var contract = await ThirdwebManager.Instance.GetContract(ClaimTokenContractAddress, ActiveChainId);

//            // Get the token's decimals to convert the amount to the correct units
//            var decimals = await contract.ERC20_Decimals();
//            Debug.Log($"Claim token decimals: {decimals}");

//            // Convert the claim amount to the correct units (e.g., 100 tokens with 18 decimals = 100 * 10^18)
//            string claimAmountInWei = Utils.ToEth(ClaimTokenAmount);
//            Debug.Log($"Converted claim amount to Wei (string): {claimAmountInWei}");

//            // Call the DropERC20_Claim function to claim the tokens to the wallet's own address
//            var transactionResult = await contract.DropERC20_Claim(wallet, walletAddress, claimAmountInWei);
//            Debug.Log($"Tokens claimed successfully! Transaction Hash: {transactionResult.TransactionHash}");
//            await Task.Delay(10000); // Wait 10 seconds

//            // Get the updated token balance and display it
//            var tokenBalance = await contract.ERC20_BalanceOf(walletAddress);
//            var tokenBalanceFormatted = Utils.ToEth(tokenBalance.ToString(), (int)decimals, addCommas: true);
//            Debug.Log($"Updated token balance for {walletAddress}: {tokenBalanceFormatted}");
//            if (ClaimedTokenBalanceText != null)
//            {
//                ClaimedTokenBalanceText.gameObject.SetActive(true);
//                ClaimedTokenBalanceText.text = $"CYS: {tokenBalanceFormatted}";
//            }
//        }
//        catch (System.Exception ex)
//        {
//            Debug.LogError($"Failed to claim tokens: {ex.Message}");
//        }
//    }

//    private void OnDestroy()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;

//        // Remove listeners to prevent memory leaks
//        startButton.onClick.RemoveAllListeners();
//        raceModeButton.onClick.RemoveAllListeners();
//        survivalModeButton.onClick.RemoveAllListeners();
//        //gameModeQuitButton.onClick.RemoveAllListeners();
//        gameModeBackButton.onClick.RemoveAllListeners();
//        pauseButton.onClick.RemoveAllListeners();
//        resumeButton.onClick.RemoveAllListeners();
//        pauseRestartButton.onClick.RemoveAllListeners();
//        pauseBackButton.onClick.RemoveAllListeners();
//        gameOverRestartButton.onClick.RemoveAllListeners();
//        gameOverBackButton.onClick.RemoveAllListeners();
//        connectButton.onClick.RemoveAllListeners();
//        disconnectButton.onClick.RemoveAllListeners();
//    }
//}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Thirdweb;
using Thirdweb.Unity;
using System.Threading.Tasks;
using System;

public class NewGameManager : MonoBehaviour
{
    public static NewGameManager Instance { get; private set; }

    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isGameWon = false;

    // UI Canvases
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject gameModeMenuCanvas;
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    // Main Menu Buttons
    [SerializeField] private Button startButton;

    // Game Mode Menu Buttons
    [SerializeField] private Button raceModeButton;
    [SerializeField] private Button survivalModeButton;
    [SerializeField] private Button gameModeBackButton;

    // Gameplay Canvas Elements
    [SerializeField] private TextMeshProUGUI gameplayScoreOrTimeLabel;
    [SerializeField] private Button pauseButton;

    // Pause Menu Buttons
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseRestartButton;
    [SerializeField] private Button pauseBackButton;

    // Game Over Menu Buttons
    [SerializeField] private Button gameOverRestartButton;
    [SerializeField] private Button gameOverBackButton;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button disconnectButton;

    // Shared Labels (Game Over)
    [SerializeField] private TextMeshProUGUI gameOverScoreOrTimeLabel;
    [SerializeField] private TextMeshProUGUI connectedLabel;
    [SerializeField] private TextMeshProUGUI addressLabel;
    [SerializeField] private TextMeshProUGUI ethBalanceLabel;
    [field: SerializeField]
    public string ClaimTokenAmount { get; set; } // Amount of tokens to claim (e.g., "100")
    [field: SerializeField, Header("Claim Token Options")]
    public string ClaimTokenContractAddress { get; set; } // Contract address of the claim token (Drop contract)
    [field: SerializeField]
    public TextMeshProUGUI ClaimedTokenBalanceText { get; set; } // The claimed token balance text (TextMeshProUGUI)

    private ThirdwebManager thirdweb;
    private IThirdwebWallet wallet;
    private string walletAddress;
    [SerializeField, Header("Wallet Options")]
    private ulong ActiveChainId = 84532;

    public enum GameMode { Race, Survival }
    public GameMode currentMode = GameMode.Race;

    private float raceTime = 0f;
    private int score = 0;
    private bool isRacing = false;

    private void Awake()
    {
        thirdweb = FindObjectOfType<ThirdwebManager>();
        //thirdweb = ThirdwebManager.Instance;
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

    private void Start()
    {
        // Assign button listeners
        startButton.onClick.AddListener(ShowGameModeMenu);

        raceModeButton.onClick.AddListener(() => StartGame(GameMode.Race));
        survivalModeButton.onClick.AddListener(() => StartGame(GameMode.Survival));
        gameModeBackButton.onClick.AddListener(ReturnToMainMenu);

        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        pauseRestartButton.onClick.AddListener(RestartGame);
        pauseBackButton.onClick.AddListener(ReturnToMainMenu);

        gameOverRestartButton.onClick.AddListener(RestartGame);
        gameOverBackButton.onClick.AddListener(ReturnToMainMenu);

        ResetUI();
    }

    private void Update()
    {
        if (isRacing && currentMode == GameMode.Race && !isPaused && !isGameOver)
        {
            raceTime += Time.deltaTime;
            UpdateUI();
        }

        // Pause toggle with Escape key
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && gameplayCanvas.activeSelf)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckAndRemoveDuplicateAudioListeners();
        ResetUI();
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

    private void ResetUI()
    {
        mainMenuCanvas.SetActive(true);
        gameModeMenuCanvas.SetActive(false);
        gameplayCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        connectButton.gameObject.SetActive(true);
        disconnectButton.gameObject.SetActive(false);
        connectedLabel.gameObject.SetActive(false);
        addressLabel.gameObject.SetActive(false);
        ethBalanceLabel.gameObject.SetActive(false);
        ClaimedTokenBalanceText.gameObject.SetActive(false);

        ResetGameState();
        Time.timeScale = 0f;
    }

    private void ResetGameState()
    {
        raceTime = 0f;
        score = 0;
        isRacing = false;
        isPaused = false;
        isGameOver = false;
        isGameWon = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        string displayText = currentMode == GameMode.Race ? $"Time: {raceTime:F2}" : $"Score: {score}";
        if (gameplayScoreOrTimeLabel != null) gameplayScoreOrTimeLabel.text = displayText;
        if (gameOverScoreOrTimeLabel != null) gameOverScoreOrTimeLabel.text = displayText;
    }

    public void ShowGameModeMenu()
    {
        mainMenuCanvas.SetActive(false);
        gameModeMenuCanvas.SetActive(true);
    }

    private void StartGame(GameMode mode)
    {
        currentMode = mode;
        Debug.Log("Game Started! Mode: " + currentMode);
        gameModeMenuCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);

        raceTime = 0f;
        score = 0;
        isRacing = (currentMode == GameMode.Race);
        Time.timeScale = 1f;
        AudioManager.Instance.PlayGameplayMusic();
        AudioManager.Instance.PlayWaterSound();
        UpdateUI();
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        AudioManager.Instance.PlayPauseMusic();
        gameplayCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
        Debug.Log("Game Paused");
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioManager.Instance.PlayGameplayMusic();
        pauseMenuCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        Debug.Log("Game Resumed");
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        isGameOver = true;
        Time.timeScale = 0f;
        AudioManager.Instance.PlayGameOverMusic();
        gameplayCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        UpdateUI();
    }

    public void GameWin()
    {
        Debug.Log("Game Won!");
        isGameWon = true;
        Time.timeScale = 0f;
        AudioManager.Instance.PlayGameWinMusic();
        gameplayCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        UpdateUI();
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        ResetGameState();
        mainMenuCanvas.SetActive(true);
        gameModeMenuCanvas.SetActive(false);
        gameplayCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game...");
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void IncrementScore()
    {
        score++;
        UpdateUI();
    }

    public async void Connect()
    {
        if (thirdweb == null || !thirdweb.isActiveAndEnabled)
        {
            Debug.LogError("Cannot connect: ThirdwebManager is not initialized.");
            return;
        }

        try
        {
            var options = new WalletOptions(
                provider: WalletProvider.WalletConnectWallet,
                chainId: ActiveChainId
            );

            Debug.Log("Initiating wallet connection...");
            wallet = await ThirdwebManager.Instance.ConnectWallet(options);

            // If wallet is null, retry once
            if (wallet == null)
            {
                Debug.LogWarning("First connection attempt failed, retrying...");
                wallet = await ThirdwebManager.Instance.ConnectWallet(options);
            }

            walletAddress = await wallet.GetAddress();
            Debug.Log($"Wallet connected successfully! Address: {walletAddress}");

            var balance = await wallet.GetBalance(chainId: ActiveChainId);
            var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
            Debug.Log($"Wallet balance: {balanceEth}");

            // Get the updated token balance and display it with 2 decimal places
            var contract = await ThirdwebManager.Instance.GetContract(ClaimTokenContractAddress, ActiveChainId);
            var tokenBalance = await contract.ERC20_BalanceOf(walletAddress);
            var decimals = await contract.ERC20_Decimals();
            var tokenBalanceFormatted = float.Parse(Utils.ToEth(tokenBalance.ToString(), (int)decimals)).ToString("F2");
            Debug.Log($"Updated token balance for {walletAddress}: {tokenBalanceFormatted}");

            connectButton.gameObject.SetActive(false);
            disconnectButton.gameObject.SetActive(true);
            connectedLabel.gameObject.SetActive(true);
            connectedLabel.text = "Connected";
            addressLabel.gameObject.SetActive(true);
            addressLabel.text = $"{walletAddress.Substring(0, 3)}...{walletAddress.Substring(walletAddress.Length - 3)}";
            ethBalanceLabel.gameObject.SetActive(true);
            ethBalanceLabel.text = $"ETH: {balanceEth}";
            ClaimedTokenBalanceText.gameObject.SetActive(true);
            ClaimedTokenBalanceText.text = $"CYS: {tokenBalanceFormatted}";
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to connect wallet: {ex.Message}");
            wallet = null;
            walletAddress = null;

            connectButton.gameObject.SetActive(true);
            disconnectButton.gameObject.SetActive(false);
            connectedLabel.gameObject.SetActive(false);
            addressLabel.gameObject.SetActive(false);
            ethBalanceLabel.gameObject.SetActive(false);
        }
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

            connectButton.gameObject.SetActive(true);
            disconnectButton.gameObject.SetActive(false);
            connectedLabel.gameObject.SetActive(false);
            addressLabel.gameObject.SetActive(false);
            ethBalanceLabel.gameObject.SetActive(false);
            ClaimedTokenBalanceText?.gameObject.SetActive(false);    
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to disconnect: {ex.Message}");
        }
    }

    public async void ClaimToken()
    {
        if (thirdweb == null)
        {
            Debug.LogError("Cannot claim token: ThirdwebManager is not initialized.");
            return;
        }

        if (wallet == null)
        {
            Debug.LogError("Cannot claim token: Wallet is not connected. Please connect the wallet first.");
            return;
        }

        if (string.IsNullOrEmpty(ClaimTokenContractAddress) || !ClaimTokenContractAddress.StartsWith("0x") || ClaimTokenContractAddress.Length != 42)
        {
            Debug.LogError("Invalid claim token contract address. Please provide a valid Ethereum address (e.g., 0x...)");
            return;
        }

        if (string.IsNullOrEmpty(ClaimTokenAmount) || !float.TryParse(ClaimTokenAmount, out float claimAmount) || claimAmount <= 0)
        {
            Debug.LogError("Invalid claim amount. Please provide a valid amount (e.g., 100)");
            return;
        }

        try
        {
            Debug.Log($"Claiming {ClaimTokenAmount} tokens to {walletAddress} from contract {ClaimTokenContractAddress}...");

            // Ensure the wallet is on the correct chain before claiming
            Debug.Log($"Ensuring wallet is on chain {ActiveChainId}...");
            if (wallet is WalletConnectWallet walletConnect)
            {
                await walletConnect.EnsureCorrectNetwork(ActiveChainId);
            }
            else
            {
                Debug.LogWarning("Wallet is not a WalletConnectWallet; cannot ensure correct network.");
            }

            // Add a delay to ensure the chain switch takes effect
            Debug.Log("Waiting for chain switch to take effect...");
            await Task.Delay(10000); // Wait 10 seconds

            // Create a contract instance for the Drop contract
            var contract = await ThirdwebManager.Instance.GetContract(ClaimTokenContractAddress, ActiveChainId);

            // Get the token's decimals to convert the amount to the correct units
            var decimals = await contract.ERC20_Decimals();
            Debug.Log($"Claim token decimals: {decimals}");

            // Convert the claim amount to the correct units (e.g., 100 tokens with 18 decimals = 100 * 10^18)
            string claimAmountInWei = Utils.ToEth(ClaimTokenAmount);
            Debug.Log($"Converted claim amount to Wei (string): {claimAmountInWei}");

            // Call the DropERC20_Claim function to claim the tokens to the wallet's own address
            var transactionResult = await contract.DropERC20_Claim(wallet, walletAddress, claimAmountInWei);
            //var transactionResult = await contract.DropERC721_Claim()
            Debug.Log($"Tokens claimed successfully! Transaction Hash: {transactionResult.TransactionHash}");
            await Task.Delay(10000); // Wait 10 seconds

            // Get the updated token balance and display it with 2 decimal places
            var tokenBalance = await contract.ERC20_BalanceOf(walletAddress);
            var tokenBalanceFormatted = float.Parse(Utils.ToEth(tokenBalance.ToString(), (int)decimals)).ToString("F2");
            Debug.Log($"Updated token balance for {walletAddress}: {tokenBalanceFormatted}");
            if (ClaimedTokenBalanceText != null)
            {
                ClaimedTokenBalanceText.gameObject.SetActive(true);
                ClaimedTokenBalanceText.text = $"CYS: {tokenBalanceFormatted}";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to claim tokens: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Remove listeners to prevent memory leaks
        startButton.onClick.RemoveAllListeners();
        raceModeButton.onClick.RemoveAllListeners();
        survivalModeButton.onClick.RemoveAllListeners();
        gameModeBackButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
        resumeButton.onClick.RemoveAllListeners();
        pauseRestartButton.onClick.RemoveAllListeners();
        pauseBackButton.onClick.RemoveAllListeners();
        gameOverRestartButton.onClick.RemoveAllListeners();
        gameOverBackButton.onClick.RemoveAllListeners();
        connectButton.onClick.RemoveAllListeners();
        disconnectButton.onClick.RemoveAllListeners();
    }
}