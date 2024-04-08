using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton; // Reference to the start button in the UI
    public GameManager.GameState nextState; // The next game state to transition to

    private const int game1SceneIndex = 2;
    private const int  introSceneIndex = 1;

    public void LoadGame1()
    {
        SceneManager.LoadScene(game1SceneIndex);
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene(introSceneIndex);
    }

    void Start()
    {
        // Add an onClick listener to the start button
        //startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        // Get the GameManager instance
        GameManager gameManager = FindObjectOfType<GameManager>();

        // Check if the GameManager instance is found
        if (gameManager != null)
        {

            // Transition to the next game state
            gameManager.EnterState(nextState);  
    
        }
        else
        {
            Debug.LogError("GameManager object not found in the scene.");
        }
    }
}
