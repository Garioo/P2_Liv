using UnityEngine;
using UnityEngine.SceneManagement;

public class StorylineManager : MonoBehaviour
{
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
            // Load the next scene
            currentSceneIndex++;
            SceneManager.LoadScene(storylineScenes[currentSceneIndex]);
        }
        else
        {
            Debug.Log("End of storyline reached."); // Optionally, handle end of storyline
        }
    }
}
