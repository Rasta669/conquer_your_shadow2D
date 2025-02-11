//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance { get; private set; }
//    private bool isPaused = false;

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // Keep across scenes
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    //void Awake()
//    //{
//    //    if (Instance == null)
//    //    {
//    //        GameObject obj = new GameObject("GameManager");
//    //        Instance = obj.AddComponent<GameManager>();
//    //        DontDestroyOnLoad(obj);
//    //    }
//    //}

//    public void StartGame()
//    {
//        Debug.Log("Game Started!");
//        SceneManager.LoadScene("GameScene"); // Replace with actual scene name
//    }

//    public void PauseGame()
//    {
//        isPaused = true;
//        Time.timeScale = 0f; // Stop game time
//        Debug.Log("Game Paused!");
//    }

//    public void ResumeGame()
//    {
//        isPaused = false;
//        Time.timeScale = 1f; // Resume game time
//        Debug.Log("Game Resumed!");
//    }

//    public void GameOver()
//    {
//        Debug.Log("Game Over! Restarting...");
//        Time.timeScale = 0f; // Stop game time
//        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }
//}

using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool isPaused = false;

    public GameObject gameOverUI; // The Game Over UI panel

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
            Debug.Log("GameManager instance created"); // Add a debug log here to confirm it's set up
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void StartGame()
    {
        Debug.Log("Game Started!");
        // Normally, you would load your main game scene, but we skip that now
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Stop game time
        Debug.Log("Game Paused!");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        Debug.Log("Game Resumed!");
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Restarting...");
        Time.timeScale = 0f; // Stop the game time
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Show the Game Over UI
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the time
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Hide the Game Over UI
        }

        // Optionally, reset the game state, and load the current scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene to restart game
    }

    public void QuitGame()
    {
        // Optionally, quit the game (works in build, not in editor)
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
