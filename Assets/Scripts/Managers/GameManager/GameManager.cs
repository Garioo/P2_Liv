/*
--------------------------  
    GameManager.cs
Description: Manages the game state and scene transitions, including loading scenes with or without cutscenes.
--------------------------

Litterature:
    * Unity SceneManager Documentation:
        [Unity SceneManager Documentation](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/SceneManagement.SceneManager.html)
    * Unity DontDestroyOnLoad Documentation:
        [Unity DontDestroyOnLoad Documentation](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Object.DontDestroyOnLoad.html)
    * Unity StartCoroutine Documentation:
        [Unity StartCoroutine Documentation](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.StartCoroutine.html)
    * GameManager Tutorial by Tarodev on YouTube:
        [GameManager Tutorial by Tarodev](https://www.youtube.com/watch?v=4I0vonyqMi8&ab_channel=Tarodev)
    * ChatGPT:
        [ChatGPT](https://openai.com/)
*/

using System; // Importing the System namespace
using UnityEngine; // Importing the UnityEngine namespace
using UnityEngine.SceneManagement; // Importing the UnityEngine.SceneManagement namespace

public class GameManager : MonoBehaviour // Defining a class named GameManager that inherits from MonoBehaviour
{
    public enum GameState // Defining an enumeration named GameState
    {
        StartState, // Enum value for the start state
        Intro, // Enum value for the intro state
        MainScene1, // Enum value for the main scene 1 state
        MainScene2, // Enum value for the main scene 2 state
        MainScene3, // Enum value for the main scene 3 state
        MainScene4, // Enum value for the main scene 4 state
        Game1, // Enum value for the game 1 state
        Game2, // Enum value for the game 2 state
        Game3, // Enum value for the game 3 state
        Game4, // Enum value for the game 4 state
        Game5, // Enum value for the game 5 state
        FinalScene, // Enum value for the final scene state
        // Add more game states as needed
    }

    public static GameManager instance; // Static instance of the GameManager class

    AudioManager audioManager; // Reference to the AudioManager class

    private GameState gameState = GameState.StartState; // Private variable to store the current game state, initialized to the start state

    void Awake() // Unity callback method called when the script instance is being loaded
    {
        if (instance == null) // Check if the instance is null
        {
            instance = this; // Set the instance to this GameManager object
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene
        }
        else
        {
            Destroy(gameObject); // Destroy this object if an instance already exists
        }
    }

    void Start() // Unity callback method called before the first frame update
    {
        // Start the initial game state
        EnterState(GameState.StartState); // Call the EnterState method with the start state
        AudioManager.instance.PlayAudio("event:/title_music"); // Play the title music audio
    }

    // Function to handle state transitions in the game
    public void EnterState(GameState newState) // Method to enter a new game state
    {
        gameState = newState; // Update the current game state

        Debug.Log("Entering game state: " + gameState); // Log the current game state

        switch (gameState) // Switch statement based on the current game state
        {
            case GameState.Game1: // If the game state is Game1
                LoadSceneWithCutscene("Game1", "Cutscene1"); // Load the Game1 scene with Cutscene1
                AudioManager.instance.StopAudio("event:/title_music"); // Stop the title music audio
                AudioManager.instance.PlayAudio("event:/Dream"); // Play the Dream audio
                AudioManager.instance.PlayAudio("event:/white_noise"); // Play the white noise audio
                break;

            case GameState.MainScene1: // If the game state is MainScene1
                LoadSceneWithCutscene("MainScene1", "Cutscene2"); // Load the MainScene1 scene with Cutscene2
                break;

            case GameState.Game2: // If the game state is Game2
                LoadSceneWithCutscene("Game2", "Cutscene3"); // Load the Game2 scene with Cutscene3
                break;

            case GameState.MainScene2: // If the game state is MainScene2
                LoadSceneWithCutscene("MainScene2", "Cutscene4"); // Load the MainScene2 scene with Cutscene4
                break;

            case GameState.Game3: // If the game state is Game3
                LoadNextState(); // Load the next state
                break;

            case GameState.MainScene3: // If the game state is MainScene3
                LoadSceneWithCutscene("MainScene3", "Cutscene6"); // Load the MainScene3 scene with Cutscene6
                break;

            case GameState.Game4: // If the game state is Game4
                LoadSceneWithCutscene("Game4", "Cutscene7"); // Load the Game4 scene with Cutscene7
                break;

            case GameState.MainScene4: // If the game state is MainScene4
                LoadSceneWithCutscene("MainScene4", "Cutscene8"); // Load the MainScene4 scene with Cutscene8
                break;

            case GameState.Game5: // If the game state is Game5
                LoadSceneWithCutscene("Game5", "Cutscene9"); // Load the Game5 scene with Cutscene9
                break;

            case GameState.FinalScene: // If the game state is FinalScene
                LoadSceneWithCutscene("FinalScene", "FinalCutscene"); // Load the FinalScene scene with FinalCutscene
                break;

            // Add more cases for other game states as needed
        }
    }

