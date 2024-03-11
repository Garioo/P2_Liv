using UnityEngine;

[System.Serializable]
public class WaypointData
{
    public Transform waypoint;
    public string sceneTag; // Tag indicating the active scene
    public float speed;
    public float waitTime;
}
