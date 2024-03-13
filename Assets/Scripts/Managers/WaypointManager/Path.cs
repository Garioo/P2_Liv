using System;
using UnityEngine; //unity engine

public class Path : MonoBehaviour
{
    [SerializeField] Transform[] Waypoints;
    [SerializeField] private float[] moveSpeeds;
    [SerializeField] private float[] waitTimes;

    private float timer;
    private int waypointsIndex;

    void Start()
    {
        waypointsIndex = 0;
        timer += Time.deltaTime;
    }

    void Update()
    {
        if (waypointsIndex < Waypoints.Length)
        {
            float distance = Vector2.Distance(transform.position, Waypoints[waypointsIndex].position);

            if (distance > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    Waypoints[waypointsIndex].position, moveSpeeds[waypointsIndex] * Time.deltaTime);
            }
            
            if (timer >= waitTimes[waypointsIndex])
            {
                timer = 0f;
                waypointsIndex++;
            }
            
        }
    }
}