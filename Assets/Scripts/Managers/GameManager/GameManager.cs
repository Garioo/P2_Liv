using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Enum to define different game states
    [SerializeField] public enum GameState
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

    // Current game state
    private GameState gameState = GameState.MainScene1;

    // Reference to other managers
    public StorylineManager storylineManager;
    public VideoManager videoManager;
    public AudioManager audioManager;
    public WaypointManager waypointManager;

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
        // Start initial game state
        EnterState(gameState);
    }

    // Function to handle state transitions in the game
    public void EnterState(GameState newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case GameState.MainScene1:
                LoadSceneWithCutscene("MainScene1", "Cutscene1");
                break;

            case GameState.MainScene2:
                LoadSceneWithCutscene("MainScene2", "Cutscene2");
                break;

            case GameState.Spil1:
                LoadSceneWithCutscene("Spil1", "Cutscene3");
                break;

            case GameState.Spil2:
                LoadSceneWithoutCutscene("Spil2");
                break;

            // Add more cases for other game states as needed
        }
    }

    // Function to load a scene with a cutscene
    private void LoadSceneWithCutscene(string sceneName, string cutsceneTag)
    {
        // Play the cutscene video
        videoManager.PlayVideo(cutsceneTag);
    }

    // Load the scene after the cutscene
    private void LoadSceneAfterCutscene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to load a scene without a cutscene
    private void LoadSceneWithoutCutscene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to handle completion of video
    public void OnVideoFinished()
    {
        switch (gameState)
        {
            case GameState.MainScene1:
                // Load the next scene in the storyline
                storylineManager.LoadNextScene();
                break;

            case GameState.MainScene2:
                // Load the next scene in the storyline
                storylineManager.LoadNextScene();
                break;

            case GameState.Spil2:
                // Start the toothbrush finding game
                StartToothbrushGame();
                break;

            // Add more cases for other game states as needed
        }
    }

    // Function to start the toothbrush finding game
    private void StartToothbrushGame()
    {
        // Implement logic to start the toothbrush finding game
    }

    // Add more functions as needed to handle other aspects of your game
}
