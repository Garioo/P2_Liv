/*
--------------------------  
    StorylineManager.cs
 Manages the storyline of the game by loading scenes in sequence.
--------------------------  

 Litterature:
    * Tutorial for Scene Manager by RehopeGames: 
        [Link](https://www.youtube.com/watch?v=4fvQUK2pPds&ab_channel=RehopeGames)
 */


using UnityEngine;
using UnityEngine.SceneManagement;

public class StorylineManager : MonoBehaviour
{
    // Singleton pattern
  public static StorylineManager instance;
    private void Awake()
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

    // Array to hold the names of scenes in the storyline
    public string[] storylineScenes;

    // Index to keep track of the current scene in the storyline
    private int currentSceneIndex = 0;

    // Function to load the next scene in the storyline
    public void LoadNextScene()
    {
        // Check if there are more scenes in the storyline
        if (currentSceneIndex < storylineScenes.Length)
        {
            // Increment the index
            currentSceneIndex++;
            // Load the scene with the specified index
            SceneManager.LoadScene(storylineScenes[currentSceneIndex]);
        }
        else
        {
            Debug.Log("End of storyline reached."); // Optionally, handle end of storyline
        }
    }
}
