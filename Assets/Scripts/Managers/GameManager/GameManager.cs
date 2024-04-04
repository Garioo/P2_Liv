using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;


public class GameManager : MonoBehaviour
{

    /*public enum GameState
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
        // Add more game states as needed
    }*/

    //SOFIE's

    public GameStateIndex currentGameState;
    public enum GameStateIndex
    {
        StartState = 0,
        Intro = 1,
        Game1 = 2,
        MainScene1 = 3,
        MainScene2 = 5,
        MainScene3 = 7,
        MainScene4 = 9,
        Game2 = 4,
        Game3 = 6,
        Game4 = 8,
        Game5 = 10,
        FinalScene = 11
    }
    //SOFIE's

    public static GameManager instance;

    AudioManager audioManager;

    //private GameState gameState = GameState.StartState;

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
        //SOFIE
        currentGameState = GameStateIndex.StartState;
        Debug.Log(currentGameState);
        //SOFIE

        // Start the initial game state
        //EnterState(GameState.StartState);
    }

    //SOFIE_______________________________________________________________
    public void ChangeGameState()
    {
        switch (currentGameState)
        {
            case GameStateIndex.StartState:
                SceneManager.LoadScene((int)GameStateIndex.Intro);
                currentGameState = GameStateIndex.Intro;
                Debug.Log(currentGameState);
                Debug.Log("TJEK 1");
                break;
        
            case GameStateIndex.Intro:
                Debug.Log("TJEK 2");
                LoadSceneWithCutscene("Game1", "Cutscene1");
                Debug.Log("TJEK 3");
                currentGameState = GameStateIndex.Game1;
                Debug.Log(currentGameState);
                Debug.Log("TJEK 4");
                break;

            case GameStateIndex.Game1:
                Debug.Log("TJEK 5");
                LoadSceneWithCutscene("Game2", "Cutscene3");
                Debug.Log("TJEK 6");
                currentGameState = GameStateIndex.MainScene1;
                Debug.Log("TJEK 7");
                break;

            case GameStateIndex.MainScene1:
                Debug.Log("HALLO");
                LoadSceneWithCutscene("MainScene1", "Cutscene2");
                AudioManager.instance.PlayAudio("event:/Cutscenes/Liv Vågner");
                currentGameState = GameStateIndex.Game2;
                break;
            case GameStateIndex.Game2:
                LoadSceneWithCutscene("Game2", "Cutscene3");
                currentGameState = GameStateIndex.MainScene2;
                break;
            case GameStateIndex.MainScene2:
                LoadSceneWithCutscene("Game3", "Cutscene5");
                currentGameState = GameStateIndex.Game3;
                break;
            case GameStateIndex.Game3:
                LoadSceneWithCutscene("MainScene3", "Cutscene6");
                currentGameState = GameStateIndex.MainScene3;
                break;
            case GameStateIndex.MainScene3:
                LoadSceneWithCutscene("Game4", "Cutscene7");
                currentGameState = GameStateIndex.Game4;
                break;
            case GameStateIndex.Game4:
                LoadSceneWithCutscene("MainScene4", "Cutscene8");
                currentGameState = GameStateIndex.MainScene4;
                break;
            case GameStateIndex.MainScene4:
                LoadSceneWithCutscene("Game5", "Cutscene9");
                currentGameState = GameStateIndex.Game5;
                break;
            case GameStateIndex.Game5:
                LoadSceneWithCutscene("FinalScene", "FinalCutscene");
                currentGameState = GameStateIndex.FinalScene;
                break;
        }
    }

    public void LoadNextScene()
    {
        switch (currentGameState)
        {
            case GameStateIndex.StartState:
                SceneManager.LoadScene((int)GameStateIndex.StartState);
                Debug.Log("Scene StartState loaded");
                break;

            case GameStateIndex.Intro:
                SceneManager.LoadScene((int)GameStateIndex.Intro);
                Debug.Log("Scene GAme1 loaded");
                break;

            case GameStateIndex.Game1:
                SceneManager.LoadScene((int)GameStateIndex.Game1);
                Debug.Log("Scene MainScene1 loaded");
                break;

            case GameStateIndex.MainScene1:
                SceneManager.LoadScene((int)GameStateIndex.MainScene1);
                Debug.Log("Scene Game2 loaded");
                break;
            case GameStateIndex.Game2:
                SceneManager.LoadScene((int)GameStateIndex.Game2);
                Debug.Log("Scene MainScene2 loaded");

                break;
            case GameStateIndex.MainScene2:
                SceneManager.LoadScene((int)GameStateIndex.MainScene2);
                break;
            case GameStateIndex.Game3:
                SceneManager.LoadScene((int)GameStateIndex.Game3);

                break;
            case GameStateIndex.MainScene3:
                SceneManager.LoadScene((int)GameStateIndex.MainScene3);

                break;
            case GameStateIndex.Game4:
                SceneManager.LoadScene((int)GameStateIndex.Game4);

                break;
            case GameStateIndex.MainScene4:
                SceneManager.LoadScene((int)GameStateIndex.MainScene4);

                break;
            case GameStateIndex.Game5:
                SceneManager.LoadScene((int)GameStateIndex.Game5);
                break;

            case GameStateIndex.FinalScene:
                SceneManager.LoadScene((int)GameStateIndex.FinalScene);
                break;
        }
    }
    //SOFIE_______________________________________________________________

    // Function to handle state transitions in the game
    /*public void EnterState(GameState newState)
    {
        gameState = newState;

        Debug.Log("Entering game state: " + gameState);

        switch (gameState)
        {
            case GameState.Intro:
                LoadNextState();
                break;

            case GameState.Game1:
                LoadSceneWithCutscene("Game1", "Cutscene1");
                break;

            case GameState.MainScene1:
                LoadSceneWithCutscene("MainScene1", "Cutscene2");
                AudioManager.instance.PlayAudio("event:/Cutscenes/Liv Vågner");
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
                
             case GameState.FinalScene:
                LoadSceneWithCutscene("FinalScene", "FinalCutscene");
                break;

            // Add more cases for other game states as needed
        }
    }*/

    // Function to load a scene with a cutscene
    public void LoadSceneWithCutscene(string sceneName, string cutsceneTag)
    {
        Debug.Log("Loading scene: " + sceneName);
        // Check if VideoManager.instance is not null

        if (VideoManager.instance != null)
        {
            // Play the cutscene video if the cutscene tag exists
            Debug.Log("TJEK 8");
            bool hasCutscene = false;
            foreach (VideoManager.VideoInfo videoInfo in VideoManager.instance.videoInfos)
            {
                if (videoInfo.cutsceneTag == cutsceneTag)
                {
                    hasCutscene = true;
                    VideoManager.instance.PlayVideo(cutsceneTag);
                    Debug.Log("TJEK 9");

                    // Get the duration of the cutscene video clip
                    float cutsceneDuration = VideoManager.instance.GetVideoDuration(cutsceneTag);

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
/*
 public void LoadSceneWithoutCutscene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
*/
    

    /*
    public void LoadNextState()
    {
        // Handle scene loading based on the current game state
        switch (gameState)
        {
            case GameState.Intro:
                StorylineManager.instance.LoadNextScene();
                break;

            case GameState.StartState:
                StorylineManager.instance.LoadNextScene();
                break;
                
            case GameState.MainScene1:
                StorylineManager.instance.LoadNextScene();
                break;

            case GameState.MainScene2:
                StorylineManager.instance.LoadNextScene();
                break;
            
            case GameState.MainScene3:
                StorylineManager.instance.LoadNextScene();
                break;

            case GameState.MainScene4:
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
            case GameState.Game4:
                Debug.Log("Loading next scene!!");
                StorylineManager.instance.LoadNextScene();
                break;
            
            case GameState.Game5:
                Debug.Log("Loading next scene!!");
                StorylineManager.instance.LoadNextScene();
                break;
            case GameState.FinalScene:
                Debug.Log("Loading next scene!!");
                StorylineManager.instance.LoadNextScene();
                break;

            // Add more cases for other game states as needed
        }
    }*/
    
    // Function to be called when a video finishes playing
    public void VideoNextState()
    {
        Debug.Log("Video finished playing");
        // Call LoadNextScene after the video finishes playing (if necessary)
        ChangeGameState();
    }

    public void SetGameManagerReference(SceneChangeTrigger sceneChangeTrigger)
    {
        sceneChangeTrigger.gameManager = this;
    }

    internal void SetGameManagerReference(DialogueManager dialogueManager)
    {
        throw new NotImplementedException();
    }
}
    