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


using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager class to manage the game state and scene transitions
public class GameManager : MonoBehaviour
{
    // Enum to define the different game states
    public enum GameState
    {
        StartState,
        Intro,
        MainScene1,
        MainScene2,
        MainScene3,
        MainScene4,
        Game1,
        Game2,
        Game3,
        Game4,
        Game5,
        FinalScene,
    }

    // Static reference to the GameManager instance
    public static GameManager instance;
    // Reference to the AudioManager
    AudioManager audioManager;
    // Reference to the VideoManager
    private GameState gameState = GameState.StartState;

    void Awake()
    {
        // Check if an instance of GameManager already exists in the scene 
        if (instance == null)
        {
            // If not, set the instance to this
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
        EnterState(GameState.StartState);
    }


    // Function to handle state transitions in the game
    public void EnterState(GameState newState)
    {
        // Set the new game state
        gameState = newState;

        Debug.Log("Entering game state: " + gameState);

        // Handle scene loading based on the current game state
        switch (gameState)
        {
            case GameState.Game1:
                LoadSceneWithCutscene("Game1", "Cutscene1");
                AudioManager.instance.PlayAudio("event:/Cutscenes/Dream");
                break;

            case GameState.MainScene1:
                LoadSceneWithCutscene("MainScene1", "Cutscene2");
                break;

            case GameState.Game2:
                LoadSceneWithCutscene("Game2", "Cutscene3");
                break;

            case GameState.MainScene2:
                LoadSceneWithCutscene("MainScene2", "Cutscene4");
                break;

            case GameState.Game3:
                StorylineManager.instance.LoadNextScene();;
                break;

            case GameState.MainScene3:
                LoadSceneWithCutscene("MainScene3", "Cutscene6");
                break;

            case GameState.Game4:
                LoadSceneWithCutscene("Game4", "Cutscene7");
                break;
            
            case GameState.MainScene4:
                LoadSceneWithCutscene("MainScene4", "Cutscene8");
                break;

            case GameState.Game5:
                LoadSceneWithCutscene("Game5", "Cutscene9");
                break;
                
             case GameState.FinalScene:
                LoadSceneWithCutscene("FinalScene", "FinalCutscene");
                break;
        }
    }

    // Function to load a scene with a cutscene
   public void LoadSceneWithCutscene(string sceneName, string cutsceneTag)
{
    Debug.Log("Loading scene: " + sceneName);

    // Check if VideoManager.instance is not null
    if (VideoManager.instance != null)
    {
        // Play the cutscene video if the cutscene tag exists
        bool hasCutscene = false;
        foreach (VideoManager.VideoInfo videoInfo in VideoManager.instance.videoInfos)
        {
            // Check if the cutscene tag matches the video info tag
            if (videoInfo.cutsceneTag == cutsceneTag)
            {
                // Set the hasCutscene to true
                hasCutscene = true;
                // Play the cutscene video
                VideoManager.instance.PlayVideo(cutsceneTag);

                // Get the duration of the cutscene video clip
                float cutsceneDuration = VideoManager.instance.GetVideoDuration(cutsceneTag);
                Debug.Log("Cutscene duration: " + cutsceneDuration);
                break;
            }
        }

        // Log an error if the cutscene tag is not found
        if (!hasCutscene)
        {
            Debug.LogError("Cutscene tag not found in VideoManager: " + cutsceneTag);
        }
    }
    else
    {
        Debug.LogError("VideoManager instance is null.");
    }
}

    // Function to load a scene without a cutscene
 public void LoadSceneWithoutCutscene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        StorylineManager.instance.LoadNextScene();
    }

    // Function to be called when a video finishes playing
    public void VideoNextState()
    {
        Debug.Log("Video finished playing");
        // Call LoadNextScene after the video finishes playing (if necessary)
        StorylineManager.instance.LoadNextScene();
    }

    //  Function to set the AudioManager reference
    public void SetGameManagerReference(SceneChangeTrigger sceneChangeTrigger)
    {
        // Set the AudioManager reference
        sceneChangeTrigger.gameManager = this;
    }

    // Function to set the AudioManager reference
    internal void SetGameManagerReference(DialogueManager dialogueManager)
    {
        // Set the AudioManager reference
        throw new NotImplementedException();
    }
}
