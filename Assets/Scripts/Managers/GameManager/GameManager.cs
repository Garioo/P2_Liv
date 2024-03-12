using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainScene1,
        MainScene2,
        MainScene3,
        MainScene4,
        Spil1,
        Spil2,
        Spil3,
        Spil4,
        Spil5,
        // Add more game states as needed
    }

    public static GameManager instance;

    private GameState gameState = GameState.MainScene1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start the initial game state
        EnterState(gameState);
    }

    // Function to handle state transitions in the game
    public void EnterState(GameState newState)
    {
        gameState = newState;

        Debug.Log("Entering game state: " + gameState);

        switch (gameState)
        {
            /*case GameState.MainScene1:
                LoadSceneWithCutscene("MainScene1", "Cutscene1");
                break;*/

            case GameState.MainScene2:
                LoadSceneWithCutscene("MainScene2", "Cutscene2");
                break;

            case GameState.Spil1:
                LoadSceneWithCutscene("Spil1", "Cutscene3");
                break;

            case GameState.Spil2:
                LoadSceneWithCutscene("Spil2", "Cutscene4");
                break;

            // Add more cases for other game states as needed
        }
    }

    // Function to load a scene with a cutscene
    public void LoadSceneWithCutscene(string sceneName, string cutsceneTag)
    {
        Debug.Log("Loading scene: " + sceneName);
        // Play the cutscene video
        VideoManager.instance.PlayVideo(cutsceneTag);

        // Get the duration of the cutscene video clip
        float cutsceneDuration = VideoManager.instance.GetVideoDuration(cutsceneTag);

        // Call LoadNextScene after the duration of the cutscene video clip
        Invoke ("LoadNextScene", cutsceneDuration);
        Debug.Log("Cutscene duration: " + cutsceneDuration);
    }

    // Function to load the next scene
    private void LoadNextScene()
    {
        // Handle scene loading based on the current game state
        switch (gameState)
        {
            case GameState.MainScene1:
            case GameState.MainScene2:
                StorylineManager.instance.LoadNextScene();
                break;

            case GameState.Spil2:
                StorylineManager.instance.LoadNextScene();
                StartToothbrushGame();
                break;

            // Add more cases for other game states as needed
        }
    }

    // Function to be called when a video finishes playing
    public void OnVideoFinished()
    {
        Debug.Log("Video finished playing");
        // Call LoadNextScene after the video finishes playing (if necessary)
        LoadNextScene();
    }

    // Function to start the toothbrush finding game
    private void StartToothbrushGame()
    {
        // Implement logic to start the toothbrush finding game
    }
}
