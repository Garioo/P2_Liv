using UnityEngine;
using UnityEngine.SceneManagement;

public class StorylineManager : MonoBehaviour
{
    public void Start()
    {
		DontDestroyOnLoad (transform.gameObject);
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
            SceneManager.LoadScene(storylineScenes[currentSceneIndex]);
            currentSceneIndex++;
        }
        else
        {
            Debug.Log("End of storyline reached."); // Optionally, handle end of storyline
        }
    }
}
