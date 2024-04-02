using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton; // Reference to the start button in the UI
    public GameManager.GameState nextState; // The next game state to transition to

    void Start()
    {
        // Add an onClick listener to the start button
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        // Get the GameManager instance
        GameManager gameManager = FindObjectOfType<GameManager>();

        // Check if the GameManager instance is found
        if (gameManager != null)
        {
            // Enter the next game state
            gameManager.EnterState(nextState);
            FindObjectOfType<AudioManager>().Play("musik");
        }
        else
        {
            Debug.LogError("GameManager object not found in the scene.");
        }
    }
}
