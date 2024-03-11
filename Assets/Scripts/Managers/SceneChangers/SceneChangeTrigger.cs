using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    public GameManager.GameState nextState; // Enum representing the next game state

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the player
        {
            // Get the GameManager instance and transition to the next game state
            GameManager.instance.EnterState(nextState);
        }
    }
}
