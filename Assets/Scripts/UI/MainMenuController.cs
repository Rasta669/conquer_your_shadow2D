using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private VisualElement mainMenu;
    private VisualElement optionsMenu;
    private UIDocument uiDocument;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Get UI Elements
        mainMenu = root.Q<VisualElement>("CQYS_startPage");
        optionsMenu = root.Q<VisualElement>("OptionsPage");

        Button startButton = root.Q<Button>("StartButton");
        Button optionsButton = root.Q<Button>("OptionsButton");
        Button quitButton = root.Q<Button>("QuitButton");
        Button backButton = root.Q<Button>("BackButton");

        // Assign button event handlers
        startButton.clicked += StartGame;
        optionsButton.clicked += ShowOptions;
        quitButton.clicked += QuitGame;
        backButton.clicked += ShowMainMenu;

        // Ensure correct visibility at start
        mainMenu.style.display = DisplayStyle.Flex;
        optionsMenu.style.display = DisplayStyle.None;
    }

    private void StartGame()
    {
        mainMenu.style.display = DisplayStyle.None; // Hide the main menu
        GameManager.Instance.StartGame(); // Start the game
    }

    private void ShowOptions()
    {
        mainMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.Flex;
    }

    private void ShowMainMenu()
    {
        mainMenu.style.display = DisplayStyle.Flex;
        optionsMenu.style.display = DisplayStyle.None;
    }

    private void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
