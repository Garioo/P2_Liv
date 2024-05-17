using UnityEngine;

public class DelayedAction : MonoBehaviour
{

    public GameManager.GameState targetState;
    public GameManager gameManager;

    private const float delayTime = 10f;

    // Start is called before the first frame update

    void Start()
    {
        // Find the GameManager object in the scene and get its GameManager component
        gameManager = FindObjectOfType<GameManager>();

        // Check if the GameManager object is found
        if (gameManager == null)
        {
            Debug.LogError("GameManager object not found in the scene.");
        }
        StartCoroutine(DoActionAfterDelay());
    }

    System.Collections.IEnumerator DoActionAfterDelay()
    {
        // Wait for 15 seconds
        yield return new WaitForSeconds(delayTime);

        if (gameManager != null)
        {
            // Transition to the target game state
            gameManager.EnterState(targetState);
            Debug.Log("Transitioned to " + targetState);
        }
        else
        {
            Debug.LogError("GameManager object is not initialized.");
        }
    }
}
