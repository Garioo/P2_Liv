/*
------------------------------
    SceneChangeTrigger.cs
Description: A script to transition to a target game state after a delay when the player enters the trigger
------------------------------

Litterature:
    * Unity StartCoroutine Documentation:
        [Unity MonoBehaviour StartCoroutine Documentation](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html)
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangeTrigger : MonoBehaviour
{
     // Reference to the GameManager
    public GameManager gameManager;
    public float delayTime;

    // The target game state to transition to
    public GameManager.GameState targetState;

    void Start()
    {
        // Find the GameManager object with the tag "GameManager" and get its GameManager component
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // Check if the GameManager is missing or not found, and log an error if so
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found or GameManager component missing!");
        }

        // Set a reference to this SceneChangeTrigger script in the GameManager
        gameManager.SetGameManagerReference(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider's tag is "Player"
        if (other.CompareTag("Player"))
        {
            // Start a coroutine to perform an action after a delay
            StartCoroutine(DoActionAfterDelay());
        }
    }

    IEnumerator DoActionAfterDelay()
    {
        // Log a message when the player enters the trigger
        Debug.Log("OnTriggerEnter2D");

        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Log a message indicating that the player entered the trigger
        Debug.Log("Player entered trigger");

        // Transition to the target game state in the GameManager
        gameManager.EnterState(targetState);

        // Log a message indicating the transition to the target game state
        Debug.Log("Transitioned to " + targetState);
    }
}
