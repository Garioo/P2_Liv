using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;

    // Add any other necessary fields for waypoint management

    void Awake()
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

    // Check if the player should move to the next scene based on game logic
    public bool ShouldMoveToNextScene()
    {
        // Implement your logic here to determine if the player should move to the next scene
        // Example: return true if specific conditions are met, otherwise return false
        return true; // Modify this line based on your game logic
    }
}
