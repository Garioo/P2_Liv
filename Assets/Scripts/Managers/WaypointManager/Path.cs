/*
------------------------------
Path.cs
Description: This script is responsible for moving an object along a predefined path of waypoints, with customizable movement speeds and wait times at each waypoint.
------------------------------
Litterature:
    * Pathfinding Tutorial by Mohammad Faizan Khan: 
        [Link](https://www.youtube.com/watch?v=oaFJBP4Ld7k&ab_channel=MohammadFaizanKhan)
    * Pathfinding Tutorial by MetalStormGames: 
        [Link](https://www.youtube.com/watch?v=EwHiMQ3jdHw&ab_channel=MetalStormGames)
    * Waypoint Movement Tutorial by MetalStormGames: 
        [Link](https://www.youtube.com/watch?v=RpmQ2xC6gLE&t=69s&ab_channel=MetalStormGames)
*/

using System;
using UnityEngine;

public class Path : MonoBehaviour
{
    // Array of waypoints to follow
    [SerializeField] Transform[] waypoints;
    // Reference to the animator component
    [SerializeField] private Animator animator;
    // Array of movement speeds for each waypoint
    [SerializeField] private float[] moveSpeeds;
    // Array of wait times for each waypoint
    [SerializeField] private float[] waitTimes;
    // Timer to track wait time        
    private float timer;
    // Index of the current waypoint
    private int waypointsIndex;
    // Last position of the object
    private Vector2 lastPosition;

    void Start()
    {
        // Start at the first waypoint
        waypointsIndex = 0;
        // Set the timer to 0
        timer += Time.deltaTime;
        // Set the last position to the current position
        lastPosition = transform.position;
    }

    void Update()
    {
        // Check if there are waypoints to follow
        if (waypointsIndex < waypoints.Length)
        {
            // Calculate the distance between the object and the waypoint
            float distance = Vector2.Distance(transform.position, waypoints[waypointsIndex].position);
            // Check if the object is not at the waypoint
            if (distance > 0.1f)
            {
                // Determine direction of movement
                Vector2 direction = (waypoints[waypointsIndex].position - transform.position).normalized;

                // Set animator parameter based on movement direction
                animator.SetFloat("moveX", direction.x);

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
                // Reset the timer
                timer = 0f;
                // Move to the next waypoint
                waypointsIndex++;
            }
        }

        // Check if the object is not moving
        if (Vector2.Distance(transform.position, lastPosition) <= 0.01f)
        {
            // If not moving, set x parameter to 0
            animator.SetFloat("moveX", 0f);
        }
        // Update the last position
        lastPosition = transform.position;
    }
}