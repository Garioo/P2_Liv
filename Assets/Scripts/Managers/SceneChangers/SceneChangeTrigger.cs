using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
     // Reference to the GameManager
    public GameManager gameManager;

    // The target game state to transition to
    public GameManager.GameState targetState;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            // Transition to the target game state
            gameManager.EnterState(targetState);
            Debug.Log("Transitioned to" + targetState);
        }
    }
}
