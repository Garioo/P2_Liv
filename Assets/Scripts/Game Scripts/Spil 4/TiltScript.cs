/* 
--------------------------  
    TiltScript.cs
Description: Script for tilting an object based on device acceleration
--------------------------

Literature:
    * Unity Youtube Tutorial by Alexander Zotov- Accelerometer and Gyroscope:
        [Link](https://www.youtube.com/watch?v=wpSm2O2LIRM&t=9s&ab_channel=AlexanderZotov)
*/

using UnityEngine;

public class TiltScript : MonoBehaviour
{
    Rigidbody2D rb; // Reference to the Rigidbody2D component
    float dirx; // Variable to store the horizontal direction
    public float moveSpeed; // Speed at which the object moves

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the object
        moveSpeed = Random.Range(3f, 18f); // Set a random move speed between 3 and 18
    }

    void Update()
    {
        dirx = Input.acceleration.x * moveSpeed; // Get the horizontal acceleration from the device and multiply it by the move speed
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -6.5f, 6.5f), transform.position.y); // Clamp the object's x position between -6.5 and 6.5
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirx, 0f); // Set the object's velocity based on the horizontal direction
    }
}
