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
    // The time to wait before transitioning to the target game state
    public float delayTime;
    // The target game state to transition to
    public GameManager.GameState targetState;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
        Debug.LogError("GameManager not found or GameManager component missing!");
        }

        gameManager.SetGameManagerReference(this);
    }
    // When the player enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the coroutine to transition to the target game state after a delay
            StartCoroutine(DoActionAfterDelay());
        }
    }
    // Coroutine to transition to the target game state after a delay
    IEnumerator DoActionAfterDelay()
    {
        Debug.Log("OnTriggerEnter2D");
        // Wait for the delay time
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Player entered trigger");
        // Transition to the target game state
        gameManager.EnterState(targetState);
        Debug.Log("Transitioned to " + targetState);
    }
}

