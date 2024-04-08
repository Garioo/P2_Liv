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
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
        Debug.LogError("GameManager not found or GameManager component missing!");
        }

        gameManager.SetGameManagerReference(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DoActionAfterDelay());
        }
    }

    IEnumerator DoActionAfterDelay()
    {
        Debug.Log("OnTriggerEnter2D");
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Player entered trigger");
        // Transition to the target game state
        gameManager.EnterState(targetState);
        Debug.Log("Transitioned to " + targetState);
    }
}

