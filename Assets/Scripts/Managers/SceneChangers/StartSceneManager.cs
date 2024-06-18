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
    public Button startButton; // Reference to the start button in the UI
    public GameManager.GameState nextState; // The next game state to transition to

    private const int game1SceneIndex = 2;
    private const int  introSceneIndex = 1;
    // Reference to the audio manager

    public void Start()
    {
        // Add a listener to the start button
        startButton.onClick.AddListener(StartGame);
    }
    public void LoadGame1()
    {
        //SceneManager.LoadScene(game1SceneIndex);
        StartGame();
    }

    public void LoadIntro()
    {
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
