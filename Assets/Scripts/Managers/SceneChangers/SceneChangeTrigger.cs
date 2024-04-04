using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
     // Reference to the GameManager
    public GameManager gameManager;

    // The target game state to transition to
    //public GameManager.GameState targetState;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
        Debug.LogError("GameManager not found or GameManager component missing!");
        }

        gameManager.SetGameManagerReference(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            // Transition to the target game state
            gameManager.ChangeGameState();
            //Debug.Log("Transitioned to" + targetState);
        }
    }
    
}