    // Function to load a scene with a cutscene
    public void LoadSceneWithCutscene(string sceneName, string cutsceneTag) // Method to load a scene with a cutscene
    {
        Debug.Log("Loading scene: " + sceneName); // Log the scene being loaded

        // Check if VideoManager.instance is not null
        if (VideoManager.instance != null) // Check if the VideoManager instance exists
        {
            // Play the cutscene video if the cutscene tag exists
            bool hasCutscene = false; // Flag to check if the cutscene tag exists
            foreach (VideoManager.VideoInfo videoInfo in VideoManager.instance.videoInfos) // Iterate through the videoInfos list in the VideoManager instance
            {
                if (videoInfo.cutsceneTag == cutsceneTag) // If the cutscene tag matches
                {
                    hasCutscene = true; // Set the flag to true
                    VideoManager.instance.PlayVideo(cutsceneTag); // Play the cutscene video

                    // Get the duration of the cutscene video clip
                    float cutsceneDuration = VideoManager.instance.GetVideoDuration(cutsceneTag); // Get the duration of the cutscene video

                    Debug.Log("Cutscene duration: " + cutsceneDuration); // Log the cutscene duration

                    break; // Exit the loop
                }
            }

            if (!hasCutscene) // If the cutscene tag was not found
            {
                Debug.LogError("Cutscene tag not found in VideoManager: " + cutsceneTag); // Log an error message
            }
        }
        else
        {
            Debug.LogError("VideoManager instance is null."); // Log an error message if the VideoManager instance is null
        }
    }

    public void LoadSceneWithoutCutscene(string sceneName) // Method to load a scene without a cutscene
    {
        SceneManager.LoadScene(sceneName); // Load the scene with the given name
    }

    public void LoadNextState() // Method to load the next game state
    {
        // Handle scene loading based on the current game state
        switch (gameState) // Switch statement based on the current game state
        {
            case GameState.StartState: // If the game state is StartState
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.MainScene1: // If the game state is MainScene1
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.MainScene2: // If the game state is MainScene2
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.MainScene3: // If the game state is MainScene3
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.MainScene4: // If the game state is MainScene4
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.Game1: // If the game state is Game1
                Debug.Log("Loading next scene!!"); // Log a message
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.Game2: // If the game state is Game2
                Debug.Log("Loading next scene!!"); // Log a message
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.Game3: // If the game state is Game3
                Debug.Log("Loading next scene!!"); // Log a message
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.Game4: // If the game state is Game4
                Debug.Log("Loading next scene!!"); // Log a message
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.Game5: // If the game state is Game5
                Debug.Log("Loading next scene!!"); // Log a message
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            case GameState.FinalScene: // If the game state is FinalScene
                Debug.Log("Loading next scene!!"); // Log a message
                StorylineManager.instance.LoadNextScene(); // Load the next scene using the StorylineManager
                break;

            // Add more cases for other game states as needed
        }
    }

    // Function to be called when a video finishes playing
    public void VideoNextState() // Method to handle the next state after a video finishes playing
    {
        Debug.Log("Video finished playing"); // Log a message
        // Call LoadNextScene after the video finishes playing (if necessary)
        LoadNextState(); // Load the next state
    }

    public void SetGameManagerReference(SceneChangeTrigger sceneChangeTrigger) // Method to set the GameManager reference in a SceneChangeTrigger
    {
        sceneChangeTrigger.gameManager = this; // Set the GameManager reference in the SceneChangeTrigger
    }

    internal void SetGameManagerReference(DialogueManager dialogueManager) // Method to set the GameManager reference in a DialogueManager
    {
        throw new NotImplementedException(); // Throw a NotImplementedException
    }
}
