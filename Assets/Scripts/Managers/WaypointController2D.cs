using UnityEngine;

[System.Serializable]
public class WaypointData
{
    public Transform waypoint;
    public float speed;
    public float waitTime;
}

public class WaypointController2D : MonoBehaviour
{
    public WaypointData[] waypointData;
    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private float waitTimer = 0f;

    void Start()
    {
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsWaypoint();
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waypointData[currentWaypointIndex].waitTime)
            {
                MoveToNextWaypoint();
                waitTimer = 0f;
            }
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypointData[currentWaypointIndex].waypoint;
        float speed = waypointData[currentWaypointIndex].speed;
        Vector2 direction = ((Vector2)targetWaypoint.position - (Vector2)transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Check if we've reached the waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            isMoving = false;
        }
    }

    void MoveToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1);
        isMoving = true;
    }
}