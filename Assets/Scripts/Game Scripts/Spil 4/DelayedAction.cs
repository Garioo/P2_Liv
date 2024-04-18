/*
-----------------------
DelayedAction.cs
This script is used to transition to a target game state after a delay.
-----------------------
    Litterature:
        * Unity Tutorial  - Coroutines in Unity:
            [Unity Manual for Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)
         * Unity Tutorial  - WaitForSeconds in Unity:
            [Unity Scripting API for WaitForSeconds](https://docs.unity3d.com/ScriptReference/WaitForSeconds.html)
*/

using UnityEngine;

public class DelayedAction : MonoBehaviour
{

    // The target game state to transition to
    public GameManager.GameState targetState;
    // Reference to the GameManager
    public GameManager gameManager;
    // The time to wait before transitioning to the target game state
    public float delayTime;

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
        // Start the coroutine to transition to the target game state after a delay
        StartCoroutine(DoActionAfterDelay());
    }

    System.Collections.IEnumerator DoActionAfterDelay()
    {
        // Wait for 20 seconds
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
