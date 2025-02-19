////using UnityEngine;
////using UnityEngine.UIElements;

////public class MainMenuController : MonoBehaviour
////{
////    private VisualElement mainMenu;
////    private VisualElement optionsMenu;
////    private UIDocument uiDocument;
////    private VisualElement pauseMenu;

////    private void OnEnable()
////    {
////        uiDocument = GetComponent<UIDocument>();
////        var root = uiDocument.rootVisualElement;

////        // Get UI Elements
////        mainMenu = root.Q<VisualElement>("CQYS_startPage");
////        optionsMenu = root.Q<VisualElement>("OptionsPage");
////        pauseMenu = root.Q<VisualElement>("PauseMENU");

////        Button startButton = root.Q<Button>("StartButton");
////        Button optionsButton = root.Q<Button>("OptionsButton");
////        Button quitButton = root.Q<Button>("QuitButton");
////        Button backButton = root.Q<Button>("BackButton");

////        // Assign button event handlers
////        startButton.clicked += StartGame;
////        optionsButton.clicked += ShowOptions;
////        quitButton.clicked += QuitGame;
////        backButton.clicked += ShowMainMenu;

////        // Ensure correct visibility at start
////        mainMenu.style.display = DisplayStyle.Flex;
////        pauseMenu.style.display = DisplayStyle.None;
////        optionsMenu.style.display = DisplayStyle.None;
////    }

////    private void StartGame()
////    {
////        mainMenu.style.display = DisplayStyle.None; // Hide the main menu
////        pauseMenu.style.display = DisplayStyle.Flex;
////        GameManager.Instance.StartGame(); // Start the game
////    }

////    private void ShowOptions()
////    {
////        mainMenu.style.display = DisplayStyle.None;
////        pauseMenu.style.display = DisplayStyle.None;
////        optionsMenu.style.display = DisplayStyle.Flex;
////    }

////    private void ShowMainMenu()
////    {
////        mainMenu.style.display = DisplayStyle.Flex;
////        pauseMenu.style.display = DisplayStyle.None;
////        optionsMenu.style.display = DisplayStyle.None;
////    }

////    private void QuitGame()
////    {
////        GameManager.Instance.QuitGame();
////    }
////}


//using UnityEngine;
//using UnityEngine.UIElements;

//public class MainMenuController : MonoBehaviour
//{
//    private VisualElement mainMenu;
//    private VisualElement optionsMenu;
//    private UIDocument uiDocument;
//    private VisualElement pauseMenu;
//    private Label scoreLabel; // To show the score on the pause menu

//    private void OnEnable()
//    {
//        uiDocument = GetComponent<UIDocument>();
//        var root = uiDocument.rootVisualElement;

//        // Get UI Elements
//        mainMenu = root.Q<VisualElement>("CQYS_startPage");
//        optionsMenu = root.Q<VisualElement>("OptionsPage");
//        pauseMenu = root.Q<VisualElement>("PauseMENU");
//        scoreLabel = pauseMenu.Q<Label>("ScoreLabel"); // Assuming "ScoreLabel" is the name of your score element

//        Button startButton = root.Q<Button>("StartButton");
//        Button optionsButton = root.Q<Button>("OptionsButton");
//        Button quitButton = root.Q<Button>("QuitButton");
//        Button backButton = root.Q<Button>("BackButton");
//        Button resumeButton = pauseMenu.Q<Button>("ResumeButton"); // Assuming you have a Resume button in the pause menu
//        Button pauseButton = pauseMenu.Q<Button>("PauseButton");

//        // Assign button event handlers
//        startButton.clicked += StartGame;
//        optionsButton.clicked += ShowOptions;
//        quitButton.clicked += QuitGame;
//        backButton.clicked += ShowMainMenu;
//        pauseButton.clicked += PauseGame;
//        //resumeButton.clicked += ResumeGame; // Resume game when clicked

//        // Ensure correct visibility at start
//        mainMenu.style.display = DisplayStyle.Flex;
//        pauseMenu.style.display = DisplayStyle.None;
//        optionsMenu.style.display = DisplayStyle.None;
//    }

//    private void StartGame()
//    {
//        mainMenu.style.display = DisplayStyle.None; // Hide the main menu
//        pauseMenu.style.display = DisplayStyle.Flex; // Show pause menu
//        //UpdateScoreDisplay(); // Display current score in the pause menu

//        // Pause game time to simulate "pause"
//        Time.timeScale = 0f;

//        // Call GameManager to start the game (if needed)
//        GameManager.Instance.StartGame();
//    }

//    private void ShowOptions()
//    {
//        mainMenu.style.display = DisplayStyle.None;
//        pauseMenu.style.display = DisplayStyle.None;
//        optionsMenu.style.display = DisplayStyle.Flex;
//    }

//    private void ShowMainMenu()
//    {
//        mainMenu.style.display = DisplayStyle.Flex;
//        pauseMenu.style.display = DisplayStyle.None;
//        optionsMenu.style.display = DisplayStyle.None;

//        // Unpause the game when returning to the main menu
//        Time.timeScale = 1f;
//    }

//    private void QuitGame()
//    {
//        GameManager.Instance.QuitGame();
//    }

//    void PauseGame()
//    {
//        GameManager.Instance.PauseGame();
//    }

//    // Call this method when the user presses "Resume" from the pause menu
//    public void ResumeGame()
//    {
//        GameManager.Instance.ResumeGame();
//        //pauseMenu.style.display = DisplayStyle.None; // Hide pause menu
//        //Time.timeScale = 1f; // Resume game
//    }

