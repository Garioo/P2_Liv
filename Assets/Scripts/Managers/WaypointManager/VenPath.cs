using System;
using UnityEngine;

public class VenPath : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    //[SerializeField] private Animator animator;
    [SerializeField] private float[] moveSpeeds;
    [SerializeField] private float[] waitTimes;
   
    private float timer;
    private int waypointsIndex;
    private Vector2 lastPosition; // Store last position to determine movement

    public bool VenWalk = false;
    void Start()
    {
        waypointsIndex = 0;
        timer += Time.deltaTime;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (waypointsIndex < waypoints.Length && VenWalk == true)
        {
            float distance = Vector2.Distance(transform.position, waypoints[waypointsIndex].position);

            if (distance > 0.1f)
            {
                // Determine direction of movement
                Vector2 direction = (waypoints[waypointsIndex].position - transform.position).normalized;

                // Set animator parameter based on movement direction
                //animator.SetFloat("moveX", direction.x);

                // Move the object towards the waypoint
                transform.position = Vector2.MoveTowards(transform.position,
                    waypoints[waypointsIndex].position, moveSpeeds[waypointsIndex] * Time.deltaTime);
            }
            else
            {
                // If the object reaches the waypoint, start the timer
                timer += Time.deltaTime;
            }

            // If the timer exceeds the wait time, proceed to the next waypoint
            if (timer >= waitTimes[waypointsIndex])
            {
                timer = 0f;
                waypointsIndex++;
            }
        }

        // Check if the object is not moving
        if (Vector2.Distance(transform.position, lastPosition) <= 0.01f)
        {
            // If not moving, set x parameter to 0
            //animator.SetFloat("moveX", 0f);
        }

        lastPosition = transform.position;
    }

    
}
