using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        StartState,
        MainScene1,
        MainScene2,
        MainScene3,
        MainScene4,
        Game1,
        Game2,
        Game3,
        Game4,
        Game5,
        // Add more game states as needed
    }

    public static GameManager instance;

    private GameState gameState = GameState.StartState;

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
            case GameState.StartState:
                LoadNextState();
                break;

            case GameState.Game1:
                LoadSceneWithCutscene("Game1", "Cutscene1");
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
                LoadNextState();
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

            // Add more cases for other game states as needed
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
            if (videoInfo.cutsceneTag == cutsceneTag)
            {
                hasCutscene = true;
                VideoManager.instance.PlayVideo(cutsceneTag);

                // Get the duration of the cutscene video clip
                float cutsceneDuration = VideoManager.instance.GetVideoDuration(cutsceneTag);

                // Invoke LoadNextScene after the duration of the cutscene video clip
                //Invoke("LoadNextState", cutsceneDuration - 0.5f);
                Debug.Log("Cutscene duration: " + cutsceneDuration);

                break;
            }
        }

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

public void LoadSceneWithoutCutscene(string sceneName)
{

}
    // Function to load the next scene
    private void LoadNextState()
    {
        // Handle scene loading based on the current game state
        switch (gameState)
        {
            case GameState.MainScene1:
                StorylineManager.instance.LoadNextScene();
                break;

            case GameState.MainScene2:
                StorylineManager.instance.LoadNextScene();
                break;
            
            case GameState.Game1:
                Debug.Log("Loading next scene!!");
                StorylineManager.instance.LoadNextScene();
                break;

            case GameState.Game2:
                Debug.Log("Loading next scene!!");
                StorylineManager.instance.LoadNextScene();
                break;
            
            case GameState.Game3:
                Debug.Log("Loading next scene!!");
                StorylineManager.instance.LoadNextScene();
                break;

            // Add more cases for other game states as needed
        }
    }

    // Function to be called when a video finishes playing
    public void VideoNextState()
    {
        Debug.Log("Video finished playing");
        // Call LoadNextScene after the video finishes playing (if necessary)
        LoadNextState();
    }

    public void SetGameManagerReference(SceneChangeTrigger sceneChangeTrigger)
    {
        sceneChangeTrigger.gameManager = this;
    }
}