//    // Method to update the score display in the pause menu
//    //private void UpdateScoreDisplay()
//    //{
//    //    // Assuming GameManager handles the score logic
//    //    int score = GameManager.Instance.GetScore();
//    //    scoreLabel.text = "Score: " + score.ToString();
//    //}
//}


//using UnityEngine;
//using UnityEngine.UIElements;

//public class MainMenuController : MonoBehaviour
//{
//    private VisualElement mainMenu;
//    private VisualElement optionsMenu;
//    private UIDocument uiDocument;
//    private VisualElement pauseMenu;

//    private void OnEnable()
//    {
//        uiDocument = GetComponent<UIDocument>();
//        var root = uiDocument.rootVisualElement;

//        // Get UI Elements
//        mainMenu = root.Q<VisualElement>("CQYS_startPage");
//        optionsMenu = root.Q<VisualElement>("OptionsPage");
//        pauseMenu = root.Q<VisualElement>("PauseMENU");

//        Button startButton = root.Q<Button>("StartButton");
//        Button optionsButton = root.Q<Button>("OptionsButton");
//        Button quitButton = root.Q<Button>("QuitButton");
//        Button backButton = root.Q<Button>("BackButton");

//        // Assign button event handlers
//        startButton.clicked += StartGame;
//        optionsButton.clicked += ShowOptions;
//        quitButton.clicked += QuitGame;
//        backButton.clicked += ShowMainMenu;

//        // Ensure correct visibility at start
//        mainMenu.style.display = DisplayStyle.Flex;
//        pauseMenu.style.display = DisplayStyle.None;
//        optionsMenu.style.display = DisplayStyle.None;
//    }

//    private void StartGame()
//    {
//        mainMenu.style.display = DisplayStyle.None; // Hide the main menu
//        pauseMenu.style.display = DisplayStyle.Flex;
//        GameManager.Instance.StartGame(); // Start the game
//    }

//    private void ShowOptions()
//    {
//        mainMenu.style.display = DisplayStyle.None;
//        pauseMenu.style.display = DisplayStyle.None;
//        optionsMenu.style.display = DisplayStyle.Flex;
//    }

//    private void ShowMainMenu()
//    {
//        mainMenu.style.display = DisplayStyle.Flex;
//        pauseMenu.style.display = DisplayStyle.None;
//        optionsMenu.style.display = DisplayStyle.None;
//    }

//    private void QuitGame()
//    {
//        GameManager.Instance.QuitGame();
//    }
//}


using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private VisualElement mainMenu;
    private VisualElement optionsMenu;
    private UIDocument uiDocument;
    private VisualElement pauseMenu;
    private VisualElement resumeMenu;
    private Label scoreLabel; // To show the score on the pause menu

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Get UI Elements
        mainMenu = root.Q<VisualElement>("CQYS_startPage");
        optionsMenu = root.Q<VisualElement>("OptionsPage");
        pauseMenu = root.Q<VisualElement>("PauseMENU");
        resumeMenu = root.Q<VisualElement>("ResumeMenu");
        scoreLabel = pauseMenu.Q<Label>("ScoreLabel"); // Assuming "ScoreLabel" is the name of your score element

        Button startButton = root.Q<Button>("StartButton");
        Button optionsButton = root.Q<Button>("OptionsButton");
        Button quitButton = root.Q<Button>("QuitButton");
        Button backButton = root.Q<Button>("BackButton");
        Button resumeButton = resumeMenu.Q<Button>("ResumeButton"); // Assuming you have a Resume button in the pause menu
        Button pauseButton = pauseMenu.Q<Button>("PauseButton");

        // Assign button event handlers
        startButton.clicked += StartGame;
        optionsButton.clicked += ShowOptions;
        quitButton.clicked += QuitGame;
        backButton.clicked += ShowMainMenu;
        pauseButton.clicked += PauseGame;
        resumeButton.clicked += ResumeGame; // Resume game when clicked

        // Ensure correct visibility at start
        mainMenu.style.display = DisplayStyle.Flex;
        pauseMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.None;
    }

    private void StartGame()
    {
        mainMenu.style.display = DisplayStyle.None; // Hide the main menu
        pauseMenu.style.display = DisplayStyle.Flex; // Show pause menu
        //UpdateScoreDisplay(); // Display current score in the pause menu

        // Pause game time to simulate "pause"
        Time.timeScale = 0f;

        // Call GameManager to start the game (if needed)
        GameManager.Instance.StartGame();
    }

    private void ShowOptions()
    {
        mainMenu.style.display = DisplayStyle.None;
        pauseMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.Flex;
    }

    private void ShowMainMenu()
    {
        mainMenu.style.display = DisplayStyle.Flex;
        pauseMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.None;

        // Unpause the game when returning to the main menu
        Time.timeScale = 1f;
    }

    private void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }

    // Call this method when the user presses "Resume" from the pause menu
    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
        //pauseMenu.style.display = DisplayStyle.None; // Hide pause menu
        //Time.timeScale = 1f; // Resume game
    }

    // Method to update the score display in the pause menu
    //private void UpdateScoreDisplay()
    //{
    //    // Assuming GameManager handles the score logic
    //    int score = GameManager.Instance.GetScore();
    //    scoreLabel.text = "Score: " + score.ToString();
    //}
}
