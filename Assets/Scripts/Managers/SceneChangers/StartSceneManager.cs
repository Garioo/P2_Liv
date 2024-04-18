/*
------------------------------
    StartSceneManager.cs
Description: A script to manage the start scene and transition to the next game state when the start button is pressed
------------------------------
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    // Reference to the start button in the UI
    public Button startButton;
    // The next game state to transition to
    public GameManager.GameState nextState;

    // Scene index for the game1 scene
    private const int game1SceneIndex = 2;
    // Scene index for the intro scene
    private const int  introSceneIndex = 1;

    public void LoadGame1()
    {
        //SceneManager.LoadScene(game1SceneIndex);
        StartGame();
    }

    // Function to load the intro scene
    public void LoadIntro()
    {
        // Load the intro scene
        SceneManager.LoadScene(introSceneIndex);
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
