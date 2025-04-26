using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Thirdweb;
using Thirdweb.Unity;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Numerics;

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
    [SerializeField] private Button nextLevelButton; // Added for Next Level

    // Shared Labels (Game Over)
    [SerializeField] private TextMeshProUGUI gameOverScoreOrTimeLabel;
    [SerializeField] private TextMeshProUGUI connectedLabel;
    [SerializeField] private TextMeshProUGUI addressLabel;
    [SerializeField] private TextMeshProUGUI ethBalanceLabel;

    [SerializeField, Header("Health UI")]
    private List<Image> heartImages; // List of heart UI Images for health display
    [field: SerializeField]
    public string ClaimTokenAmount { get; set; }
    [field: SerializeField, Header("Claim Token Options")]
    public string ClaimTokenContractAddress { get; set; }
    [field: SerializeField]
    public TextMeshProUGUI ClaimedTokenBalanceText { get; set; }

    [field: SerializeField, Header("Claim Nft Options")]
    public string ClaimNftContractAddress { get; set; }
    [field: SerializeField]
    public string ClaimNftAmount { get; set; }
    [field: SerializeField]
    public TextMeshProUGUI ClaimedNFTText { get; set; }

    [field: SerializeField, Header("NFT Display Canvas")]
    public Canvas NftDisplayCanvas { get; set; }
    [field: SerializeField]
    public GameObject NftDisplayPrefab { get; set; }
    [field: SerializeField]
    public Transform NftDisplayParent { get; set; }

    // Scene Progression
    [SerializeField, Header("Level Progression")]
    private List<string> levelScenes; // List of scene names in order
    [SerializeField] private CanvasGroup transitionCanvasGroup; // For fade transition
    [SerializeField] private float transitionDuration = 1f; // Duration of fade in seconds

    private List<GameObject> instantiatedNfts = new List<GameObject>();
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
        nextLevelButton.onClick.AddListener(NextLevel); // Added listener for Next Level

        ResetUI();
    }

    private void Update()
    {
        if (isRacing && currentMode == GameMode.Race && !isPaused && !isGameOver)
        {
            raceTime += Time.deltaTime;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && gameplayCanvas.activeSelf)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckAndRemoveDuplicateAudioListeners();
        ResetPlayerHealth();
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

        // Ensure transition canvas is ready
        if (transitionCanvasGroup != null)
        {
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.gameObject.SetActive(false);
        }
        UpdatePlayerHealth(3); // Initialize health UI
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
        ResetPlayerHealth();
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
        ResetPlayerHealth(); // Ensure health is reset when starting
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

    public void NextLevel()
    {
        if (levelScenes == null || levelScenes.Count == 0)
        {
            Debug.LogError("No level scenes assigned in the Inspector!");
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;
        int currentIndex = levelScenes.IndexOf(currentScene);

        if (currentIndex == -1)
        {
            Debug.LogWarning($"Current scene '{currentScene}' not found in levelScenes list. Loading first level.");
            StartCoroutine(LoadLevelWithTransition(levelScenes[0]));
            return;
        }

        int nextIndex = (currentIndex + 1) % levelScenes.Count; // Loop back to first level if at the end
        string nextScene = levelScenes[nextIndex];
        Debug.Log($"Loading next level: {nextScene} (Index: {nextIndex})");
        StartCoroutine(LoadLevelWithTransition(nextScene));
    }

    private System.Collections.IEnumerator LoadLevelWithTransition(string sceneName)
    {
        if (transitionCanvasGroup == null)
        {
            Debug.LogWarning("Transition CanvasGroup not assigned. Loading scene without transition.");
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        // Fade out (to black or opaque)
        transitionCanvasGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            transitionCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionDuration);
            yield return null;
        }
        transitionCanvasGroup.alpha = 1f;

        // Load the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Fade in (from black to transparent)
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            transitionCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration);
            yield return null;
        }
        transitionCanvasGroup.alpha = 0f;
        transitionCanvasGroup.gameObject.SetActive(false);
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

            Debug.Log($"Ensuring wallet is on chain {ActiveChainId}...");
            if (wallet is WalletConnectWallet walletConnect)
            {
                await walletConnect.EnsureCorrectNetwork(ActiveChainId);
            }
            else
            {
                Debug.LogWarning("Wallet is not a WalletConnectWallet; cannot ensure correct network.");
            }

            Debug.Log("Waiting for chain switch to take effect...");
            await Task.Delay(10000);

            var contract = await ThirdwebManager.Instance.GetContract(ClaimTokenContractAddress, ActiveChainId);
            var decimals = await contract.ERC20_Decimals();
            Debug.Log($"Claim token decimals: {decimals}");

            string claimAmountInWei = Utils.ToEth(ClaimTokenAmount);
            Debug.Log($"Converted claim amount to Wei (string): {claimAmountInWei}");

            var transactionResult = await contract.DropERC20_Claim(wallet, walletAddress, claimAmountInWei);
            Debug.Log($"Tokens claimed successfully! Transaction Hash: {transactionResult.TransactionHash}");
            await Task.Delay(10000);

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

    public async void ClaimNft()
    {
        if (ThirdwebManager.Instance == null)
        {
            Debug.LogError("Cannot claim NFT: ThirdwebManager is not initialized.");
            return;
        }

        if (wallet == null)
        {
            Debug.LogError("Cannot claim NFT: Wallet is not connected. Please connect the wallet first.");
            return;
        }

        if (string.IsNullOrEmpty(ClaimNftContractAddress) || !ClaimNftContractAddress.StartsWith("0x") || ClaimNftContractAddress.Length != 42)
        {
            Debug.LogError("Invalid claim NFT contract address. Please provide a valid Ethereum address (e.g., 0x...)");
            return;
        }

        if (string.IsNullOrEmpty(ClaimNftAmount) || !int.TryParse(ClaimNftAmount, out int claimAmount) || claimAmount <= 0)
        {
            Debug.LogError("Invalid claim amount. Please provide a valid integer amount (e.g., 1)");
            return;
        }

        if (NftDisplayCanvas == null || NftDisplayPrefab == null)
        {
            Debug.LogError("NFT display canvas or prefab not assigned in Inspector.");
            return;
        }

        Transform parentTransform = NftDisplayParent != null ? NftDisplayParent : NftDisplayCanvas.transform;
        Debug.Log($"Using parent transform: {parentTransform.name}");

        try
        {
            if (ClaimedNFTText != null)
            {
                ClaimedNFTText.gameObject.SetActive(true);
                ClaimedNFTText.text = "Claiming...";
            }

            Debug.Log($"Claiming {claimAmount} NFTs to {walletAddress} from contract {ClaimNftContractAddress}...");

            Debug.Log($"Ensuring wallet is on chain {ActiveChainId}...");
            if (wallet is WalletConnectWallet walletConnect)
            {
                await walletConnect.EnsureCorrectNetwork(ActiveChainId);
            }
            else
            {
                Debug.LogWarning("Wallet is not a WalletConnectWallet; cannot ensure correct network.");
            }

            Debug.Log("Waiting for chain switch to take effect...");
            await Task.Delay(20000);

            var contract = await ThirdwebManager.Instance.GetContract(ClaimNftContractAddress, ActiveChainId);
            var transactionResult = await contract.DropERC721_Claim(wallet, walletAddress, claimAmount);
            Debug.Log($"NFTs claimed successfully! Transaction Hash: {transactionResult.TransactionHash}");

            BigInteger tokenBalance = 0;
            int maxAttempts = 5;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                tokenBalance = await contract.ERC721_BalanceOf(walletAddress);
                Debug.Log($"Balance check attempt {attempt + 1}/{maxAttempts}: {tokenBalance} NFTs");
                if (tokenBalance >= claimAmount) break;
                await Task.Delay(5000);
            }

            if (tokenBalance == 0)
            {
                Debug.LogError($"No NFTs owned by {walletAddress} after claim. Check contract logic or transaction.");
                if (ClaimedNFTText != null)
                {
                    ClaimedNFTText.text = $"No NFTs owned. Tx Hash: {transactionResult.TransactionHash}";
                }
                return;
            }

            if (ClaimedNFTText != null)
            {
                ClaimedNFTText.text = $"Claimed! Tx Hash: {transactionResult.TransactionHash}\nBalance: {tokenBalance}";
            }

            ClearNftDisplay();
            Debug.Log("Cleared previous NFT display.");

            List<string> tokenIds = new List<string>();
            try
            {
                var ownedNfts = await contract.ERC1155_GetOwnedNFTs(walletAddress);
                foreach (var nft in ownedNfts)
                {
                    tokenIds.Add(nft.Metadata.Id.ToString());
                    Debug.Log($"Fetched token ID via GetOwnedNFTs: {nft.Metadata.Id}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"GetOwnedNFTs not supported or failed: {ex.Message}. Falling back to tokenOfOwnerByIndex.");
                for (int i = 0; i < tokenBalance; i++)
                {
                    try
                    {
                        BigInteger index = new BigInteger(i);
                        var tokenId = await contract.Read<BigInteger>("tokenOfOwnerByIndex", walletAddress, index);
                        tokenIds.Add(tokenId.ToString());
                        Debug.Log($"Fetched token ID at index {i}: {tokenId}");
                    }
                    catch (System.Exception ex2)
                    {
                        Debug.LogWarning($"Failed to fetch token ID at index {i}: {ex2.Message}");
                    }
                }
            }

            if (tokenIds.Count == 0 && tokenBalance > 0)
            {
                Debug.LogWarning("No token IDs fetched. Assuming sequential IDs based on totalSupply.");
                BigInteger totalSupply;
                try
                {
                    totalSupply = await contract.Read<BigInteger>("totalSupply");
                    Debug.Log($"Contract totalSupply: {totalSupply}");
                }
                catch
                {
                    Debug.LogWarning("Could not fetch totalSupply; assuming totalSupply = tokenBalance.");
                    totalSupply = tokenBalance;
                }

                BigInteger startId = totalSupply - claimAmount;
                if (startId < 0) startId = 0;
                for (int i = 0; i < claimAmount; i++)
                {
                    BigInteger assumedId = startId + i;
                    tokenIds.Add(assumedId.ToString());
                    Debug.Log($"Assumed token ID: {assumedId}");
                }
            }

            Debug.Log($"Total token IDs fetched: {tokenIds.Count}");
            if (tokenIds.Count == 0)
            {
                Debug.LogError("No token IDs available. Verify contract or ownership on Basescan.");
                if (ClaimedNFTText != null)
                {
                    ClaimedNFTText.text = $"No NFTs found for {walletAddress}. Balance: {tokenBalance}";
                }
                return;
            }

            if (NftDisplayCanvas != null)
            {
                NftDisplayCanvas.gameObject.SetActive(true);
                NftDisplayCanvas.enabled = true;
                Debug.Log($"Activated NftDisplayCanvas: {NftDisplayCanvas.name}, IsActive: {NftDisplayCanvas.gameObject.activeInHierarchy}");
            }
            else
            {
                Debug.LogError("NftDisplayCanvas is null!");
                return;
            }

            foreach (var tokenId in tokenIds)
            {
                try
                {
                    Debug.Log($"Processing token ID: {tokenId}");
                    BigInteger tokenIdBigInt = BigInteger.Parse(tokenId);
                    var nft = await contract.ERC721_GetNFT(tokenIdBigInt);
                    string name = nft.Metadata.Name ?? $"NFT #{tokenId}";
                    string description = nft.Metadata.Description ?? "No description";
                    string tokenUri = nft.Metadata.Uri ?? "";
                    Debug.Log($"NFT metadata - TokenID: {tokenId}, Name: {name}, Description: {description}, TokenUri: {tokenUri}");

                    string imageUrl = await GetImageUrlFromTokenUri(tokenUri);
                    Debug.Log($"Image URL for token ID {tokenId}: {imageUrl ?? "null"}");
                    Texture2D imageTexture = null;
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        imageTexture = await LoadImage(imageUrl);
                        Debug.Log($"Image texture loaded for token ID {tokenId}: {(imageTexture != null ? "Success" : "Failed")}");
                    }
                    else
                    {
                        Debug.LogWarning($"No image URL for token ID {tokenId}. Using placeholder.");
                        imageUrl = "https://via.placeholder.com/150";
                        imageTexture = await LoadImage(imageUrl);
                    }

                    GameObject nftDisplay = Instantiate(NftDisplayPrefab, parentTransform);
                    nftDisplay.SetActive(true);
                    instantiatedNfts.Add(nftDisplay);
                    Debug.Log($"Instantiated prefab for token ID {tokenId}: {nftDisplay.name}");

                    TextMeshProUGUI[] textComponents = nftDisplay.GetComponentsInChildren<TextMeshProUGUI>(true);
                    RawImage imageRaw = nftDisplay.GetComponentInChildren<RawImage>(true);

                    TextMeshProUGUI nameText = null;
                    TextMeshProUGUI descText = null;
                    TextMeshProUGUI tokenIdText = null;

                    foreach (var text in textComponents)
                    {
                        string textName = text.gameObject.name.ToLower();
                        if (textName.Contains("name")) nameText = text;
                        else if (textName.Contains("description") || textName.Contains("desc")) descText = text;
                        else if (textName.Contains("tokenid") || textName.Contains("token")) tokenIdText = text;
                    }

                    if (nameText != null)
                    {
                        nameText.text = $"Name: {name}";
                        nameText.color = new Color(1, 1, 1, 1);
                        Debug.Log($"Set NameText for token ID {tokenId}: {nameText.text}");
                    }
                    else Debug.LogWarning($"NameText not found in prefab for token ID {tokenId}");
                    if (descText != null)
                    {
                        descText.text = $"Description: {description}";
                        descText.color = new Color(1, 1, 1, 1);
                        Debug.Log($"Set DescriptionText for token ID {tokenId}: {descText.text}");
                    }
                    else Debug.LogWarning($"DescriptionText not found in prefab for token ID {tokenId}");
                    if (tokenIdText != null)
                    {
                        tokenIdText.text = $"Token ID: {tokenId}";
                        tokenIdText.color = new Color(1, 1, 1, 1);
                        Debug.Log($"Set TokenIdText for token ID {tokenId}: {tokenIdText.text}");
                    }
                    else Debug.LogWarning($"TokenIdText not found in prefab for token ID {tokenId}");
                    if (imageRaw != null)
                    {
                        imageRaw.color = Color.white;
                        imageRaw.texture = imageTexture ?? Texture2D.grayTexture;
                        Debug.Log($"Set RawImage for token ID {tokenId}: {(imageTexture != null ? "Image" : "Placeholder")}");
                    }
                    else Debug.LogWarning($"RawImage not found in prefab for token ID {tokenId}");

                    Debug.Log($"Displayed NFT: TokenID={tokenId}, Name={name}, Description={description}, ImageURL={imageUrl ?? "null"}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to process NFT with TokenID {tokenId}: {ex.Message}");
                }
            }

            if (ClaimedNFTText != null)
            {
                ClaimedNFTText.gameObject.SetActive(true);
                ClaimedNFTText.color = new Color(1, 1, 1, 1);
                ClaimedNFTText.text = $"NFT Balance: {tokenBalance}\nClaimed Tx Hash: {transactionResult.TransactionHash}";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to claim NFTs: {ex.Message}");
            if (ClaimedNFTText != null)
            {
                ClaimedNFTText.text = $"Failed to claim: {ex.Message}";
            }
            if (NftDisplayCanvas != null) NftDisplayCanvas.gameObject.SetActive(false);
        }
    }

    private async Task<string> GetImageUrlFromTokenUri(string tokenUri)
    {
        if (string.IsNullOrEmpty(tokenUri))
        {
            Debug.LogWarning("Token URI is empty or null.");
            return null;
        }

        Debug.Log($"Fetching metadata from token URI: {tokenUri}");
        if (tokenUri.StartsWith("ipfs://"))
        {
            tokenUri = tokenUri.Replace("ipfs://", "https://cloudflare-ipfs.com/ipfs/");
            Debug.Log($"Converted IPFS URI to: {tokenUri}");
        }

        try
        {
            UnityWebRequest request = UnityWebRequest.Get(tokenUri);
            request.timeout = 10;
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to fetch metadata from {tokenUri}: {request.error} (Response Code: {request.responseCode})");
                return null;
            }

            string json = request.downloadHandler.text;
            Debug.Log($"Metadata JSON: {json}");
            JObject metadata = JObject.Parse(json);
            string imageUrl = metadata["image"]?.ToString() ?? metadata["image_url"]?.ToString();
            if (string.IsNullOrEmpty(imageUrl))
            {
                Debug.LogWarning($"No 'image' or 'image_url' field found in metadata: {json}");
                return null;
            }

            Debug.Log($"Parsed image URL: {imageUrl}");
            if (imageUrl.StartsWith("ipfs://"))
            {
                imageUrl = imageUrl.Replace("ipfs://", "https://cloudflare-ipfs.com/ipfs/");
                Debug.Log($"Converted IPFS image URL to: {imageUrl}");
            }

            return imageUrl;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to parse metadata from {tokenUri}: {ex.Message}");
            return null;
        }
    }

    private async Task<Texture2D> LoadImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            Debug.LogWarning("Image URL is empty or null.");
            return null;
        }

        Debug.Log($"Loading image from: {imageUrl}");
        try
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            request.timeout = 10;
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load image from {imageUrl}: {request.error} (Response Code: {request.responseCode})");
                return null;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Debug.Log($"Image loaded successfully: {imageUrl} (Size: {texture.width}x{texture.height})");
            return texture;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load image from {imageUrl}: {ex.Message}");
            return null;
        }
    }

    private void ClearNftDisplay()
    {
        foreach (var nftDisplay in instantiatedNfts)
        {
            if (nftDisplay != null) Destroy(nftDisplay);
        }
        instantiatedNfts.Clear();
        Debug.Log("Cleared NFT display.");
    }

    public void UpdatePlayerHealth(int health)
    {
        if (heartImages != null && heartImages.Count >= 3)
        {
            for (int i = 0; i < heartImages.Count; i++)
            {
                heartImages[i].enabled = i < health; // Enable hearts up to current health
            }
        }
        else
        {
            Debug.LogWarning("Heart Images not assigned or insufficient in NewGameManager.");
        }
    }

    private void ResetPlayerHealth()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.ResetHealth();
        }
        else
        {
            Debug.LogWarning("PlayerMovement not found when resetting health.");
        }
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

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
        nextLevelButton.onClick.RemoveAllListeners(); // Added cleanup
    }
}