using UnityEngine;
using UnityEngine.SceneManagement;

public class WaypointController : MonoBehaviour
{

    public WaypointData[] waypoints;
    public bool isGameActive = false; // Set this to true when a game is active
    public string activeSceneTag; // Tag indicating the active scene

    void MoveToNextWaypoint(WaypointData waypointData)
    {
        // Check if the waypoint belongs to the active scene
        if (waypointData.sceneTag != activeSceneTag)
        {
            // Waypoint does not belong to the active scene, do not move
            return;
        }

        if (isGameActive)
        {
            // Logic to move the player...
        }
        else
        {
            // Player is not in an active game, do not move
            return;
        }

        // Check if the player has reached the end waypoint
        /*if (reachedEnd)
        {
            // Check with WaypointManager if the player should move to the next scene
            if (WaypointManager.instance.ShouldMoveToNextScene())
            {
                // Trigger the player to move to the next scene
                // Call GameManager's method to move to the next scene
                GameManager.instance.MovePlayerToNextScene();
            }
            else
            {
                // Move the player to the next waypoint
                // Logic to move the player...
            }
        }*/
    }
}
